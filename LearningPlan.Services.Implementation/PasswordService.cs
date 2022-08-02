using System.Text.RegularExpressions;
using LearningPlan.DomainModel.Exceptions;

namespace LearningPlan.Services.Implementation;

public class PasswordService : IPasswordService
{
    public void ValidatePassword(string password)
    {
        if (password.Length < 8)
        {
            throw new DomainServicesException("Password length should be at least 8 characters.");
        }

        Regex regex =
            new Regex(
                @"^(?=.*\d)(?=.*[\p{L}])(?=.*[\p{Lu}])(?=.*[\u0020-\u002F\u003A-\u0040\u005B-\u0060\u007B-\u00FF\u0100-\uFFFF])(?=.*[\\p{Lt}]|).{8,254}$");
        if (!regex.IsMatch(password))
        {
            throw new DomainServicesException("Password should contain lower-case, upper-case and special characters.");
        }
    }
}