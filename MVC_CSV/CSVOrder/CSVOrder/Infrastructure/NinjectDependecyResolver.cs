using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSVOrder.DAL.Abstract;
using CSVOrder.DAL.Concrete;
using Ninject;

namespace CSVOrder.Infrastructure
{
    public class NinjectDependecyResolver: IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependecyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IServiseRepository>().To<ServiseRepository>().InSingletonScope();

        }
    }
}