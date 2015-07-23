using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CSVOrder.Models.Service;

namespace CSVOrder.Controllers
{
    public class ModuleOrderController : Controller
    {
        //
        // GET: /ModuleOrder/
        private string _returnUrl = string.Empty;
        private readonly ListPostsViewModel _model = new ListPostsViewModel();
        public ActionResult Index()
        {
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

        public ViewResult OrderView(int indx)
        {
            var curentorder = _model.ListOrders.FirstOrDefault(o => o.OrderNumer == indx);
            if (curentorder != null)
                ViewBag.Title = string.Format("Заявка: {0}", curentorder.OrderNumer);
            return View(curentorder);
        }

        public ViewResult OrderCreate(int indx, string time)
        {
            var ts = new TimeSpan();
            TimeSpan.TryParse(time, out ts);
            var order = new CarOrderPostModel { DateWork = _model.Date + ts, DtCreate = DateTime.Now };
            ViewBag.Title = "Новая заявка";
            return View("OrderView", order);
        }

    }
}
