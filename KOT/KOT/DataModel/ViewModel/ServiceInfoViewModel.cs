using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using KOT.Annotations;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class ServiceInfoViewModel : INotifyPropertyChanged
    {
        private double _userRate = -1;
        private PointDetailsModel _details = new PointDetailsModel();

        public ServiceInfoViewModel()
        {
            //if (DesignMode.DesignModeEnabled)
            {
                Model = new PlacesModel
                {
                    idCategory = 2,
                    Latitude = 557600,
                    Longitude = 376100,
                    Adress = "Adress 4",
                    Name = "Shop",
                    MinPrice = 150,
                    Score = 40
                };
            }
            StylePoint = Application.Current.Resources["ServiceContentControlStyle"] as Style;
        }

        public ServiceInfoViewModel(PlacesModel model)
        {
            Model = model;
            StylePoint = GetStyle(model.idCategory);
        }

        private Style GetStyle(int idCategory)
        {
            switch (idCategory)
            {
                case 0:
                    return Application.Current.Resources["GasContentControlStyle"] as Style;
                case 1:
                    return Application.Current.Resources["WashContentControlStyle"] as Style;
                case 2:
                    return Application.Current.Resources["ShopContentControlStyle"] as Style;
                case 3:
                    return Application.Current.Resources["ServiceContentControlStyle"] as Style;
                default:
                    return Application.Current.Resources["ServiceContentControlStyle"] as Style;
            }
        }
        public string ServicePrice { get { return string.Format("Услуга: {0} руб.", Model.MinPrice); } }

        public double Rate
        {
            get { return Model.Score / 100.0; }
            set { Model.Score = (int)value * 100; }
        }
        public string RateString { get { return Rate.ToString("0.#"); } }

        public double UserRate
        {
            get { return _userRate; }
            set
            {
                _userRate = value;
                //Rate = value;
            }
        }

        public PointDetailsModel Details
        {
            get { return _details; }
            set
            {
                if (Equals(value, _details)) return;
                _details = value;
                OnPropertyChanged("Comments"); 
                OnPropertyChanged("Prices");
            }
        }
        public List<PointDetailsModel.CommentModel> Comments{get { return Details.Comments; }}
        public List<PointDetailsModel.PriceModel> Prices { get { return Details.Price; } }
        public PlacesModel Model { get; set; }
        public Style StylePoint { get; set; }


        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion



    }
}
