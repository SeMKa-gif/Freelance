using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace Kyrsovai
{
    public partial class OrderDetailsPage : Page
    {
        private Order _currentOrder;

        public OrderDetailsPage(Order order)
        {
            InitializeComponent();
            _currentOrder = order;

            GuestText.Text = MainPage.UserName;
            UpdateAuthButtons();
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            OrderTitle.Text = _currentOrder.OrderTitle;
            OrderDescription.Text = _currentOrder.Description;
            OrderDeadline.Text = $"Срок выполнения: {_currentOrder.Deadline:dd.MM.yyyy}";
            OrderPrice.Text = $"{_currentOrder.Price} ₽";
            OrderRating.Text = $"⭐ {_currentOrder.Rating}";
            OrderAuthor.Text = $"Автор: {_currentOrder.Author}";

            // Загрузка изображения
            try
            {
                string imagePath = _currentOrder.ImagePath;
                string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);

                if (System.IO.File.Exists(fullPath))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullPath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    OrderImage.Source = bitmap;
                }
                else
                {
                    OrderImage.Source = new BitmapImage(new Uri("pack://application:,,,/logo.png", UriKind.Absolute));
                }
            }
            catch
            {
                OrderImage.Source = new BitmapImage(new Uri("pack://application:,,,/logo.png", UriKind.Absolute));
            }

            // Проверка: откликался ли ТЕКУЩИЙ пользователь на ЭТОТ заказ
            var myBid = MainPage.Bids.FirstOrDefault(b => b.OrderId == _currentOrder.OrderId && b.FreelancerEmail == MainPage.CurrentUser?.Email);

            if (myBid != null)
            {
                RespondButton.IsEnabled = false;
                RespondButton.Content = "Вы уже откликнулись";
                RespondButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f5b5d0"));
            }
            else if (MainPage.CurrentUser?.Email == _currentOrder.AuthorEmail)
            {
                RespondButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                RespondButton.IsEnabled = true;
                RespondButton.Content = "Откликнуться";
                RespondButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10b364"));
                RespondButton.Visibility = Visibility.Visible;
            }

            if (MainPage.CurrentUser?.Email == _currentOrder.AuthorEmail)
            {
                EditButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
            }
        }

        private void UpdateAuthButtons()
        {
            if (MainPage.CurrentUser != null)
            {
                EnterButton.Visibility = Visibility.Collapsed;
                ExitButton.Visibility = Visibility.Visible;
                MyBidsButton.Visibility = Visibility.Visible;
            }
            else
            {
                EnterButton.Visibility = Visibility.Visible;
                ExitButton.Visibility = Visibility.Collapsed;
                MyBidsButton.Visibility = Visibility.Collapsed;
            }
        }

        private void RespondButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.CurrentUser == null)
            {
                MessageBox.Show("Чтобы откликнуться, необходимо войти в аккаунт");
                return;
            }

            if (MainPage.CurrentUser.Email == _currentOrder.AuthorEmail)
            {
                MessageBox.Show("Вы не можете откликнуться на свой собственный заказ");
                return;
            }

            var existingBid = MainPage.Bids.FirstOrDefault(b => b.OrderId == _currentOrder.OrderId && b.FreelancerEmail == MainPage.CurrentUser.Email);
            if (existingBid != null)
            {
                MessageBox.Show("Вы уже откликались на этот заказ");
                return;
            }

            Bid newBid = new Bid
            {
                Id = MainPage.Bids.Count + 1,
                OrderId = _currentOrder.OrderId,
                OrderTitle = _currentOrder.OrderTitle,
                FreelancerName = MainPage.CurrentUser.UserName,
                FreelancerEmail = MainPage.CurrentUser.Email,
                Message = $"Хочу взять ваш заказ \"{_currentOrder.OrderTitle}\"",
                Date = DateTime.Now,
                Status = "pending"
            };

            MainPage.Bids.Add(newBid);

            // СОХРАНЕНИЕ
            FileHelper.SaveData(MainPage.Users, MainPage.Orders, MainPage.Bids, MainPage.Messages, MainPage.CurrentUser);

            RespondButton.IsEnabled = false;
            RespondButton.Content = "Вы откликнулись";
            RespondButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f5b5d0"));

            MessageBox.Show("Отклик отправлен! Заказчик свяжется с вами в чате.");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new MainPage());
        }

        private void MyBidsButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new MyBidsPage());
        }

        private void Iworker1_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new WorkerPage());
        }

        private void Ifrilance_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new MainPage());
        }

        private void ButtonVoiti_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new LoginPage());
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            MainPage.CurrentUser = null;
            MainPage.UserName = "Гость";
            FileHelper.SaveData(MainPage.Users, MainPage.Orders, MainPage.Bids, MainPage.Messages, MainPage.CurrentUser);
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

        private void OrderBidsButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new OrderBidsPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new EditOrderPage(_currentOrder));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Удалить заказ?", "Подтверждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                MainPage.Orders.Remove(_currentOrder);

                var bidsToRemove = MainPage.Bids.Where(b => b.OrderId == _currentOrder.OrderId).ToList();
                foreach (var bid in bidsToRemove) MainPage.Bids.Remove(bid);

                var messagesToRemove = MainPage.Messages.Where(m => m.OrderId == _currentOrder.OrderId).ToList();
                foreach (var msg in messagesToRemove) MainPage.Messages.Remove(msg);

                FileHelper.SaveData(MainPage.Users, MainPage.Orders, MainPage.Bids, MainPage.Messages, MainPage.CurrentUser);

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFrame.Navigate(new MainPage());
            }
        }
    }
}