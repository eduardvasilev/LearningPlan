using System;
using System.Threading.Tasks;
using LearningPlan.DomainModel;
using LearningPlan.Services.Model;

namespace LearningPlan.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponseModel> AuthenticateAsync(AuthenticateRequestModel model);
        Task SignUpAsync(SignInServiceModel model);
        Task<User> GetByIdAsync(string id);
        Task ActivateUserAsync(Guid activationCode);
    }
}