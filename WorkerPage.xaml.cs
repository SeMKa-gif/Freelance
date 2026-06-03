using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace Kyrsovai
{
    public partial class WorkerPage : Page
    {
        private string selectedImagePath = "logo.png";

        public WorkerPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GuestText.Text = MainPage.UserName;

            if (MainPage.CurrentUser == null)
            {
                PublishButton.IsEnabled = false;
                PublishButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f5b5d0"));
                EnterButton.Visibility = Visibility.Visible;
                ExitButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                PublishButton.IsEnabled = true;
                PublishButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10b364"));
                EnterButton.Visibility = Visibility.Collapsed;
                ExitButton.Visibility = Visibility.Visible;
            }
            UpdateAuthButtons();
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

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string sourcePath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(sourcePath);

                string destPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                System.IO.File.Copy(sourcePath, destPath, true);

                selectedImagePath = fileName;
                ImagePathBox.Text = fileName;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(sourcePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                PreviewImage.Source = bitmap;
            }
        }

        private void ButtonPublic_Click(object sender, RoutedEventArgs e)
        {
            if (MainPage.CurrentUser == null)
            {
                MessageBox.Show("Для создания заказа необходимо войти в аккаунт");
                return;
            }

            string name = NameWorkBox.Text.Trim();
            string opisanie = OpisanieBox.Text.Trim();
            DateTime? time = TimeBox.SelectedDate;
            string priceText = PriceBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите название заказа");
                return;
            }
            if (string.IsNullOrWhiteSpace(opisanie))
            {
                MessageBox.Show("Введите описание работы");
                return;
            }
            if (time == null)
            {
                MessageBox.Show("Выберите дату окончания");
                return;
            }
            if (string.IsNullOrWhiteSpace(priceText))
            {
                MessageBox.Show("Введите цену");
                return;
            }
            if (!int.TryParse(priceText, out int price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (только положительные числа)");
                return;
            }

            Order newOrder = new Order
            {
                OrderId = MainPage.Orders.Count + 1,
                OrderTitle = name,
                Description = opisanie,
                Deadline = time.Value,
                Price = price,
                Rating = 0,
                ImagePath = selectedImagePath,
                Author = MainPage.CurrentUser?.UserName ?? "Неизвестен",
                AuthorEmail = MainPage.CurrentUser?.Email ?? ""
            };

            MainPage.Orders.Add(newOrder);
            MessageBox.Show($"Заказ \"{name}\" успешно опубликован!");

            NameWorkBox.Clear();
            OpisanieBox.Clear();
            TimeBox.SelectedDate = null;
            PriceBox.Clear();
            ImagePathBox.Clear();
            PreviewImage.Source = null;
            selectedImagePath = "logo.png";

            
            FileHelper.SaveData(MainPage.Users, MainPage.Orders, MainPage.Bids, MainPage.Messages, MainPage.CurrentUser);

            

            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow?.MainFrame.Content is MainPage mainPage)
            {
                mainPage.UpdateOrdersList();
            }
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


        private void UpdateButtonsVisibility()
        {
            if (MainPage.CurrentUser != null)
            {
                MyBidsButton.Visibility = Visibility.Visible;
            }
            else
            {
                MyBidsButton.Visibility = Visibility.Collapsed;
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

        private void MyBidsButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new MyBidsPage());
        }

        private void OrderBidsButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new OrderBidsPage());
        }
    }
}