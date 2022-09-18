using System.Threading.Tasks;
using LearningPlan.DomainModel;

namespace LearningPlan.Services;

public interface IUserVerificationService
{
    Task SendUserVerificationEmail(User user);
    void CheckIfUserVerified(User user);
}