using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using KOT.Common.Controls;
using KOT.DataModel.Model;
using KOT.DataModel.ViewModel;

namespace KOT.DataModel.Handlers
{
    public class MapsElementsDataHandler
    {
        private static readonly MapsElementsDataHandler Instance = new MapsElementsDataHandler();
        
        public static async Task GetMapElements()
        {
            await Instance.GetMapElementsAsync();
        }

        private async Task GetMapElementsAsync()
        {
            var dataUri = new Uri(DataSource.Url + "GetTemplates");
            //try
            //{
            //    var request = (HttpWebRequest)WebRequest.Create(dataUri);
            //    request.Method = "GET";
            //    //request.ContentType = "text/plain";
            //    using (var response = await request.GetResponseAsync())
            //    {
            //        using (var stream = response.GetResponseStream())
            //        {

            //        }
            //    }

            //}
            //catch
            //{

            //}
            AddElements(new PlacesModel
            {
                idCategory = 3, Latitude = 557500, Longitude = 376300, Adress = "Adress 1", Name = "Service", MinPrice = 250, Score = 90
            });
            AddElements(new PlacesModel
            {
                idCategory = 1, Latitude = 557400, Longitude = 376300, Adress = "Adress 2", Name = "Wash", MinPrice = 80, Score = 80
            });
            AddElements(new PlacesModel
            {
                idCategory = 0, Latitude = 557500, Longitude = 376100, Adress = "Adress 3", Name = "Gas", MinPrice = 50, Score = 20
            });
            AddElements(new PlacesModel
            {
                idCategory = 2, Latitude = 557600, Longitude = 376100, Adress = "Adress 4", Name = "Shop", MinPrice = 150, Score = 40
            });       
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
            DataSource.ServicePoints.Add(pin);
        }

        private void AddWashElement(PlacesModel model)
        {
            var pin = new ServicePointViewModel(model, "WashRadioButtonStyle");
            //pin.Click += pin_Click;
            DataSource.ServicePoints.Add(pin);
        }

        private void AddShopElement(PlacesModel model)
        {
            var pin = new ServicePointViewModel(model, "ShopRadioButtonStyle");
            //pin.Click += pin_Click;
            DataSource.ServicePoints.Add(pin);
        }

        private void AddServiceElement(PlacesModel model)
        {
            var pin = new ServicePointViewModel(model);
            //pin.Click += pin_Click;
            DataSource.ServicePoints.Add(pin);
        }

        internal async static Task<PointDetailsModel> GetDetail(PlacesModel model)
        {
            return await Instance.GetDetailAsync(model);
        }

        private async Task<PointDetailsModel> GetDetailAsync(PlacesModel model)
        {
            var dataUri = new Uri(DataSource.Url + "GetTemplates");
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(dataUri);
                request.Method = "GET";
                //request.ContentType = "text/plain";
                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {

                    }
                }

            }
            catch
            {

            }
            var res = new PointDetailsModel();
            res.Comments.Add(new PointDetailsModel.CommentModel{Date = new DateDataModel(DateTime.Now), Score = 80, Text = "Text1"});
            res.Comments.Add(new PointDetailsModel.CommentModel{Date = new DateDataModel(DateTime.Now.AddDays(-3)), Score = 40, Text = "Text2"});
            res.Price.Add(new PointDetailsModel.PriceModel{Name = "Service 1", Cost = 250});
            res.Price.Add(new PointDetailsModel.PriceModel { Name = "Service 2", Cost = 350 });
            res.Price.Add(new PointDetailsModel.PriceModel{Name = "Service 3", Cost = 550});
            res.Price.Add(new PointDetailsModel.PriceModel { Name = "Service 4", Cost = 150 });
            return res;
        }
    }
}
