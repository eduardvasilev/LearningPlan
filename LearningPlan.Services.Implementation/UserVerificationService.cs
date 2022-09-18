using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearningPlan.DomainModel;
using LearningPlan.Infrastructure;
using LearningPlan.Infrastructure.Model;
using LearningPlan.ObjectServices;
using Microsoft.Extensions.Options;

namespace LearningPlan.Services.Implementation;

public class UserVerificationService : IUserVerificationService
{
    private readonly IUserActivationCodeObjectService _userActivationCodeObjectService;
    private readonly IEmailSender _emailSender;
    private readonly FeatureOptions _featureOptions;
    private readonly FrontEndOptions _frontEndOptions;

    public UserVerificationService(IUserActivationCodeObjectService userActivationCodeObjectService,
        IEmailSender emailSender,
        IOptions<FrontEndOptions> frontEndOptions,
        IOptions<FeatureOptions> featureOptions)
    {
        _userActivationCodeObjectService = userActivationCodeObjectService;
        _emailSender = emailSender;
        _featureOptions = featureOptions.Value;
        _frontEndOptions = frontEndOptions.Value;
    }

    public async Task SendUserVerificationEmail(User user)
    {
        if (_featureOptions.EmailVerificationEnabled)
        {
            UserActivationCode code = await _userActivationCodeObjectService.CreateCodeAsync(user);
            Uri link = new Uri(new Uri(_frontEndOptions.BaseUrl), $"user/activate/{code.Code}");
            string email = string.Format(EmailTemplates.EmailTeamplates.ActivateAccount, link);

            await _emailSender.SendAsync(new Message(new List<string> { user.Username }, "Activate your account", email, true));
        }
    }

    public void CheckIfUserVerified(User user)
    {
        if (!user.IsApproved && _featureOptions.EmailVerificationEnabled)
        {
            throw new UnauthorizedAccessException($"User {user.Username} is not approved.");
        }
    }
}