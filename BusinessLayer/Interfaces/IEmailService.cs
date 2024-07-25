
namespace BusinessLayer.Interfaces
{
   public interface IEmailService
{
    Task SendWelcomeEmailAsync(string toEmail, string name);
}
}
