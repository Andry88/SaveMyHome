using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaveMyHome.Filters;
using SaveMyHome.Models;
using SaveMyHome.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data.Entity;
using SaveMyHome.Abstract;

namespace SaveMyHome.Controllers
{
    [ForUsers]
    public class FloodController : Controller
    {
        INotifyProcessor notifyProcessor;

        public FloodController(INotifyProcessor notifyProcessor)
        {
            this.notifyProcessor = notifyProcessor;
        }

        #region Notify
        /// <summary>
        /// Взависимости от статуса оповещающего генерирует список оповещаемых квартир, главное сообщение по умолчанию
        /// или гененирует соответствующие сообщения при попытке пользователей с первого или последнего этажей 
        /// оповестить жителей несуществующих квартир. А также завершает событие в сценарии Меня топят, 
        /// когда после определения виновника потопа не осталось неоповещенных квартир
        /// </summary>
        /// <param name="problemStatus"> статус оповещающего пользователя</param>
        /// <param name="headMsg">Сообщение оповещающего пользователя</param>
        /// <param name="apartmentsForNotify">Список оповещаемых квартир</param>
        /// <param name="isSecond">Флаг, указывающий является ли данное оповещение вторым в данном событии</param>
        /// <returns></returns>
        public ActionResult Notify(ProblemStatus problemStatus, bool isSecond = false)
        {
            string headMsg = null;

            //Получение текущей квартиры
            Apartment currApart = CurrUser.Apartment;

            //Проверка статуса оповещаеющего 
            if (problemStatus == ProblemStatus.Culprit)
            {
                //если оповещающий не находится на первом этаже,
                //то создается сообщение по-умолчанию для виновника потопа,
                //иначе выводится сообщение о невозможности затопления им каких-либо квартир
                if (currApart.Floor != 1)
                    headMsg = MessagesSource.CulpritHeadMessage;
                else
                {
                    TempData["msg"] = MessagesSource.ForFirstFloorUser;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {   //если оповещающий не находится на последнем этаже,
                //то создается сообщение по-умолчанию для жертвы потопа,
                //иначе выводится сообщение о невозможности затопления его кем-либо
                if (currApart.Floor != House.FloorsCount)
                    headMsg = MessagesSource.VictimHeadMessage;
                else
                {
                    TempData["msg"] = MessagesSource.ForLastFloorUser;
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(new NotifyVM
            {
                Apartments = GetApartmentsForNotify(currApart, problemStatus, db),
                ProblemStatus = problemStatus,
                HeadMessage = headMsg,
                ProblemId = db.Problems.SingleOrDefault(p => p.Name == "Потоп").Id,
                IsSecondNotify = isSecond
            });
        }

        /// <summary>
        /// Добавляет информацию о событии в БД
        /// и отображает оповестившему схемы этажей с оповещенными квартирами
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Notify(NotifyVM model)
        {
            //Проверка успешности валидации модели
            if (ModelState.IsValid)
            {
                //Создание заглавного сообщения
                Message msg = new Message
                {
                    Text = model.HeadMessage,
                    Time = DateTime.Now,
                    User = CurrUser,
                    IsHead = true
                };

                //получение статуса оповещаемых квартир
                var statusOfnotifiedApartments = model.ProblemStatus == ProblemStatus.Culprit ?
                                                 ProblemStatus.PotentialVictim : ProblemStatus.PotentialCulprit;

                //если текущее оповещение - второе в рамках данного события
                if (model.IsSecondNotify)
                {
                    //Добавление заглавного сообщения в БД
                    msg.EventId = CurrEventId;
                    db.Messages.Add(msg);
                    db.SaveChanges();

                    //Обновление реакции текущей квартиры
                    var currReaction = db.Reactions.FirstOrDefault(r => r.EventId == CurrEventId 
                                                                     && r.ApartmentNumber == CurrUser.ApartmentNumber);
                    currReaction.ProblemStatus = model.ProblemStatus;
                    currReaction.Notifier = true;
                    currReaction.Reacted = true;
                    db.Entry(currReaction).State = EntityState.Modified;
                    db.SaveChanges();

                    //Обновление реакций квартир неуспевших ответить
                    var notAnsweredReactions = db.Reactions.Where(r => r.EventId == CurrEventId && !r.Reacted);

                    foreach (var r in notAnsweredReactions)
                    {
                        r.ProblemStatus = ProblemStatus.None;
                        r.Reacted = true;
                        db.Entry(r).State = EntityState.Modified;
                    }
                    db.SaveChanges();

                    //Добавление реакций оповещаемых квартир
                    foreach (var a in model.Apartments)
                    {
                        db.Entry(new Reaction
                        {
                            ApartmentNumber = a,
                            EventId = CurrEventId,
                            ProblemStatus = statusOfnotifiedApartments,
                        })
                        .State = EntityState.Added;
                    }
                    db.SaveChanges();
                }
                else
                {   //Создание реакции текушей квартиры
                    var reactions = new List<Reaction>
                    {
                        new Reaction
                        {
                            ApartmentNumber = CurrUser.ApartmentNumber,
                            ProblemStatus = model.ProblemStatus,
                            Notifier = true,
                            Reacted = true
                        }
                    };

                    //Создание реакций оповещенных квартир
                    foreach (var a in model.Apartments)
                        reactions.Add(new Reaction
                        {
                            ApartmentNumber = a,
                            ProblemStatus = statusOfnotifiedApartments,
                            Reacted = false
                        });

                    //Создание события
                    var _event = new Event
                    {
                        Start = DateTime.Now,
                        ProblemId = model.ProblemId,
                        Reactions = reactions,
                        Messages = new List<Message> { msg }
                    };

                    //Добавление всех созданных данных в БД
                    db.Entry(_event).State = EntityState.Added;
                    db.SaveChanges();
                }
                
                //Оповестить через email
                if (model.SendEmail)
                    notifyProcessor.ProcessNotify(msg, GetNotifiedApartments().Select(r => r.Apartment).ToList());

                return View("ReactionResult", new AnswerVM
                {
                    Apartments = model.Apartments,
                    HeadMessage = msg,
                    VisitorProblemStatus = model.ProblemStatus
                });
            }
            else //если модель не прошла валидацию,
            {   //то в представление повторно отправляется список оповещаемых квартир
                model.Apartments = GetApartmentsForNotify(CurrUser.Apartment, model.ProblemStatus, db);
                return View(model);
            }
        }
        #endregion

        #region Answer
        /// <summary>
        /// Генерирует схемы этажей с оповещенными квартирами,
        /// главное и ответные сообщения
        /// </summary>
        /// <param name="notifyStatus">статус квартиры данного пользователя </param>
        /// <returns></returns>
        public ActionResult Answer(ProblemStatus notifyStatus = ProblemStatus.None)
        {
            ApplicationUser currUser = CurrUser;

            if (notifyStatus == ProblemStatus.PotentialCulprit)
                return View("PotentialCulpritAnswer", new AnswerVM
                {
                    VisitorProblemStatus = notifyStatus,
                    IsFromPotentialCulprit = true
                });
            else 
                return View("PotentialVictimAnswer", new AnswerVM
                {
                    VisitorProblemStatus = notifyStatus,
                    IsFromPotentialCulprit = false
                });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Answer(AnswerVM model)
        {
            //Проверка успешности валидации модели
            if (ModelState.IsValid)
            {
                //Изменение инфы о реакции текущей оповещенной квартиры
                var reaction = db.Reactions.OrderByDescending(r => r.Id)
                                           .FirstOrDefault(r => r.ApartmentNumber == CurrUser.ApartmentNumber);
                reaction.ProblemStatus = model.VisitorProblemStatus;
                reaction.Reacted = true;
                db.Entry(reaction).State = EntityState.Modified;
                db.SaveChanges();

                //Если текущая оповещенная квартира не является виновником проблемы,
                if (model.VisitorProblemStatus != ProblemStatus.Culprit)
                {
                    //Создание и добавление ответного сообщения в БД
                    var msg = new Message
                    {
                        Text = model.CurrAnswer,
                        Time = DateTime.Now,
                        User = CurrUser,
                        IsHead = false,
                        EventId = CurrEventId
                    };

                    db.Messages.Add(msg);
                    db.SaveChanges();

                    //если нет неответивших квартир, событие завершается
                    if (GetNotifiedApartments().All(r => r.Reacted))
                        return RedirectToAction("EventCompletion");

                    //иначе генерируется страница с сообщением текущего пользователя
                    //подтверждающая, что его сообщение отправлено
                    TempData["msg"] = MessagesSource.MessageIsSent;

                    return View("ReactionResult");
                }
                else //Если текущая оповещенная квартира является инициатором проблемы,
                {   //то из таблица Reactions удаляются все записи касающиеся неуспевших ответить квартир
                    var reactions = GetNotifiedApartments().Where(r => !r.Reacted);
                    foreach (var r in reactions)
                    {
                        db.Reactions.Attach(r);
                        db.Entry(r).State = EntityState.Deleted;
                    }
                    db.SaveChanges();

                    //и запускается сценарий "Я топлю"
                    return RedirectToAction("Notify", new { problemStatus = model.VisitorProblemStatus, isSecond = true });
                }
            }
            else {
                    return model.IsFromPotentialCulprit 
                    ? View("PotentialCulpritAnswer", model)
                    : View("PotentialVictimAnswer", model);
            }
        }
        #endregion

        //добавляет в таблицу ProblemHistory время завершения текущего события
        //и выводит страницу с историей событий вместе с текущим
        public RedirectToRouteResult EventCompletion()
        {
            var currEvent = db.Events.FirstOrDefault(i => i.Id == CurrEventId);
            currEvent.End = DateTime.Now;
            db.SaveChanges();

            TempData["msg"] = MessagesSource.ProblemIsFixed;

            return RedirectToAction("ProblemHistory", "Problem");
        }

        #region ChildActions
        //Выводит список оповещенных квартир в частичное представление
        [ChildActionOnly]
        public PartialViewResult FloorSchema()=>  PartialView(GetNotifiedApartments());

        [ChildActionOnly]
        //Выводит главное сообщение в частичное представление
        public PartialViewResult HeadMessage()
        {
            var headMessage = db.Messages.FirstOrDefault(m => m.EventId == CurrEventId && m.IsHead);
            return PartialView(headMessage);
        }

        [ChildActionOnly]
        //Выводит ответные сообщения в частичное представление
        public PartialViewResult AnswerMessages()
        {
            var answerMessages = db.Messages.Where(m => m.EventId == CurrEventId && !m.IsHead).ToList();
            return PartialView(answerMessages);
        }
        #endregion

        #region Helpers
        private ApplicationUserManager UserManager=> HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private ApplicationUser CurrUser => UserManager.FindByEmail(User.Identity.Name);
        private ApplicationDbContext db => HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        private int CurrEventId => CurrUser.Apartment.Reactions.Last().EventId;

        //Генерирует квартиры для оповещения исходя из статуса оповещающего 
        //и потенциальной уязвимости к потопу из-за своего положения относительно квартиры оповещающего
        private IList<int> GetApartmentsForNotify(Apartment currApart, ProblemStatus problemStatus, ApplicationDbContext db)
        {
            var apartmentsForNotify = new List<int>();

            if (problemStatus == ProblemStatus.Culprit)
            {
                //Извлечение всех квартир из БД вместе с их статусами
                var allApartments = db.Apartments.Include(r => r.Reactions).ToList();

                for (int i = 1; i <= House.AlertRangeVertical; i++)
                {
                    var res = allApartments.Where(a =>
                     a.Number >= (currApart.Number - House.ApartmentsWithinFloor * i - House.AlertRangeHorizontal)
                     && a.Number <= (currApart.Number - House.ApartmentsWithinFloor * i + House.AlertRangeHorizontal)
                     && a.Floor == currApart.Floor - i 
                     //для второго оповещения в одном событии при сценарии "меня топят/я топлю" 
                     //извлекаются все неответившие квартиры из текущего события
                     && (a.Reactions.Count == 0 || a.Reactions.Any(r => r.EventId == CurrEventId && !r.Reacted)))
                     .OrderBy(a => a.Number).Select(a => a.Number).ToList();

                    apartmentsForNotify.AddRange(res);
                }
            }
            else
            {
                //Извлечение всех квартир из БД
                var allApartments = db.Apartments.ToList();

                for (int i = 1; i <= House.AlertRangeVertical; i++)
                {
                    var res = allApartments.Where(a =>
                    a.Number >= (currApart.Number + House.ApartmentsWithinFloor * i - House.AlertRangeHorizontal)
                    && a.Number <= (currApart.Number + House.ApartmentsWithinFloor * i + House.AlertRangeHorizontal)
                    && a.Floor == currApart.Floor + i)
                    .OrderBy(a => a.Number)
                    .Select(a => a.Number).ToList();

                    apartmentsForNotify.AddRange(res);
                }
            }
            return apartmentsForNotify;
        }

        //Получает из БД оповещенные квартиры
        private IEnumerable<Reaction> GetNotifiedApartments()
        {
           return db.Reactions.Include(r => r.Apartment).Where(r => r.EventId == CurrEventId && !r.Notifier)
                              .OrderBy(r => r.ApartmentNumber);
        }

        #endregion
    }
}
