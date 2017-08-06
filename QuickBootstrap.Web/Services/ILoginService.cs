namespace QuickBootstrap.Services
{
    public interface ILoginService
    {
        bool Login(string username, string password);

        void Logout(string username);
    }
}