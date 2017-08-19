using SaveMyHome.DAL;
using SaveMyHome.Infrastructure.Repository.Abstract;
using System;

namespace SaveMyHome.Infrastructure.Repository.Concret
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IUserRepository userRepository;
        private IClientProfileRepository clientProfileRepository;
        private IApartmentRepository apartmentRepository;
        private IReactionRepository reactionRepository;
        private IProblemRepository problemRepository;
        private IMessageRepository messageRepository;
        private IEventRepository eventRepository;
        private ILogRepository logRepository;

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public IClientProfileRepository ClientProfiles
        {
            get
            {
                if (clientProfileRepository == null)
                    clientProfileRepository = new ClientProfileRepository(db);
                return clientProfileRepository;
            }
        }

        public IApartmentRepository Apartments
        {
            get
            {
                if (apartmentRepository == null)
                    apartmentRepository = new ApartmentRepository(db);
                return apartmentRepository;
            }
        }

        public IReactionRepository Reactions
        {
            get
            {
                if (reactionRepository == null)
                    reactionRepository = new ReactionRepository(db);
                return reactionRepository;
            }
        }

        public IProblemRepository Problems
        {
            get
            {
                if (problemRepository == null)
                    problemRepository = new ProblemRepository(db);
                return problemRepository;
            }
        }

        public IMessageRepository Messages
        {
            get
            {
                if (messageRepository == null)
                    messageRepository = new MessageRepository(db);
                return messageRepository;
            }
        }

        public IEventRepository Events
        {
            get
            {
                if (eventRepository == null)
                    eventRepository = new EventRepository(db);
                return eventRepository;
            }
        }

        public ILogRepository Visitors
        {
            get
            {
                if (logRepository == null)
                    logRepository = new LogRepository(db);
                return logRepository;
            }
        }
        public void Save()=> db.SaveChanges();

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    db.Dispose();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}