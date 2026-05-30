using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kyrsovai
{
    public partial class Registracia : Page
    {
        public Registracia()
        {
            InitializeComponent();
        }

        private void ButtonRegi_Click(object sender, RoutedEventArgs e)
        {
            string surname = SurnameBox.Text.Trim();
            string name = NameBox.Text.Trim();
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(surname))
            {
                MessageBox.Show("Введите фамилию");
                return;
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите имя");
                return;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Введите email");
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Повторите пароль");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Введите корректный email (пример: user@mail.com)");
                return;
            }

            if (surname.Length > 30)
            {
                MessageBox.Show("Фамилия не может превышать 30 символов");
                return;
            }
            if (name.Length > 30)
            {
                MessageBox.Show("Имя не может превышать 30 символов");
                return;
            }
            if (email.Length > 30)
            {
                MessageBox.Show("Email не может превышать 30 символов");
                return;
            }
            if (password.Length > 30)
            {
                MessageBox.Show("Пароль не может превышать 30 символов");
                return;
            }

            var existingUser = MainPage.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким email уже зарегистрирован");
                return;
            }

            User newUser = new User
            {
                Surname = surname,
                UserName = name,
                Email = email,
                Password = password
            };

            MainPage.Users.Add(newUser);
            MainPage.CurrentUser = newUser;
            MainPage.UserName = "Добро пожаловать, " + name + "!";

            FileHelper.SaveData(MainPage.Users, MainPage.Orders, MainPage.CurrentUser);

            MessageBox.Show("Регистрация успешна!");

            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new MainPage());
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                string domain = addr.Host;
                string[] validDomains = { "com", "ru", "net", "org", "by", "kz", "ua" };
                string domainExtension = domain.Split('.').Last();
                return addr.Address == email && validDomains.Contains(domainExtension);
            }
            catch
            {
                return false;
            }
        }

        private void OtmenaButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new MainPage());
        }

        private void Agreement_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string filePath = "Соглашение.docx";
            if (System.IO.File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Файл не найден: " + filePath);
            }
        }

        private void Privacy_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string filePath = "Политика.docx";
            if (System.IO.File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Файл не найден: " + filePath);
            }
        }

        private void Support_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string filePath = "Помощь.docx";
            if (System.IO.File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Файл не найден: " + filePath);
            }
        }
    }
}