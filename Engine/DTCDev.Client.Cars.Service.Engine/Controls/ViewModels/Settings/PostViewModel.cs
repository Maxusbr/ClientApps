﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    public class PostViewModel : ViewModelBase
    {
        public int ID { get; set; }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("DisplayName");
            }
        }

        private string _postType = string.Empty;
        public string PostType
        {
            get { return _postType; }
            set
            {
                _postType = value;
                OnPropertyChanged("PostType");
            }
        }

        private int _startWorkTime = -1;
        public int StartWorkTime
        {
            get { return _startWorkTime; }
            set
            {
                if(_startWorkTime == value) return;
                _startWorkTime = value;
                OnPropertyChanged("StartWorkTime");
                OnPropertyChanged("DisplayName");
            }
        }

        private int _endtWorkTime = -1;
        public int EndWorkTime
        {
            get { return _endtWorkTime; }
            set
            {
                if (_endtWorkTime == value) return;
                _endtWorkTime = value;
                OnPropertyChanged("EndWorkTime");
                OnPropertyChanged("DisplayName");
            }
        }

        public string WorkTime
        {
            get
            {
                return (StartWorkTime != -1 ? string.Format("{0}:00", StartWorkTime.ToString("00")) : "") + "-" + 
                    (EndWorkTime != -1 ? string.Format("{0}:00", EndWorkTime.ToString("00")) : "");
            }
        }

        public bool IsChange { get; set; }

        public override string DisplayName
        {
            get
            {
                return Name + (string.IsNullOrEmpty(WorkTime) ? "" : string.Format(" ({0})", WorkTime));
            }
        }

        //public override string Na ()
        //{
        //    return Name + (string.IsNullOrEmpty(WorkTime) ? "": string.Format(" ({0})",WorkTime));
        //}
    }
}