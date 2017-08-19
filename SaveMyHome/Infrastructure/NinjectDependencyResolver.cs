using Ninject;
using SaveMyHome.Abstract;
using SaveMyHome.Helpers;
using SaveMyHome.Infrastructure.Repository.Abstract;
using SaveMyHome.Infrastructure.Repository.Concret;
using SaveMyHome.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;

namespace SaveMyHome.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<INotifyProcessor>().To<EmailNotifyProcessor>()
                .WithConstructorArgument("settings", new EmailSettings());
        } 
    }
}