using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaveMyHome.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SaveMyHome.Controllers;
using Moq;
using SaveMyHome.Abstract;
using System.Web.Mvc;
using SaveMyHome.Infrastructure.Repository.Abstract;
using SaveMyHome.ViewModels;
using SaveMyHome.Infrastructure.Repository.Concret;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SaveMyHome.Tests
{
    [TestClass]
    public class FloodControllerTests
    {
        #region Arrange

        private Apartment CurrentApartment(int number)
            => new Apartment { Number = number, Reactions = new List<Reaction>() };

        private List<Apartment> AllApartments()
        {
            List<Apartment> apartments = new List<Apartment>();

            for (int i = 1; i <= House.ApartmentsAmount; i++)
                apartments.Add(new Apartment { Number = i, Reactions = new List<Reaction>() });

            return apartments;
        }

        private List<Apartment> AllApartments(IEnumerable<Reaction> reactions)
        {
            List<Apartment> apartments = new List<Apartment>();

            for (int i = 1; i <= House.ApartmentsAmount; i++)
            {
                Apartment apart = new Apartment { Number = i, Reactions = new List<Reaction>() };
                foreach (var reaction in reactions)
                {
                    if (apart.Number == reaction.ApartmentNumber)
                        apart.Reactions.Add(reaction);
                }

                apartments.Add(apart);
            }

            return apartments;
        }
        #endregion

        [TestMethod]
        public void Get_Floor_by_Apartment()
        {
            var apartments = new List<Apartment>
            {
                new Apartment{ Number = 5},
                new Apartment{ Number = 35},
                new Apartment{ Number = 71}
            };

            int floorFor5 = apartments[0].Floor;
            int floorFor35 = apartments[1].Floor;
            int floorFor71 = apartments[2].Floor;

            string message = "Данная квартира находится на другом этаже";
            Assert.AreEqual(1, floorFor5, message);
            Assert.AreEqual(6, floorFor35, message);
            Assert.AreEqual(12, floorFor71, message);
        }

        #region GetPotentialVictims

        [TestMethod]
        public void Get_Potential_Victims_For_Notify()
        {
            FloodController controller = new FloodController(null, null);

            IList<int> result = controller.GetApartmentsForNotify(CurrentApartment(50), ProblemStatus.Culprit, 1, false, AllApartments());

            Assert.AreEqual(result.Count(), 6, "Сгенерированное количество квартир не соответствует ожидаемому");
            Assert.AreEqual(result[0], 43);
            Assert.AreEqual(result[1], 44);
            Assert.AreEqual(result[2], 45);
            Assert.AreEqual(result[3], 37);
            Assert.AreEqual(result[4], 38);
            Assert.AreEqual(result[5], 39);
        }

        [TestMethod]
        public void Get_Potential_Victims_For_Notify_From_First_Floor()
        {
            FloodController controller = new FloodController(null, null);

            IList<int> result = controller.GetApartmentsForNotify(CurrentApartment(9), ProblemStatus.Culprit, 1, false, AllApartments());

            Assert.AreEqual(result.Count(), 3, "Сгенерированное количество квартир не соответствует ожидаемому");
            Assert.AreEqual(result[0], 2);
            Assert.AreEqual(result[1], 3);
            Assert.AreEqual(result[2], 4);
        }

        [TestMethod]
        public void Get_Potential_Victims_For_Notify_Where_Culprit_Apartment_is_The_Last_or_The_First_On_The_Floor()
        {
            FloodController controller = new FloodController(null, null);

            IList<int> result = controller.GetApartmentsForNotify(CurrentApartment(35), ProblemStatus.Culprit, 1, false, AllApartments());

            Assert.AreEqual(result.Count(), 4, "Сгенерированное количество квартир не соответствует ожидаемому");
            Assert.AreEqual(result[0], 28);
            Assert.AreEqual(result[1], 29);
            Assert.AreEqual(result[2], 22);
            Assert.AreEqual(result[3], 23);
        }
        [TestMethod]
        public void Get_Potential_Victims_For_Notify_Which_Participated_In_Other_Completed_Events()
        {
            List<Reaction> pastReactions = new List<Reaction>
            {
                new Reaction { ApartmentNumber = 60 , EventId = 1, Notifier = true, ProblemStatus = ProblemStatus.Culprit, Reacted = true},
                new Reaction { ApartmentNumber = 54, EventId = 1, Notifier = false, ProblemStatus = ProblemStatus.None, Reacted = true},
                new Reaction { ApartmentNumber = 55, EventId = 1, Notifier = false, ProblemStatus = ProblemStatus.Victim, Reacted = true}
            };

            FloodController controller = new FloodController(null, null);

            IList<int> result = controller.GetApartmentsForNotify(CurrentApartment(67), ProblemStatus.Culprit, 2, false, AllApartments(pastReactions));

            Assert.AreEqual(result.Count(), 6, "Сгенерированное количество квартир не соответствует ожидаемому");
            Assert.AreEqual(result[0], 60);
            Assert.AreEqual(result[1], 61);
            Assert.AreEqual(result[2], 62);
            Assert.AreEqual(result[3], 54);
            Assert.AreEqual(result[4], 55);
            Assert.AreEqual(result[5], 56);
        }
        [TestMethod]
        public void Get_Potential_Victims_For_Notify_The_Second_Time_In_This_Event_Without_An_Answered_Apartments()
        {
            List<Reaction> currentReactions = new List<Reaction>
            {
                new Reaction { ApartmentNumber = 61, EventId = 1, Notifier = false, ProblemStatus = ProblemStatus.PotentialCulprit, Reacted = false},
                new Reaction { ApartmentNumber = 55 , EventId = 1, Notifier = false, ProblemStatus = ProblemStatus.None, Reacted = true},
                new Reaction { ApartmentNumber = 56, EventId = 1, Notifier = false, ProblemStatus = ProblemStatus.PotentialCulprit, Reacted = false},
                new Reaction { ApartmentNumber = 50, EventId = 1, Notifier = true, ProblemStatus = ProblemStatus.Victim, Reacted = true}
            };

            FloodController controller = new FloodController(null, null);

            IList<int> result = controller.GetApartmentsForNotify(CurrentApartment(62), ProblemStatus.Culprit, 1, true, AllApartments(currentReactions));

            Assert.AreEqual(result.Count(), 4, "Сгенерированное количество квартир не соответствует ожидаемому");
            Assert.AreEqual(result[0], 56);
            Assert.AreEqual(result[1], 57);
            Assert.AreEqual(result[2], 49);
            Assert.AreEqual(result[3], 51);
        }
        #endregion
        #region GetPotentialCulprits
        [TestMethod]
        public void Get_Potential_Culprits_For_Notify()
        {
            FloodController controller = new FloodController(null, null);

            IList<int> result = controller.GetApartmentsForNotify(CurrentApartment(50), ProblemStatus.Victim, 1, false, AllApartments());

            Assert.AreEqual(result.Count(), 6, "Сгенерированное количество квартир не соответствует ожидаемому");
            Assert.AreEqual(result[0], 55);
            Assert.AreEqual(result[1], 56);
            Assert.AreEqual(result[2], 57);
            Assert.AreEqual(result[3], 61);
            Assert.AreEqual(result[4], 62);
            Assert.AreEqual(result[5], 63);
        }

        [TestMethod]
        public void Get_Potential_Culprits_For_Notify_From_Last_Floor()
        {
            FloodController controller = new FloodController(null, null);

            IList<int> result = controller.GetApartmentsForNotify(CurrentApartment(62), ProblemStatus.Victim, 1, false, AllApartments());

            Assert.AreEqual(result.Count(), 3, "Сгенерированное количество квартир не соответствует ожидаемому");
            Assert.AreEqual(result[0], 67);
            Assert.AreEqual(result[1], 68);
            Assert.AreEqual(result[2], 69);
        }

        [TestMethod]
        public void Get_Potential_Culprits_For_Notify_Where_Victim_Apartment_is_The_Last_or_The_First_On_The_Floor()
        {
            FloodController controller = new FloodController(null, null);

            IList<int> result = controller.GetApartmentsForNotify(CurrentApartment(48), ProblemStatus.Victim, 1, false, AllApartments());

            Assert.AreEqual(result.Count(), 4, "Сгенерированное количество квартир не соответствует ожидаемому");
            Assert.AreEqual(result[0], 54);
            Assert.AreEqual(result[1], 55);
            Assert.AreEqual(result[2], 60);
            Assert.AreEqual(result[3], 61);

        }

        //Пока просто скопировал
        [TestMethod]
        public void Get_Potential_Culprits_For_Notify_Which_Participated_In_Other_Completed_Events()
        {
            List<Reaction> pastReactions = new List<Reaction>
            {
                new Reaction { ApartmentNumber = 60 , EventId = 1, Notifier = true, ProblemStatus = ProblemStatus.Culprit, Reacted = true},
                new Reaction { ApartmentNumber = 54, EventId = 1, Notifier = false, ProblemStatus = ProblemStatus.None, Reacted = true},
                new Reaction { ApartmentNumber = 55, EventId = 1, Notifier = false, ProblemStatus = ProblemStatus.Victim, Reacted = true}
            };

            FloodController controller = new FloodController(null, null);

            IList<int> result = controller.GetApartmentsForNotify(CurrentApartment(49), ProblemStatus.Victim, 2, false, AllApartments(pastReactions));

            Assert.AreEqual(result.Count(), 6, "Сгенерированное количество квартир не соответствует ожидаемому");
            Assert.AreEqual(result[0], 54);
            Assert.AreEqual(result[1], 55);
            Assert.AreEqual(result[2], 56);
            Assert.AreEqual(result[3], 60);
            Assert.AreEqual(result[4], 61);
            Assert.AreEqual(result[5], 62);
        }

        [TestMethod]
        public void Can_Send_Email()
        {
            Mock<IUnitOfWork> mock1 = new Mock<IUnitOfWork>();
            mock1.Setup(p => p.Reactions.LastReactionId).Returns<int>(id => new int());
            Mock<INotifyProcessor> mock2 = new Mock<INotifyProcessor>();
            FloodController controller = new FloodController(mock1.Object, mock2.Object);
            NotifyVM viewModel = new NotifyVM
            {
                SendEmail = true,
                Apartments = new List<int>(),
                ProblemStatus = ProblemStatus.Victim,
                ProblemId = 1,
                HeadMessage = "",
                IsSecondNotify = false
            };

            ApplicationUser currentUser = new ApplicationUser { Apartment = new Apartment { Number = 15 } };

            controller.Notify(viewModel);
            mock2.Verify(m => m.ProcessNotify(It.IsAny<Message>(), It.IsAny<IEnumerable<Apartment>>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void Can_Send_Head_Message_To_View()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Messages.All).Returns(new List<Message>
            {
                new Message
                {
                    Text = "Text", Time = DateTime.Now,
                    User = new ApplicationUser
                    {
                         ApartmentNumber = 60,
                         FirstName = "First",
                         LastName = "Last"
                    },
                    IsHead = true,
                    EventId = 1
                },
                new Message
                {
                    Text = "Text", Time = DateTime.Now,
                    User = new ApplicationUser
                    {
                         ApartmentNumber = 60,
                         FirstName = "First",
                         LastName = "Last"
                    },
                    IsHead = false,
                    EventId = 2
                },
                new Message
                {
                    Text = "Text", Time = DateTime.Now,
                    User = new ApplicationUser
                    {
                         ApartmentNumber = 60,
                         FirstName = "First",
                         LastName = "Last"
                    },
                    IsHead = true,
                    EventId = 3
                }
            }.AsQueryable());
            mock.Setup(m => m.Reactions.CurrentReaction).Returns(new Reaction { EventId = 1 });

            FloodController controller = new FloodController(mock.Object, null);

            // Act
            PartialViewResult result = controller.HeadMessage() as PartialViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void Get_Head_Message_According_Concrete_Event()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Messages.All).Returns(new List<Message>
            {
                new Message
                {
                    Id = 1,
                    Text = "Text", Time = DateTime.Now,
                    User = new ApplicationUser
                    {
                         ApartmentNumber = 60,
                         FirstName = "First",
                         LastName = "Last"
                    },
                    IsHead = true,
                    EventId = 1
                },
                new Message
                {
                    Id = 2,
                    Text = "Text", Time = DateTime.Now,
                    User = new ApplicationUser
                    {
                         ApartmentNumber = 60,
                         FirstName = "First",
                         LastName = "Last"
                    },
                    IsHead = false,
                    EventId = 2
                },
                new Message
                {
                    Id = 3,
                    Text = "Text", Time = DateTime.Now,
                    User = new ApplicationUser
                    {
                         ApartmentNumber = 60,
                         FirstName = "First",
                         LastName = "Last"
                    },
                    IsHead = true,
                    EventId = 3
                }
            }.AsQueryable());
            mock.Setup(m => m.Reactions.CurrentReaction).Returns(new Reaction { EventId = 1 });

            FloodController controller = new FloodController(mock.Object, null);

            // Act
            PartialViewResult result = controller.HeadMessage() as PartialViewResult;
            Message model = (Message)result.Model;
            // Assert
            Assert.AreEqual(model.Id, 1);
        }

        [TestMethod]
        public void Get_Answered_Messages_According_Concrete_Event()
        {

        }

        [TestClass]
        public class NotifyGetTest
        {
            private Apartment CurrentApartment(int number)
                => new Apartment { Number = number, Reactions = new List<Reaction>() };

            [TestMethod]
            public void If_The_Culprit_From_The_First_Floor_To_Notify_Goes_To_Main_Page()
            {
                var mock = new Mock<IUnitOfWork>();
                mock.Setup(a => a.Users.CurrentUser).Returns(new ApplicationUser { Apartment = new Apartment { Number = 1 } });
                FloodController controller = new FloodController(mock.Object, new Mock<INotifyProcessor>().Object);

                RedirectToRouteResult result = (RedirectToRouteResult)controller.Notify(ProblemStatus.Culprit, false);

                Assert.AreEqual(result.RouteValues["action"], "Index");
                Assert.AreEqual(result.RouteValues["controller"], "Home");
            }

            [TestMethod]
            public void If_The_Victim_From_The_Last_Floor_To_Notify_Goes_To_Main_Page()
            {
                var mock = new Mock<IUnitOfWork>();
                mock.Setup(a => a.Users.CurrentUser).Returns(new ApplicationUser
                {
                    Apartment = new Apartment { Number = House.ApartmentsAmount }
                });
                FloodController controller = new FloodController(mock.Object, new Mock<INotifyProcessor>().Object);

                RedirectToRouteResult result = (RedirectToRouteResult)controller.Notify(ProblemStatus.Victim);

                Assert.AreEqual(result.RouteValues["action"], "Index");
                Assert.AreEqual(result.RouteValues["controller"], "Home");
            }

            [TestMethod]
            public void Notifier_Can_Get_Default_Info_For_Notify()
            {
                //Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
                //mock.Setup(p => p.Apartments.AllIncluding(a => a.Reactions)).Returns(new List<Apartment> {
                //    new Apartment{ Number = 71, Reactions = new List<Reaction>()}
                //}.AsQueryable());
                //mock.Setup(p => p.Reactions.LastReactionId).Returns<int>(id => id);
                //mock.Setup(p => p.Problems.GetProblemIdByName(It.Is<string>(n => n == "Потоп")))
                //    .Returns<int>(p => p);

                Mock<IProblemRepository> mockProblem = new Mock<IProblemRepository>();
                mockProblem.Setup(p => p.All).Returns<List<Problem>>(p => new List<Problem>
            {
                new Problem { Id = 1, Name = "Потоп"},
                new Problem { Id = 2, Name = "Пожар"}
            }.AsQueryable());

                Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
                mock.Setup(m => m.Apartments.AllIncluding(a => a.Reactions));

                FloodController controller = new FloodController(new Mock<IUnitOfWork>().Object, new Mock<INotifyProcessor>().Object);

                ApplicationUser currentUser = new ApplicationUser { Apartment = new Apartment { Number = 61 } };

                NotifyVM result = (NotifyVM)(controller.Notify(ProblemStatus.Victim) as ViewResult).ViewData.Model;

                Assert.AreEqual(result.Apartments.Count, 6);
                Assert.IsNotNull(result.HeadMessage);
                Assert.AreEqual(result.ProblemStatus, ProblemStatus.Victim);
                Assert.AreEqual(result.ProblemId, 1);
            }
            #endregion
        }
    }
}
