using AutoMapper;
using WPF_lab3.Dto;
using WPF_lab3.Mapper;
using WPF_lab3.Model;
using WPF_lab3.Persistence;

namespace WPF_lab3.Service
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;
        public bool IsLoggedIn { get; set; }
        public User LoggedInUser { get; set; }

        public UserService(string connectionString)
        {
            _appDbContext = new AppDbContext(connectionString);
        }

        public void CreateUser(UserDto user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;

            _appDbContext.CreateUser(user);
        }

        public int Login(string username, string password)
        {
            var user = _appDbContext.GetUserByUsername(username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                LoggedInUser = user;
                IsLoggedIn = true;
                return user.Id;
            }

            return -1;
        }

        public bool GetUserByUsername(string username)
        {
            var user = _appDbContext.GetUserByUsername(username);
            return user != null;
        }
    }
}
