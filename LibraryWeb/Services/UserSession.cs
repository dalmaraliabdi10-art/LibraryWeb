using LibraryWeb.Models;

namespace LibraryWeb.Services
{
    public class UserSession
    {
        public Member? CurrentUser { get; private set; }
        public void Login(Member member) => CurrentUser = member;
        public void Logout() => CurrentUser = null;
        public bool IsLoggedIn => CurrentUser != null;
        public bool IsAdmin => CurrentUser != null && CurrentUser.IsAdmin;
    }
}