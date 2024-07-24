using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string email, string firstName);
    }
}
