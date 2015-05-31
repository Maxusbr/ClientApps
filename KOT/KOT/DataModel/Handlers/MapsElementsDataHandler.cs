using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using KOT.Common.Controls;
using KOT.DataModel.Model;
using KOT.DataModel.Network;
using KOT.DataModel.ViewModel;
using Newtonsoft.Json;

namespace KOT.DataModel.Handlers
{
    public class MapsElementsDataHandler
    {
        private static readonly MapsElementsDataHandler Instance = new MapsElementsDataHandler();
        
        public static async Task GetMapElements(int id = 0)
        {
            await Instance.GetMapElementsAsync(id);
        }

        private async Task GetMapElementsAsync(int id)
        {
            MapSourceHandler.ServicePoints.Clear();

            #region DesignMode

            if (DesignMode.DesignModeEnabled)
            {
                AddElements(new PlacesModel
                {
                    idCategory = 3,
                    Latitude = 557500,
                    Longitude = 376300,
                    Adress = "Adress 1",
                    Name = "Service",
                    MinPrice = 250,
                    Score = 90
                });
                AddElements(new PlacesModel
                {
                    idCategory = 1,
                    Latitude = 557400,
                    Longitude = 376300,
                    Adress = "Adress 2",
                    Name = "Wash",
                    MinPrice = 80,
                    Score = 80
                });
                AddElements(new PlacesModel
                {
                    idCategory = 0,
                    Latitude = 557500,
                    Longitude = 376100,
                    Adress = "Adress 3",
                    Name = "Gas",
                    MinPrice = 50,
                    Score = 20
                });
                AddElements(new PlacesModel
                {
                    idCategory = 2,
                    Latitude = 557600,
                    Longitude = 376100,
                    Adress = "Adress 4",
                    Name = "Shop",
                    MinPrice = 150,
                    Score = 40
                });
            }

            #endregion

            try
            {
                var latitude =(int)Math.Truncate(MapSourceHandler.Kot.Location.Position.Latitude * 10000);
                var longitude = (int)Math.Truncate(MapSourceHandler.Kot.Location.Position.Longitude * 10000);
                var query = string.Format("DC{0};{1};{2}", id, latitude, longitude);
                var res = await TcpConnection.Send(query);
                if (!string.IsNullOrEmpty(res.Msg))
                    Split(res.Fx,res.Msg);
            }
            catch (Exception e)
            {
            }
        }

        private void AddElements(PlacesModel model)
        {
            switch (model.idCategory)
            {
                case 0:
                    AddGasElement(model);
                    break;
                case 1:
                    AddWashElement(model);
                    break;
                case 2:
                    AddShopElement(model);
                    break;
                case 3:
                    AddServiceElement(model);
                    break;
            }
        }

        private void AddGasElement(PlacesModel model)
        {
            var pin = new ServicePointViewModel(model, "GasRadioButtonStyle");
            //pin.Click += pin_Click;
            MapSourceHandler.ServicePoints.Add(pin);
        }

        private void AddWashElement(PlacesModel model)
        {
            var pin = new ServicePointViewModel(model, "WashRadioButtonStyle");
            //pin.Click += pin_Click;
            MapSourceHandler.ServicePoints.Add(pin);
        }

        private void AddShopElement(PlacesModel model)
        {
            var pin = new ServicePointViewModel(model, "ShopRadioButtonStyle");
            //pin.Click += pin_Click;
            MapSourceHandler.ServicePoints.Add(pin);
        }

        private void AddServiceElement(PlacesModel model)
        {
            var pin = new ServicePointViewModel(model);
            //pin.Click += pin_Click;
            MapSourceHandler.ServicePoints.Add(pin);
        }

        internal async static Task<PointDetailsModel> GetDetail(PlacesModel model)
        {
            return await Instance.GetDetailAsync(model);
        }
        private PointDetailsModel _detail;
        private async Task<PointDetailsModel> GetDetailAsync(PlacesModel model)
        {
            #region DesignMode

            if (DesignMode.DesignModeEnabled)
            {
                _detail = new PointDetailsModel();
                _detail.Comments.Add(new PointDetailsModel.CommentModel
                {
                    Date = new DateDataModel(DateTime.Now),
                    Score = 80,
                    Text = "Text1"
                });
                _detail.Comments.Add(new PointDetailsModel.CommentModel
                {
                    Date = new DateDataModel(DateTime.Now.AddDays(-3)),
                    Score = 40,
                    Text = "Text2"
                });
                _detail.Price.Add(new PointDetailsModel.PriceModel { Name = "Service 1", Cost = 250 });
                _detail.Price.Add(new PointDetailsModel.PriceModel { Name = "Service 2", Cost = 350 });
                _detail.Price.Add(new PointDetailsModel.PriceModel { Name = "Service 3", Cost = 550 });
                _detail.Price.Add(new PointDetailsModel.PriceModel { Name = "Service 4", Cost = 150 });
                return _detail;
            }

            #endregion

            var res = await TcpConnection.Send("DD" + model.ID);
            if (!string.IsNullOrEmpty(res.Msg))
                Split(res.Fx, res.Msg);
            
            return _detail ?? new PointDetailsModel();
        }

        private void Split(char fx, string msg)
        {
            try
            {
                if (fx == 'C' || fx == 'c')
                    foreach (var el in JsonConvert.DeserializeObject<PlacesModel[]>(msg))
                    {
                        AddElements(el);
                    }
                if (fx == 'D' || fx == 'd')
                    _detail = JsonConvert.DeserializeObject<PointDetailsModel>(msg);
            }
            catch (Exception)
            {

            }
        }
    }
}
