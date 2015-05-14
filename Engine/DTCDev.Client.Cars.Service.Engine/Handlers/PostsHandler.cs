using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class PostsHandler
    {
        private static PostsHandler _instance;
        public static PostsHandler Instance
        {
            get { return _instance ?? (_instance = new PostsHandler()); }
        }

        public PostsHandler()
        {
            _instance = this;
            ListPost.CollectionChanged += ListPost_CollectionChanged;
        }

        public event EventHandler ListPostCollectionChanged;
        void ListPost_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (ListPostCollectionChanged != null) ListPostCollectionChanged(sender, e);
        }

        private readonly ObservableCollection<PostViewModel> _listPost = new ObservableCollection<PostViewModel>();
        public ObservableCollection<PostViewModel> ListPost { get { return _listPost; } }

        private readonly List<string> _listTypePost = new List<string>();
        public List<string> ListTypePost { get { return _listTypePost; } }


    }
}
