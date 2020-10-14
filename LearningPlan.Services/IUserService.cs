using System.Threading.Tasks;
using LearningPlan.DomainModel;
using LearningPlan.Services.Model;

namespace LearningPlan.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponseModel> AuthenticateAsync(AuthenticateRequestModel model);
        Task SignInAsync(SignInServiceModel model);

        Task<User> GetByIdAsync(string id);
    }
}