using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kyrsovai
{
    public partial class MyBidsPage : Page
    {
        public MyBidsPage()
        {
            InitializeComponent();
            Loaded += MyBidsPage_Loaded;
            GuestText.Text = MainPage.UserName;
            UpdateAuthButtons();
        }

        private void MyBidsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBids();
        }

        private void LoadBids()
        {
            var myBids = MainPage.Bids.Where(b => b.FreelancerEmail == MainPage.CurrentUser?.Email).ToList();
            BidsList.ItemsSource = myBids;
        }

        private void UpdateAuthButtons()
        {
            if (MainPage.CurrentUser != null)
            {
                EnterButton.Visibility = Visibility.Collapsed;
                ExitButton.Visibility = Visibility.Visible;
                MyBidsButton.Visibility = Visibility.Visible;
                OrderBidsButton.Visibility = Visibility.Visible;
            }
            else
            {
                EnterButton.Visibility = Visibility.Visible;
                ExitButton.Visibility = Visibility.Collapsed;
                MyBidsButton.Visibility = Visibility.Collapsed;
                OrderBidsButton.Visibility = Visibility.Collapsed;
            }
        }

        private void GoToChat_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Bid bid = btn?.Tag as Bid;

            if (bid != null)
            {
                var order = MainPage.Orders.FirstOrDefault(o => o.OrderId == bid.OrderId);
                if (order != null)
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow?.MainFrame.Navigate(new ChatPage(order, bid));
                }
            }
        }

        private void CancelBid_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Bid bid = btn?.Tag as Bid;

            if (bid != null)
            {
                var result = MessageBox.Show("Отменить отклик на заказ?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    MainPage.Bids.Remove(bid);

                    var messagesToRemove = MainPage.Messages.Where(m => m.OrderId == bid.OrderId && m.FromEmail == MainPage.CurrentUser?.Email).ToList();
                    foreach (var msg in messagesToRemove)
                    {
                        MainPage.Messages.Remove(msg);
                    }

                    // СОХРАНЕНИЕ
                    FileHelper.SaveData(MainPage.Users, MainPage.Orders, MainPage.Bids, MainPage.Messages, MainPage.CurrentUser);

                    LoadBids();
                    MessageBox.Show("Отклик и чат удалены");
                }
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


    }
}