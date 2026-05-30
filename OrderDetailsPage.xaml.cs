using System;
using System.Windows;
using System.Windows.Controls;
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

            if (MainPage.CurrentUser != null)
            {
                EnterButton.Visibility = Visibility.Collapsed;
                ExitButton.Visibility = Visibility.Visible;
            }

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

            try
            {
                string imagePath = _currentOrder.ImagePath;

                // Ищем в корневой папке приложения
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
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new MainPage());
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