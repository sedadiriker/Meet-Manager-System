using EntitiesLayer.Models;
namespace BusinessLayer.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string toEmail, string name);
        Task SendMeetingNotificationAsync(string toEmail, Meeting meeting); 
    }
}
