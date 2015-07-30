using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSVOrder.DAL.Abstract;
using CSVOrder.Models.Service;

namespace CSVOrder.Controllers
{
    public class ModuleStoreController : Controller
    {
        
        //
        // GET: /ModuleStore/
        private IServiseRepository _storage;

        public ModuleStoreController(IServiseRepository storage)
        {
            _storage = storage;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ViewResult GetWorksView()
        {
            return View("WorksView", _storage.Works);
        }

        public double GetDetailWork(int id, string nh)
        {
            var sum = 0.0;
            double.TryParse(nh.Replace(".", ","), out sum);
            //TODO Get part works
            var work = _storage.Works.FirstOrDefault(o => o.Id == id);
            if (work != null) sum += work.Nh / 10.0;
            return sum;
        }
    }
}
