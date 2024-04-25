using System.Windows;
using WPF_lab3.Dto;
using WPF_lab3.Service;

namespace WPF_lab3
{
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService;
        public int LoggedUserId { get; set; }

        public LoginWindow(string connectionString)
        {
            InitializeComponent();
            _userService = new UserService(connectionString);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = Login_TextBox.Text;
            var password = Password_TextBox.Password;


            var userId = _userService.Login(username, password);

            if (userId != -1)
            {
                LoggedUserId = userId;
                MessageBox.Show($"Hello {username}!");
                this.Close();
            }
            else if (_userService.GetUserByUsername(username))
            {
                MessageBox.Show("Bad password.");

            }
            else
            {
                MessageBox.Show("Login failed. Please check your credentials and try again.");
            }
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            var username = Login_TextBox.Text;
            var password = Password_TextBox.Password;

            if (_userService.GetUserByUsername(username))
            {
                MessageBox.Show("Failed. Login is already taken.");
                return;
            }

            _userService.CreateUser(new UserDto(username, password));
            MessageBox.Show("Account created successfully.\nNow you can log in.");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
