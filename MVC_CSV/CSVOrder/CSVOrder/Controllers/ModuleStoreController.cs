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


        public ViewResult GetCarModel(string carNumber)
        {
            var car = _storage.GetCar(carNumber)?? new CarViewModel(carNumber);
            return View("AddCarView", car);
        }
    }
}
