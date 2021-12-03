using System.Threading.Tasks;
using LearningPlan.DomainModel;

namespace LearningPlan.ObjectServices
{
    public interface IUserObjectService
    {
        Task<User> GetUserByUserNameAsync(string username);
        Task<User> GetUserByIdAsync(string id);
        Task CreateUserAsync(User user);
    }
}