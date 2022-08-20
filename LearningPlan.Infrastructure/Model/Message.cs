namespace LearningPlan.Infrastructure.Model;

public class Message
{
    public Message(IEnumerable<string> to, string subject, string content, bool isHtml = false)
    {
        To.AddRange(to);
        Subject = subject;
        Content = content;
        IsHtml = isHtml;
    }

    public List<string> To { get; set; } = new();
    public string Subject { get; set; }
    public string Content { get; set; }
    public bool IsHtml { get; }
}