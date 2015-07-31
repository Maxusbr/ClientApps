using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSVOrder.DAL.Abstract;
using CSVOrder.Models.Service;
using CSVOrder.Models.User;

namespace CSVOrder.DAL.Concrete
{
    public class ServiseRepository : IServiseRepository
    {
        EFDbContext _context;
        private readonly List<UserLightModel> _users = new List<UserLightModel>();
        private readonly List<OrderModel> _orders = new List<OrderModel>();
        private readonly List<PostModel> _posts = new List<PostModel>();
        private readonly List<DepartmentModel> _departaments = new List<DepartmentModel>();
        private readonly List<WorksInfoDataModel> _works = new List<WorksInfoDataModel>();

        public ServiseRepository()
        {
            _context = new EFDbContext("//Add connection ctring");
            DemoData();
        }

        /// <summary>
        /// Db context orders
        /// </summary>
        public IEnumerable<OrderModel> Orders
        {
            get { return _orders; }
        }

        /// <summary>
        /// Db context Posts
        /// </summary>
        public IEnumerable<PostModel> Posts
        {
            get { return _posts; }
        }

        /// <summary>
        /// Db context Departaments
        /// </summary>
        public IEnumerable<DepartmentModel> Departaments
        {
            get { return _departaments; }
        }

        /// <summary>
        /// Db context Users
        /// </summary>
        public IEnumerable<UserLightModel> Users
        {
            get { return _users; }
        }

        /// <summary>
        /// Db context Works
        /// </summary>
        public IEnumerable<WorksInfoDataModel> Works
        {
            get { return _works; }
        }

        public void SaveOrder(CarOrderPostModel order)
        {
            //TODO Save full model to db
        }

        public OrderModel DeleteOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public void SavePost(PostModel post)
        {
            throw new NotImplementedException();
        }

        public PostModel DeletePost(int postId)
        {
            throw new NotImplementedException();
        }

        public void SaveDepartament(DepartmentModel departament)
        {
            throw new NotImplementedException();
        }

        public DepartmentModel DeleteDepartament(int departamentId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить полную модель заказа из бд, включая данные пользователя, список работ, постId и т.д.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public CarOrderPostModel GetOrder(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.OrderNumer == orderId);
            //TODO get post and user info

            if (order == null)
                return new CarOrderPostModel();
            var fullorder = new CarOrderPostModel(order) { User = _users.FirstOrDefault(o => o.Id == order.UserId) };
            return fullorder;
        }

        public PostModel GetePost(int postId)
        {
            return Posts.FirstOrDefault(o => o.Id == postId);
        }

        public DepartmentModel GetDepartament(int departamentId)
        {
            throw new NotImplementedException();
        }

        void DemoData()
        {
            for (var i = 1; i <= 5; i++)
                _posts.Add(new PostModel
                {
                    Id = i,
                    Name = string.Format("Пост № {0:00}", i),
                    TimeFrom = new TimeSpan(i % 2 == 0 ? 8 : 9, 0, 0),
                    TimeTo = new TimeSpan(i % 2 == 0 ? 19 : 18, 0, 0)
                });
            _orders.Add(new OrderModel
            {
                OrderNumer = 1,
                Car = new CarViewModel("Demo 1"),
                DateWork = DateTime.Now.AddHours(1).AddMinutes(25),
                DtCreate = DateTime.Now,
                PostId = 4,
                UserId = 1
            });
            _orders.Add(new OrderModel
            {
                OrderNumer = 2,
                Car = new CarViewModel("Demo 2"),
                DateWork = DateTime.Now.AddHours(3).AddMinutes(32),
                DtCreate = DateTime.Now,
                PostId = 2,
                UserId = 2
            });
            _users.Add(new UserLightModel { Id = 1, Nm = "Иванов Иван Иванович" });
            _users.Add(new UserLightModel { Id = 2, Nm = "Петров Петр Петрович" });

            _works.Add(new WorksInfoDataModel { Name = "Периодические", Id = 1, IdParent = 0});
            _works.Add(new WorksInfoDataModel { Name = "Остальные", Id = 2, IdParent = 0 });
            // Значение NavUrl = Id для работы и пустое для подгруппы
            _works.Add(new WorksInfoDataModel { Name = "Двигатель", Id = 3, IdParent = 1, NavUrl = "3", Nh = 15});
            _works.Add(new WorksInfoDataModel { Name = "КПП", Id = 4, IdParent = 2, NavUrl = "4", Nh = 5 });
        }


        public CarViewModel GetCar(string carNumber)
        {
            return new CarViewModel(carNumber);
        }


    }
}