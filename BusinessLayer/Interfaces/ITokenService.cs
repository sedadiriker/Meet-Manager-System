using EntitiesLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
