using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CSVOrder.DAL.Abstract;
using CSVOrder.Models.Service;

namespace CSVOrder.Controllers
{
    public class ModuleOrderController : Controller
    {
        //
        // GET: /ModuleOrder/
        private string _returnUrl = string.Empty;
        private ListPostsViewModel _model;
        private readonly IServiseRepository _storage;

        public ModuleOrderController(IServiseRepository storage)
        {
            _storage = storage;
        }

        public ActionResult Index()
        {
            _model = new ListPostsViewModel(_storage);
            return View(_model);
        }

        public ActionResult OrderSelect(int indx, string time, string returnUrl)
        {
            _returnUrl = returnUrl;
            if (string.IsNullOrEmpty(time))
                return JavaScript("window.location = '" + Url.Action("OrderView", "ModuleOrder", new {indx = indx}) + "'");
            return JavaScript("window.location = '" + Url.Action("OrderCreate", "ModuleOrder", 
                new { indx = indx, time = time}) + "'"); ;
        }

        public ViewResult OrderView(int indx=-1)
        {
            if (indx < 0) return View("Index", new ListPostsViewModel(_storage));
            var curentorder = GetOrder(indx); 
            if (curentorder != null)
                ViewBag.Title = string.Format("Заявка №{0}:", curentorder.OrderNumer);
            return View(curentorder);
        }

        public ViewResult OrderCreate(int indx, string time)
        {
            var dt = DateTime.Now;
            DateTime.TryParse(time, out dt);
            var order = new CarOrderPostModel { DateWork = dt, DtCreate = DateTime.Now };
            ViewBag.Title = "Новая заявка";
            return View("OrderView", order);
        }

        public ActionResult LeftDate(string dt)
        {
            DateTime date;
            if (!DateTime.TryParse(dt, out date)) return View("Index");
            _model = new ListPostsViewModel(_storage, date.AddDays(-1));
            return View("Index", _model);
        }

        public ActionResult RightDate(string dt)
        {
            DateTime date;
            if (!DateTime.TryParse(dt, out date)) return View("Index");
            _model = new ListPostsViewModel(_storage, date.AddDays(1));
            return View("Index", _model);
        }

        private CarOrderPostModel GetOrder(int indx)
        {
            //TODO Get Order from db
            return _storage.GetOrder(indx);
        }

        [HttpPost]
        public ActionResult EditOrder(CarOrderPostModel model)
        {
            if (ModelState.IsValid)
            {
                _storage.SaveOrder(model);
                return View("Index", new ListPostsViewModel(_storage));
            }
            else
                return View("OrderView", model);
        }
    }
}
