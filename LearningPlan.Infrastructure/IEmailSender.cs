using LearningPlan.Infrastructure.Model;

namespace LearningPlan.Infrastructure;

public interface IEmailSender
{
    Task SendAsync(Message message);
}