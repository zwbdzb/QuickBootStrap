 
using System.Linq;
using QuickBootstrap.Services.Util;

namespace QuickBootstrap.Services.Impl
{
    public sealed class LoginService : ServiceContext, ILoginService
    {
        public bool Login(string username, string password)
        {
            return DbContext.User.FirstOrDefault(p => p.UserName == username && p.UserPwd == password) != null;
        }

        public void Logout(string username)
        {
            //记录用户注销日志
        }
    }
}