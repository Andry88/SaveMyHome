using SaveMyHome.Areas.Admin.Models;
using System.Collections.Generic;

namespace SaveMyHome.Infrastructure.Repository.Abstract
{
    public interface ILogRepository
    {
        IEnumerable<Visitor> All { get; }
        void Add(Visitor entity);
    }
}