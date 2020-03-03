using ApplicationCore.Entities;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ITaskRepository : IRepository<TaskObject>
    {
        Task<TaskObject> GetByIdWithChildren(int id);
    }
}
