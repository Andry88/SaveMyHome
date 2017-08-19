using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveMyHome.Infrastructure.Repository.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IClientProfileRepository ClientProfiles { get; }
        IApartmentRepository Apartments { get; }
        IReactionRepository Reactions{ get; }
        IProblemRepository Problems { get; }
        IMessageRepository Messages { get; }
        IEventRepository Events { get; }
        ILogRepository Visitors { get; }
        void Save();
    }
}
