using LearningPlan.Infrastructure.Model;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace LearningPlan.Infrastructure.Implementation;

public class MailKitEmailSender : IEmailSender
{
    private readonly ISmtpClient _smtpClient;
    private readonly EmailOptions _emailOptions;

    public MailKitEmailSender(ISmtpClient smtpClient, IOptions<EmailOptions> emailOptions)
    {
        _smtpClient = smtpClient;
        _emailOptions = emailOptions.Value ?? throw new InvalidOperationException("Email sending isn't configured.");
    }

    public async Task SendAsync(Message message)
    {
        MimeMessage emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailOptions.UserName, _emailOptions.From));
        emailMessage.To.AddRange(message.To.Select(m => new MailboxAddress(m, m)));
        emailMessage.Subject = message.Subject;

        TextPart body = (message.IsHtml ? new TextPart(TextFormat.Html) : new TextPart(TextFormat.Plain));

        body.Text = message.Content;

        emailMessage.Body = body;

        await Send(emailMessage);
    }

    private async Task Send(MimeMessage emailMessage)
    {
        await _smtpClient.SendAsync(emailMessage);
        await _smtpClient.DisconnectAsync(true);
    }
}