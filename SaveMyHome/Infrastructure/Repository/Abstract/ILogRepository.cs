using SaveMyHome.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaveMyHome.Infrastructure.Repository.Abstract
{
    public interface ILogRepository
    {
        IEnumerable<Visitor> All { get; }
        void Add(Visitor entity);
    }
}