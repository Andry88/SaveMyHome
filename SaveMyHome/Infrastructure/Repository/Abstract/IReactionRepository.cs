using SaveMyHome.Models;
using System.Collections.Generic;

namespace SaveMyHome.Infrastructure.Repository.Abstract
{
    public interface IReactionRepository : IRepository<Reaction>
    {
        int LastReactionId { get; }
        Reaction CurrentReaction { get; }
        void DeleteMany(IEnumerable<Reaction> entities);
    }
}
