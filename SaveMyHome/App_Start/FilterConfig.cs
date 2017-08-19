using SaveMyHome.DAL;
using SaveMyHome.Filters;
using SaveMyHome.Infrastructure.Repository.Abstract;
using SaveMyHome.Infrastructure.Repository.Concret;
using System.Web;
using System.Web.Mvc;

namespace SaveMyHome
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogAttribute(new UnitOfWork()));
        }
    }
}
