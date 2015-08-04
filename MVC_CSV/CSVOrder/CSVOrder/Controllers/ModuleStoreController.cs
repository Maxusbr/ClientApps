using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSVOrder.DAL.Abstract;
using CSVOrder.Models.Abstract;
using CSVOrder.Models.Service;

namespace CSVOrder.Controllers
{
    public class ModuleStoreController : Controller
    {

        //
        // GET: /ModuleStore/
        private readonly IServiseRepository _storage;

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
            if (work != null)
            {
                sum += work.Nh / 10.0;
            }
            return sum;
        }

        public static ReturnSelectList GetList(string name, string display, CarViewModel model)
        {
            var result = new ReturnSelectList();
            switch (name)
            {
                case "Marks":
                    result.List.Add(new KVPBase { Name = "Выберите \"Марку\"", id = 0 });
                    result.List.AddRange(model.Marks);
                    result.Name = model.Mark;
                    break;
                case "Models":
                    if (string.IsNullOrEmpty(model.Model)) break;
                    result.List.Add(new KVPBase { Name = "Выберите \"Модель\"", id = 0 });
                    result.List.AddRange(model.Models);
                    result.Name = model.Model;
                    break;
                case "BodyTypes":
                    if (string.IsNullOrEmpty(model.BodyType)) break;
                    result.List.Add(new KVPBase { Name = "Выберите \"Тип кузова\"", id = 0 });
                    result.List.AddRange(model.BodyTypes);
                    result.Name = model.BodyType;
                    break;
                case "EngineTypes":
                    if (string.IsNullOrEmpty(model.EngineType)) break;
                    result.List.Add(new KVPBase { Name = "Выберите \"Тип двигателя\"", id = 0 });
                    result.List.AddRange(model.EngineTypes);
                    result.Name = model.EngineType;
                    break;
                case "EngineVolumes":
                    if (string.IsNullOrEmpty(model.EngineVolume)) break;
                    result.List.Add(new KVPBase { Name = "Выберите \"Объем двигателя\"", id = 0 });
                    result.List.AddRange(model.EngineVolumes);
                    result.Name = model.EngineVolume;
                    break;
                case "TransTypes":
                    if (string.IsNullOrEmpty(model.TransType)) break;
                    result.List.Add(new KVPBase { Name = "Выберите \"Тип КПП\"", id = 0 });
                    result.List.AddRange(model.TransTypes);
                    result.Name = model.TransType;
                    break;
            }
            return result;
        }

        public static IList<SelectListItem> GetList(string name, string display, string data)
        {
            return new List<SelectListItem>();
        }


        public JsonResult ChangeSelectList(string name, string data)
        {
            var model = new ReturnSelectList();
            var indx = 0;
            if (!int.TryParse(data, out indx) || indx == 0) return Json(model);
            switch (name)
            {
                case "GetMark":
                    model.Name = "Car_Mark";
                    model.List.Add(new KVPBase { Name = "Выберите \"Марку\"", id = 0 });
                    //TODO Get Models
                    model.List.AddRange(_storage.GetMarks());
                    break;
                case "Car.Mark":
                    model.Name = "Car_Model";
                    model.List.Add(new KVPBase {Name = "Выберите \"Модель\"", id = 0});
                    //TODO Get Models
                    model.List.AddRange(_storage.GetModels(indx));
                    break;
                case "Car.Model":
                    model.Name = "Car_BodyType";
                    model.List.Add(new KVPBase { Name = "Выберите \"Тип кузова\"", id = 0 });
                    //TODO Get Bodys
                    model.List.AddRange(_storage.GetBodyTypes(indx));
                    break;
                case "Car.BodyType":
                    model.Name = "Car_EngineType";
                    model.List.Add(new KVPBase { Name = "Выберите \"Тип двигателя\"", id = 0 });
                    //TODO Get EngineTypes
                    model.List.AddRange(_storage.GetEngineTypes(indx));
                    break;
                case "Car.EngineType":
                    model.Name = "Car_EngineVolume";
                    model.List.Add(new KVPBase { Name = "Выберите \"Объем двигателя\"", id = 0 });
                    //TODO Get EngineVolumes
                    model.List.AddRange(_storage.GetEngineVolumes(indx));
                    break;
                case "Car.EngineVolume":
                    model.Name = "Car_TransType";
                    model.List.Add(new KVPBase { Name = "Выберите \"Тип КПП\"", id = 0 });
                    //TODO Get TransTypes
                    model.List.AddRange(_storage.GetTransTypes(indx));
                    break;
            }

            return Json(model);
        }

        public JsonResult GetMarks(string indx)
        {
            var list = new List<KVPBase> { new KVPBase { id = 0, Name = "Выберите \"Марку\"" } };
            list.AddRange(_storage.GetMarks());
            return Json(list);
        }
    }

    public class ReturnSelectList
    {
        public ReturnSelectList() { List = new List<KVPBase>();}
        public string Name { get; set; }
        public List<KVPBase> List { get; set; }
    }
}
