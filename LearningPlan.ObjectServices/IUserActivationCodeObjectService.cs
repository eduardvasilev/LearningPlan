using System;
using System.Threading.Tasks;
using LearningPlan.DomainModel;

namespace LearningPlan.ObjectServices;

public interface IUserActivationCodeObjectService : IObjectService
{
    Task<UserActivationCode> GetCodeByUserAsync(User user);

    Task<UserActivationCode> GetCodeAsync(Guid code);

    Task<UserActivationCode> CreateCodeAsync(User user);
}