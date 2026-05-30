using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Kyrsovai
{
    public partial class MainPage : Page
    {
        public static string UserName { get; set; } = "Гость";
        public static List<User> Users { get; set; } = new List<User>();
        public static User CurrentUser { get; set; } = null;
        public static List<Order> Orders { get; set; } = new List<Order>();

        public MainPage()
        {
            InitializeComponent();

            LoadDataFromFile();

            if (Orders.Count == 0)
            {
                AddDefaultOrders();
            }

            GuestText.Text = UserName;
            UpdateButtonsVisibility();
            UpdateOrdersList();
        }

        private void LoadDataFromFile()
        {
            var data = FileHelper.LoadData();
            if (data != null)
            {
                if (data.Users != null && data.Users.Count > 0)
                {
                    Users.Clear();
                    foreach (var user in data.Users)
                        Users.Add(user);
                }

                if (data.Orders != null && data.Orders.Count > 0)
                {
                    Orders.Clear();
                    foreach (var order in data.Orders)
                        Orders.Add(order);
                }

                if (!string.IsNullOrEmpty(data.CurrentUserEmail))
                {
                    CurrentUser = Users.Find(u => u.Email == data.CurrentUserEmail);
                    if (CurrentUser != null)
                        UserName = "Добро пожаловать, " + CurrentUser.UserName + "!";
                }
            }
        }

        private void AddDefaultOrders()
        {
            Orders.Add(new Order
            {
                OrderTitle = "Создать логотип для бренда",
                Description = "Разработка уникального логотипа",
                Deadline = DateTime.Now.AddDays(7),
                Price = 500,
                Rating = 4.8,
                ImagePath = "логотип.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Оформление ВК",
                Description = "Оформление сообщества ВКонтакте",
                Deadline = DateTime.Now.AddDays(3),
                Price = 1000,
                Rating = 3.8,
                ImagePath = "вк1.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Написание портфолио",
                Description = "Составление профессионального портфолио",
                Deadline = DateTime.Now.AddDays(5),
                Price = 500,
                Rating = 5.0,
                ImagePath = "портфолио.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Карточки для маркетплейсов",
                Description = "Создание карточек товаров",
                Deadline = DateTime.Now.AddDays(10),
                Price = 1500,
                Rating = 4.5,
                ImagePath = "карточкатовара.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Написание SEO-статьи",
                Description = "SEO-оптимизированная статья",
                Deadline = DateTime.Now.AddDays(4),
                Price = 700,
                Rating = 4.4,
                ImagePath = "статья1.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Рерайт 10000 знаков",
                Description = "Качественный рерайт текста",
                Deadline = DateTime.Now.AddDays(2),
                Price = 5000,
                Rating = 2.8,
                ImagePath = "рерайт.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Вёрстка сайта на Tilda",
                Description = "Профессиональная вёрстка",
                Deadline = DateTime.Now.AddDays(14),
                Price = 5000,
                Rating = 4.9,
                ImagePath = "тильда.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Парсинг сайта по заданным ключам",
                Description = "Сбор данных с сайтов",
                Deadline = DateTime.Now.AddDays(5),
                Price = 2500,
                Rating = 3.8,
                ImagePath = "парсинг.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Настройка контекстной рекламы",
                Description = "Настройка рекламных кампаний",
                Deadline = DateTime.Now.AddDays(3),
                Price = 1000,
                Rating = 3.8,
                ImagePath = "реклама.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Расчёт себестоимости товара",
                Description = "Экономический расчёт",
                Deadline = DateTime.Now.AddDays(1),
                Price = 700,
                Rating = 4.5,
                ImagePath = "себестоимость.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Создание ВК-бота",
                Description = "Разработка бота для ВКонтакте",
                Deadline = DateTime.Now.AddDays(7),
                Price = 5000,
                Rating = 4.3,
                ImagePath = "вк2.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Перевод текста на арабский",
                Description = "Профессиональный перевод",
                Deadline = DateTime.Now.AddDays(3),
                Price = 2000,
                Rating = 5.0,
                ImagePath = "перевод.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Создание ИИ-видео",
                Description = "Генерация видео с помощью ИИ",
                Deadline = DateTime.Now.AddDays(5),
                Price = 5000,
                Rating = 4.8,
                ImagePath = "видео.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Ведение аккаунта на Озон",
                Description = "Управление аккаунтом продавца",
                Deadline = DateTime.Now.AddDays(30),
                Price = 10000,
                Rating = 4.8,
                ImagePath = "озон.jpg",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Создание делового письма",
                Description = "Составление деловой переписки",
                Deadline = DateTime.Now.AddDays(1),
                Price = 5000,
                Rating = 1.8,
                ImagePath = "письмо.png",
                Author = "Система"
            });

            Orders.Add(new Order
            {
                OrderTitle = "Написание статьи",
                Description = "Уникальный контент для блога",
                Deadline = DateTime.Now.AddDays(3),
                Price = 500,
                Rating = 4.3,
                ImagePath = "статья2.png",
                Author = "Система"
            });
        }

        public void UpdateButtonsVisibility()
        {
            if (CurrentUser != null)
            {
                EnterButton.Visibility = Visibility.Collapsed;
                ExitButton.Visibility = Visibility.Visible;
            }
            else
            {
                EnterButton.Visibility = Visibility.Visible;
                ExitButton.Visibility = Visibility.Collapsed;
            }
        }

        public void UpdateOrdersList()
        {
            OrdersList.ItemsSource = null;
            OrdersList.ItemsSource = Orders.AsEnumerable().Reverse().ToList();
        }

        private void ButtonVoiti_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new LoginPage());
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = null;
            UserName = "Гость";
            GuestText.Text = UserName;
            UpdateButtonsVisibility();
            FileHelper.SaveData(Users, Orders, CurrentUser);
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateOrdersList();
        }

        private void OrderCard_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            Order selectedOrder = border?.DataContext as Order;

            if (selectedOrder != null)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFrame.Navigate(new OrderDetailsPage(selectedOrder));
            }
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var img = sender as Image;
            img.Source = new BitmapImage(new Uri("pack://application:,,,/logo.png", UriKind.Absolute));
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