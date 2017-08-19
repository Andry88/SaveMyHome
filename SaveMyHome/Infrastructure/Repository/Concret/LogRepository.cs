using SaveMyHome.Infrastructure.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SaveMyHome.Areas.Admin.Models;
using SaveMyHome.DAL;
using System.Data.Entity;

namespace SaveMyHome.Infrastructure.Repository.Concret
{
    public class LogRepository : ILogRepository
    {
        ApplicationDbContext db;
        public LogRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Visitor> All => db.Visitors;

        public void Add(Visitor entity)
        {
            db.Entry(entity).State = EntityState.Added;
        }
    }
}