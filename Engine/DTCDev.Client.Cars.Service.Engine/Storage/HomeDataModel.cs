using DTCDev.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DTCDev.Client.Cars.Service.Engine.Storage
{
    public class HomeDataModel : ViewModelBase
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                this.OnPropertyChanged("Text");
            }
        }

        private string _mainText1 = "";
        public string MainText1
        {
            get { return _mainText1; }
            set
            {
                _mainText1 = value;
                this.OnPropertyChanged("MainText1");
            }
        }

        private string _mainComment1 = "";
        public string MainComment1
        {
            get { return _mainComment1; }
            set
            {
                _mainComment1 = value;
                this.OnPropertyChanged("MainComment1");
            }
        }

        private string _mainText2 = "";
        public string MainText2
        {
            get { return _mainText2; }
            set
            {
                _mainText2 = value;
                this.OnPropertyChanged("MainText2");
            }
        }

        private string _mainComment2 = "";
        public string MainComment2
        {
            get { return _mainComment2; }
            set
            {
                _mainComment2 = value;
                this.OnPropertyChanged("MainComment2");
            }
        }



        private string _secondText1 = "";
        public string SecondText1
        {
            get { return _secondText1; }
            set
            {
                _secondText1 = value;
                this.OnPropertyChanged("SecondText1");
            }
        }

        private string _secondComment1 = "";
        public string SecondComment1
        {
            get { return _secondComment1; }
            set
            {
                _secondComment1 = value;
                this.OnPropertyChanged("SecondComment1");
            }
        }

        private string _secondText2 = "";
        public string SecondText2
        {
            get { return _secondText2; }
            set
            {
                _secondText2 = value;
                this.OnPropertyChanged("SecondText2");
            }
        }

        private string _secondComment2 = "";
        public string SecondComment2
        {
            get { return _secondComment2; }
            set
            {
                _secondComment2 = value;
                this.OnPropertyChanged("SecondComment2");
            }
        }

        public int AnimatePosition { get; set; }

        private string _image;

        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                this.OnPropertyChanged("Image");
            }
        }





        public BGColor Color { get; set; }


        public enum BGColor
        {
            LightBlue,
            Blue,
            Red,
            Green,
            Purpur
        }
    }
}
