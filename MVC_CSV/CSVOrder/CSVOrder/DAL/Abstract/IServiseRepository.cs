using System.Collections.Generic;
using CSVOrder.Models.Service;
using CSVOrder.Models.User;

namespace CSVOrder.DAL.Abstract
{
    public interface IServiseRepository
    {
        IEnumerable<OrderModel> Orders { get; }
        IEnumerable<PostModel> Posts { get; }
        IEnumerable<DepartmentModel> Departaments { get; }
        IEnumerable<UserLightModel> Users { get; }

        void SaveOrder(CarOrderPostModel order);
        OrderModel DeleteOrder(int orderId);
        CarOrderPostModel GetOrder(int orderId);

        void SavePost(PostModel post);
        PostModel DeletePost(int postId);
        PostModel GetePost(int postId);

        void SaveDepartament(DepartmentModel departament);
        DepartmentModel DeleteDepartament(int departamentId);
        DepartmentModel GetDepartament(int departamentId);
    }
}