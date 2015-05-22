using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using KOT.Annotations;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class ServiceInfoViewModel : INotifyPropertyChanged
    {
        private double _userRate = -1;
        private double _oldvalue = -1;
        private bool _showAllPrices;
        private PointDetailsModel _details = new PointDetailsModel();
        private CommentsDetailModel _commentsDetail = new CommentsDetailModel();
        private string _userComment;

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

        public Color Brush
        {
            get { return _userRate < 0 ? Color.FromArgb(255, 33, 33, 33) : Color.FromArgb(255, 233, 30, 99); }
        }

        public Brush Background
        {
            get
            {
                return new SolidColorBrush(Brush);
            }
        }
        public double Rate
        {
            get { return Model.Score / 100.0; }
            set
            {
                Model.Score = (int)(value * 100);
                OnPropertyChanged("Rate");
            }
        }
        public string RateString { get { return Rate.ToString("0.#"); } }

        public double UserRate
        {
            get { return _userRate; }
            set
            {
                _userRate = value;
                if(value > 0)
                {
                    _oldvalue = Model.Score / 100.0;
                    Rate = value;
                }
                else
                {
                    Rate = _oldvalue;
                }
                OnPropertyChanged("UserRate");
                OnPropertyChanged("RateString");
                OnPropertyChanged("Brush");
                OnPropertyChanged("Background");
                OnPropertyChanged("IsUserRate");
            }
        }

        public bool IsUserRate { get { return _userRate > 0; } }

        public PointDetailsModel Details
        {
            get { return _details; }
            set
            {
                if (Equals(value, _details)) return;
                _details = value;
                OnPropertyChanged("Comments");
                OnPropertyChanged("Prices");
                CommentsDetail = new CommentsDetailModel(value.Comments);
                OnPropertyChanged("VisableListComment");
            }
        }

        public bool VisableListComment { get { return ListComment.Count > 0; } }
        public List<PointDetailsModel.CommentModel> Comments { get { return Details.Comments; } }
        public List<PointDetailsModel.PriceModel> Prices { get { return !ShowAllPrices? Details.Price.Take(3).ToList(): Details.Price; } }
        public PlacesModel Model { get; set; }
        public Style StylePoint { get; set; }

        public bool ShowAllPrices
        {
            get { return _showAllPrices; }
            set
            {
                if (value.Equals(_showAllPrices)) return;
                _showAllPrices = value;
                OnPropertyChanged("Prices");
            }
        }

        public CommentsDetailModel CommentsDetail
        {
            get { return _commentsDetail; }
            set
            {
                _commentsDetail = value; 
                OnPropertyChanged("CommentsDetail");
                OnPropertyChanged("ListComment");
            }
        }

        public List<RateClass> ListComment { get { return CommentsDetail.ListComment; } }

        public string UserComment
        {
            get { return _userComment; }
            set
            {
                if (value == _userComment) return;
                _userComment = value;
                OnPropertyChanged("UserComment");
            }
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        public class CommentsDetailModel
        {
            private readonly List<PointDetailsModel.CommentModel> _list = new List<PointDetailsModel.CommentModel>();
            private readonly List<RateClass> _listComment = new List<RateClass>();
            public CommentsDetailModel() { }
            public CommentsDetailModel(List<PointDetailsModel.CommentModel> listComment)
            {
                _list = listComment;
                foreach (var el in _list)
                {
                    _listComment.Add(new RateClass(el));
                }
                if (DesignMode.DesignModeEnabled)
                {
                    _list.Add(new PointDetailsModel.CommentModel { Date = new DateDataModel(DateTime.Now), Score = 80, Text = "Text1" });
                    _list.Add(new PointDetailsModel.CommentModel { Date = new DateDataModel(DateTime.Now), Score = 40, Text = "Text2" });

                }
                _count1 = _list.Where(o => o.Score <= 20).ToList().Count;
                _count2 = _list.Where(o => o.Score > 20 && o.Score <= 40).ToList().Count;
                _count3 = _list.Where(o => o.Score > 40 && o.Score <= 60).ToList().Count;
                _count4 = _list.Where(o => o.Score > 60 && o.Score <= 80).ToList().Count;
                _count5 = _list.Where(o => o.Score > 80 && o.Score <= 100).ToList().Count;
            }
            public string TotalValue { get { return _list.Count > 0 ? _list.Average(o => o.Score / 100.0).ToString("0.#") : ""; } }
            public string CountComments { get { return string.Format("Отзывов: {0}", Count); } }
            public int Count { get { return _list.Count; } }

            public string Count1Star { get { return _count1 > 0 ? _count1.ToString() : ""; } }
            public string Count2Star { get { return _count2 > 0 ? _count2.ToString() : ""; } }
            public string Count3Star { get { return _count3 > 0 ? _count3.ToString() : ""; } }
            public string Count4Star { get { return _count4 > 0 ? _count4.ToString() : ""; } }
            public string Count5Star { get { return _count5 > 0 ? _count5.ToString() : ""; } }

            private readonly int _count1;
            private readonly int _count2;
            private readonly int _count3;
            private readonly int _count4;
            private readonly int _count5;

            public double Value1Star { get { return (_count1 * 1.0) / Count; } }
            public double Value2Star { get { return (_count2 * 1.0) / Count; } }
            public double Value3Star { get { return (_count3 * 1.0) / Count; } }
            public double Value4Star { get { return (_count4 * 1.0) / Count; } }
            public double Value5Star { get { return (_count5 * 1.0) / Count; } }

            public List<RateClass> ListComment { get { return _listComment; } }
        }

        public class RateClass: PointDetailsModel.CommentModel
        {
            public RateClass(PointDetailsModel.CommentModel model)
            {
                Date = model.Date;
                Text = model.Text;
                Score = model.Score;
            }

            public string Dates { get { return new DateTime(Date.Y, Date.M, Date.D).ToString("d"); } }

            public double Rate { get { return Score / 100.0; } }
        }

        internal async void SendComment()
        {
            await DataSource.SendComment(UserRate, UserComment);
        }
    }
}
