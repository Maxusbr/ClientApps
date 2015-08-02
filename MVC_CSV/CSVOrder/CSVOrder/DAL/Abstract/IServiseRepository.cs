using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSVOrder.Models.Abstract;
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
        IEnumerable<WorksInfoDataModel> Works { get; }

        void SaveOrder(CarOrderPostModel order);
        OrderModel DeleteOrder(int orderId);
        CarOrderPostModel GetOrder(int orderId);

        void SavePost(PostModel post);
        PostModel DeletePost(int postId);
        PostModel GetePost(int postId);

        void SaveDepartament(DepartmentModel departament);
        DepartmentModel DeleteDepartament(int departamentId);
        DepartmentModel GetDepartament(int departamentId);

        CarViewModel GetCar(string carNumber);

        List<KVPBase> Marks { get; }
        List<KVPBase> Models { get; }
        List<KVPBase> BodyTypes { get; }
        List<KVPBase> EngineTypes { get; }
        List<KVPBase> EngineVolumes { get; }
        List<KVPBase> TransTypes { get; }

        IEnumerable<KVPBase> GetMarks();
        IEnumerable<KVPBase> GetModels(int indx);
        IEnumerable<KVPBase> GetBodyTypes(int indx);
        IEnumerable<KVPBase> GetEngineTypes(int indx);
        IEnumerable<KVPBase> GetEngineVolumes(int indx);
        IEnumerable<KVPBase> GetTransTypes(int indx);
    }
}