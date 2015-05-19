using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.Service;
using DTCDev.Models;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    public class SlidePostViewModel : ViewModelBase
    {
        private readonly PersonalHandler _handler = PersonalHandler.Instance;

        public SlidePostViewModel()
        {
            UpdatePosts();
            PersonalHandler.Instance.SelectedDepRereshed += Instance_SelectedDepRereshed;
        }

        void Instance_SelectedDepRereshed(object sender, EventArgs e)
        {
            UpdatePosts();
        }

        private void UpdatePosts()
        {
            ListTypePost.Clear();
            ListPost.Clear();
            if (_handler.SelectedDep != null)
            {
                foreach (var item in _handler.Model.PostTypes)
                    ListTypePost.Add(item);
                foreach (var item in _handler.SelectedDep.Posts)
                {
                    ListPost.Add(new PostViewModel(item));
                }
            }
        }

        ObservableCollection<DicDataModel> _listTypePost = new ObservableCollection<DicDataModel>();


        public ObservableCollection<DicDataModel> ListTypePost
        {
            get  { return _listTypePost; }
        }

        private ObservableCollection<PostViewModel> _listPost = new ObservableCollection<PostViewModel>();
        public ObservableCollection<PostViewModel> ListPost { get { return _listPost; } }

        private PostViewModel _selectedPost;
        public PostViewModel SelectedPost
        {
            get { return _selectedPost; }
            set
            {
                _selectedPost = value;
                OnPropertyChanged("SelectedPost");
                OnPropertyChanged("EnableEditPost");
                if(value == null) return;
                OnPropertyChanged("StartWorkTime");
                OnPropertyChanged("EndWorkTime");
            }
        }

        public int StartWorkTime
        {
            get { return SelectedPost != null ? SelectedPost.StartWorkTime: -1; }
            set
            {
                if (SelectedPost == null || SelectedPost.StartWorkTime == value) return;
                SelectedPost.StartWorkTime = value;
                OnPropertyChanged("StartWorkTime");
            }
        }

        public int EndWorkTime
        {
            get { return SelectedPost != null ? SelectedPost.EndWorkTime : -1; }
            set
            {
                if (SelectedPost == null || SelectedPost.EndWorkTime == value) return;
                SelectedPost.EndWorkTime = value;
                OnPropertyChanged("EndWorkTime");
            }
        }

        public bool EnableEditPost
        {
            get { return SelectedPost != null; }
        }

        private bool _completeSaveEnabled = false;
        public bool CompleteSaveEnabled
        {
            get { return _completeSaveEnabled; }
            set
            {
                _completeSaveEnabled = value;
                OnPropertyChanged("CompleteSaveEnabled");
            }
        }

        private RelayCommand _addPostCommand;
        public RelayCommand AddPostCommand
        {
            get { return _addPostCommand ?? (_addPostCommand = new RelayCommand(AddPost)); }
        }

        private void AddPost(object sender)
        {
            var el = new PostViewModel { Name = "Новый пост", StartWorkTime = 8, EndWorkTime = 17 };
            el.PropertyChanged += post_PropertyChanged;
            ListPost.Add(el);
            SelectedPost = el;
            CompleteSaveEnabled = true;
        }

        private void post_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var el = sender as PostViewModel;
            if(el != null)
            {
                el.IsChange =
                CompleteSaveEnabled = true;
            }
        }

        private RelayCommand _completeSaveCommand;
        public RelayCommand CompleteSaveCommand
        {
            get { return _completeSaveCommand ?? (_completeSaveCommand = new RelayCommand(CompleteSave)); }
        }

        private void CompleteSave(object obj)
        {
            SelectedPost = null; CompleteSaveEnabled = false;
        }

    }
}
