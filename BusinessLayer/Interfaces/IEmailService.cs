
namespace BusinessLayer.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string email, string firstName);
    }
}
