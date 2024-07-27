
namespace BusinessLayer.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string toEmail, string name);
        Task SendMeetingNotificationEmailAsync(string toEmail, string meetingName, DateTime startDate, DateTime endDate, string description);

    }
}
