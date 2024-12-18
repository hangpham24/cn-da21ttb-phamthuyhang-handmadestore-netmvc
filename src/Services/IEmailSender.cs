namespace WebHM.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendEmailsAsync(string[] emails, string subject, string htmlMessage);
    }
}
