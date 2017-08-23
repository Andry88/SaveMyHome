using SaveMyHome.Models;
using SaveMyHome.ViewModels;
using SaveMyHome.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SaveMyHome.Abstract;
using SaveMyHome.Infrastructure.Repository.Abstract;
using Resources;
using SaveMyHome.Filters;

namespace SaveMyHome.Controllers
{
    public class FloodController : Controller
    {
        IUnitOfWork Database;
        INotifyProcessor notifyProcessor;

        public FloodController(IUnitOfWork unitOfWork, INotifyProcessor notifyProcessor)
        {
            Database = unitOfWork;
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
            
            Apartment currApart = Database.ClientProfiles.CurrentClientProfile.Apartment;

            //если житель одной и той же квартиры повторно оповещает о потопе,
            //выводится об этом сообщение
            if (Database.Reactions.All.Count() > 0)
            {
                Reaction currentRection = Database.Reactions.CurrentReaction;
                if (currApart.Reactions.Any(r => r.Notifier == true && isSecond == false
                && r.EventId == currentRection.EventId && r.Event.End == null))
                {
                    TempData["msg"] = Resource.ForTheSameNotifierAtDontCompletedEvent;
                    return RedirectToAction("Index", "Home");
                }
            }

            if (problemStatus == ProblemStatus.Culprit)
            {
                //если оповещающий не находится на первом этаже,
                //то создается сообщение по-умолчанию для виновника потопа,
                //иначе выводится сообщение о невозможности затопления им каких-либо квартир
                if (currApart.Floor != 1)
                    headMsg = Resource.CulpritHeadMessage;
                else
                {
                    TempData["msg"] = Resource.ForFirstFloorUser;
                    return RedirectToAction("Index", "Home");
                }
            }
            else { //если оповещающий не находится на последнем этаже,
                   //то создается сообщение по-умолчанию для жертвы потопа,
                   //иначе выводится сообщение о невозможности затопления его кем-либо
                if (currApart.Floor != House.FloorsCount)
                    headMsg = Resource.VictimHeadMessage;
                else
                {
                    TempData["msg"] = Resource.ForLastFloorUser;
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(new NotifyViewModel
            {
                Apartments = GetApartmentsForNotification(currApart, problemStatus, Database.Reactions.LastReactionId, isSecond, 
                             Database.Apartments.AllIncluding(a => a.Reactions).ToList()),
                ProblemStatus = problemStatus,
                HeadMessage = headMsg,
                ProblemId = Database.Problems.GetProblemIdByName("Потоп"),
                IsSecondNotify = isSecond
            });
        }

        /// <summary>
        /// Добавляет информацию о событии в БД
        /// и отображает оповестившему схемы этажей с оповещенными квартирами
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult Notify(NotifyViewModel model)
        {
            ClientProfile CurrentUserProfile = Database.ClientProfiles.CurrentClientProfile;
            int lastReactionId = Database.Reactions.LastReactionId;

            //Проверка успешности валидации модели
            if (ModelState.IsValid)
            {
                //Создание заглавного сообщения
                Message msg = new Message
                {
                    Text = model.HeadMessage,
                    Time = DateTime.Now,
                    ClientProfile = CurrentUserProfile,
                    IsHead = true
                };

                //получение статуса оповещаемых квартир
                var statusOfnotifiedApartments = model.ProblemStatus == ProblemStatus.Culprit ?
                                                 ProblemStatus.PotentialVictim : ProblemStatus.PotentialCulprit;

                //если текущее оповещение - второе в рамках данного события
                if (model.IsSecondNotify)
                {
                    //Добавление заглавного сообщения в БД
                    msg.EventId = lastReactionId;
                    Database.Messages.Add(msg);
                    Database.Save();

                    //Обновление реакции текущей квартиры
                    var currReaction = Database.Reactions.All.FirstOrDefault(r => r.EventId == lastReactionId
                                                                     && r.ApartmentNumber == CurrentUserProfile.ApartmentNumber);
                    currReaction.ProblemStatus = model.ProblemStatus;
                    currReaction.Notifier = true;
                    currReaction.Reacted = true;
                    Database.Reactions.Update(currReaction);
                    Database.Save();

                    //Обновление реакций квартир неуспевших ответить
                    List<Reaction> notAnsweredReactions = Database.Reactions.All
                        .Where(r => r.EventId == lastReactionId && !r.Reacted)
                        .ToList();

                    foreach (Reaction r in notAnsweredReactions)
                    {
                        r.ProblemStatus = ProblemStatus.None;
                        r.Reacted = true;
                        Database.Reactions.Update(r);
                    }
                    Database.Save();
                   
                    //Добавление реакций оповещаемых квартир
                    foreach (var a in model.Apartments)
                        Database.Reactions.Add(new Reaction
                        {
                            ApartmentNumber = a,
                            EventId = lastReactionId,
                            ProblemStatus = statusOfnotifiedApartments,
                        });
                    Database.Save();
                }
                else
                {   //Создание реакции текушей квартиры
                    var reactions = new List<Reaction>
                    {
                        new Reaction
                        {
                            ApartmentNumber = CurrentUserProfile.ApartmentNumber,
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
                    Database.Events.Add(_event);
                    Database.Save();
                }
                
                //Оповестить через email
                if(model.SendEmail)
                    notifyProcessor.ProcessNotify(msg, 
                        GetNotifiedApartments().Select(r => r.Apartment).ToList(), 
                        Request.Url.Authority);

                return View("ReactionResult");
            }
            else //если модель не прошла валидацию,
            {   //то в представление повторно отправляется список оповещаемых квартир
                model.Apartments = GetApartmentsForNotification(CurrentUserProfile.Apartment, 
                    model.ProblemStatus, lastReactionId, model.IsSecondNotify, Database.Apartments.All.ToList());
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
        public ViewResult Answer(ProblemStatus notifyStatus = ProblemStatus.None)
        {
            if (notifyStatus == ProblemStatus.PotentialCulprit)
                return View("PotentialCulpritAnswer", new ViewModels.AnswerViewModel
                {
                    VisitorProblemStatus = notifyStatus,
                    IsFromPotentialCulprit = true
                });
            else 
                return View("PotentialVictimAnswer", new ViewModels.AnswerViewModel
                {
                    VisitorProblemStatus = notifyStatus,
                    IsFromPotentialCulprit = false
                });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Answer(ViewModels.AnswerViewModel model)
        {
            ClientProfile CurrentUserProfile = Database.ClientProfiles.CurrentClientProfile;

            //Проверка успешности валидации модели
            if (ModelState.IsValid)
            {
                //Изменение инфы о реакции текущей оповещенной квартиры
                var reaction = Database.Reactions.CurrentReaction;
                reaction.ProblemStatus = model.VisitorProblemStatus;
                reaction.Reacted = true;
                Database.Reactions.Update(reaction);
                Database.Save();

                //Если текущая оповещенная квартира не является виновником проблемы,
                if (model.VisitorProblemStatus != ProblemStatus.Culprit)
                {
                    //Создание и добавление ответного сообщения в БД
                    var msg = new Message
                    {
                        Text = model.CurrAnswer,
                        Time = DateTime.Now,
                        ClientProfile = CurrentUserProfile,
                        IsHead = false,
                        EventId = Database.Reactions.CurrentReaction.EventId
                    };

                    Database.Messages.Add(msg);
                    Database.Save();

                    //если нет неответивших квартир, событие завершается
                    if (GetNotifiedApartments().All(r => r.Reacted))
                        return RedirectToAction("EventCompletion");

                    //иначе генерируется страница с сообщением текущего пользователя
                    //подтверждающая, что его сообщение отправлено
                    TempData["msg"] = Resource.MessageIsSent;

                    return View("ReactionResult");
                }
                else //Если текущая оповещенная квартира является инициатором проблемы,
                {   //то из таблица Reactions удаляются все записи касающиеся неуспевших ответить квартир
                    var reactions = GetNotifiedApartments().Where(r => !r.Reacted);

                    Database.Reactions.DeleteMany(reactions);
                    Database.Save();
                    
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
            var currEvent = Database.Events.GetOne(Database.Reactions.CurrentReaction.EventId);
            currEvent.End = DateTime.Now;
            Database.Save();

            TempData["msg"] = Resource.ProblemIsFixed;

            return RedirectToAction("ProblemHistory", "History");
        }

        #region ChildActions
        //Выводит список оповещенных квартир в частичное представление
        [ChildActionOnly]
        public PartialViewResult FloorSchema()=>  PartialView(GetNotifiedApartments());

        //Выводит главное сообщение в частичное представление
        [ChildActionOnly]
        public PartialViewResult HeadMessage()
        {
            Reaction currentReaction = Database.Reactions.CurrentReaction;
            Message message = Database.Messages.All.FirstOrDefault(m => m.EventId == currentReaction.EventId && m.IsHead);
            return PartialView(message);
        }

        //Выводит ответные сообщения в частичное представление
        [ChildActionOnly]
        public PartialViewResult AnswerMessages()
        {
            Reaction currentReaction = Database.Reactions.CurrentReaction;
            List<Message> messages = Database.Messages.All
                .Where(m => m.EventId == currentReaction.EventId && !m.IsHead).ToList();
            return PartialView(messages);
        }
        #endregion

        #region Helpers
        //Генерирует квартиры для оповещения исходя из статуса оповещающего 
        //и потенциальной уязвимости к потопу из-за своего положения относительно квартиры оповещающего
        public IList<int> GetApartmentsForNotification(Apartment currApart, ProblemStatus problemStatus, 
            int lastReactionId, bool isSecond, List<Apartment> allApartments)
        {
            var apartmentsForNotification = new List<int>();

            if (problemStatus == ProblemStatus.Culprit)
            {
                for (int i = 1; i <= House.AlertRangeVertical; i++)
                {
                    var res = allApartments.Where(a =>
                     a.Number >= (currApart.Number - House.ApartmentsWithinFloor * i - House.AlertRangeHorizontal)
                     && a.Number <= (currApart.Number - House.ApartmentsWithinFloor * i + House.AlertRangeHorizontal)
                     && a.Floor == currApart.Floor - i && (a.Reactions.Count == 0
                                //В сценарии "меня топят/я топлю" при 2-м оповещении в рамках данного события
                                //позволяет избежать получения для оповещения квартир, которые уже ответили
                     || isSecond ? !a.Reactions.Any(r => r.EventId == lastReactionId && r.Reacted)
                                 //Позволяет получить для оповещения квартиры, участвовавшие в прошлых уже завершенных событиях
                                 : !a.Reactions.Any(r => r.EventId == lastReactionId && !r.Reacted)))
                     .OrderBy(a => a.Number).Select(a => a.Number).ToList();

                    apartmentsForNotification.AddRange(res);
                }
            }
            else {
                for (int i = 1; i <= House.AlertRangeVertical; i++)
                {
                    var res = allApartments.Where(a =>
                    a.Number >= (currApart.Number + House.ApartmentsWithinFloor * i - House.AlertRangeHorizontal)
                    && a.Number <= (currApart.Number + House.ApartmentsWithinFloor * i + House.AlertRangeHorizontal)
                    && a.Floor == currApart.Floor + i)
                    .OrderBy(a => a.Number)
                    .Select(a => a.Number).ToList();

                    apartmentsForNotification.AddRange(res);
                }
            }
            return apartmentsForNotification;
        }

        //Получает из БД оповещенные квартиры
        private IEnumerable<Reaction> GetNotifiedApartments()
        {
            int currentEventId = Database.Reactions.CurrentReaction.EventId;
            return Database.Reactions.AllIncluding(r => r.Apartment).Where(r => r.EventId == currentEventId && !r.Notifier)
                              .OrderBy(r => r.ApartmentNumber).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            Database.Dispose();
            base.Dispose(disposing);
        }
        #endregion
    }
}
