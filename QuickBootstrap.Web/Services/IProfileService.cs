
using QuickBootstrap.Entities;

namespace QuickBootstrap.Services
{
    public interface IProfileService
    {
        User GetUserInfo(string username = "");

        bool ChangePassword(string username = "", string newPassword = "");
    }
}