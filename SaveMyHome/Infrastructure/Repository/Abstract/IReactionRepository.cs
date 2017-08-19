using SaveMyHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveMyHome.Infrastructure.Repository.Abstract
{
    public interface IReactionRepository : IRepository<Reaction>
    {
        int LastReactionId { get; }
        Reaction CurrentReaction { get; }
        void DeleteMany(IEnumerable<Reaction> entities);
    }
}
