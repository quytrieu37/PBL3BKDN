using Ninject;
using Ninject.Web.Common.WebHost;
using PBL3Store.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PBL3Store.UI
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IMainRepository>().To<MainRepository>();
            return kernel;
        }
    }
}
