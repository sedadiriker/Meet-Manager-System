using System.Threading.Tasks;
using api.Interfaces;

namespace api.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendWelcomeEmailAsync(string email, string firstName)
        {
            // E-posta gönderme işlemi burada yapılacak.
            // Bu örnekte sadece bir task tamamlanmış olarak dönüyoruz.
            await Task.CompletedTask;
        }
    }
}
