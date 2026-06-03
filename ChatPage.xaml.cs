using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kyrsovai
{
    public partial class ChatPage : Page
    {
        private Order _currentOrder;
        private Bid _currentBid;
        private ObservableCollection<ChatMessage> _messages;

        public ChatPage(Order order, Bid bid)
        {
            InitializeComponent();
            _currentOrder = order;
            _currentBid = bid;

            GuestText.Text = MainPage.UserName;
            UpdateAuthButtons();

            ChatTitle.Text = $"Чат по заказу: {order.OrderTitle}";
            LoadMessages();
        }

        private void LoadMessages()
        {
            var messages = MainPage.Messages
                .Where(m => m.OrderId == _currentOrder.OrderId)
                .OrderBy(m => m.Date)
                .Select(m => new ChatMessage
                {
                    From = m.From,
                    Text = m.Text,
                    Date = m.Date,
                    IsMine = m.FromEmail == MainPage.CurrentUser?.Email
                }).ToList();

            _messages = new ObservableCollection<ChatMessage>(messages);
            MessagesList.ItemsSource = _messages;

            if (MessagesList.Items.Count > 0)
                MessagesList.ScrollIntoView(MessagesList.Items[MessagesList.Items.Count - 1]);
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string text = ChatMessageBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Введите сообщение");
                return;
            }

            string toName = MainPage.CurrentUser?.Email == _currentBid.FreelancerEmail
                ? _currentOrder.Author
                : _currentBid.FreelancerName;
            string toEmail = MainPage.CurrentUser?.Email == _currentBid.FreelancerEmail
                ? _currentOrder.AuthorEmail
                : _currentBid.FreelancerEmail;

            Message newMessage = new Message
            {
                Id = MainPage.Messages.Count + 1,
                OrderId = _currentOrder.OrderId,
                OrderTitle = _currentOrder.OrderTitle,
                From = MainPage.CurrentUser?.UserName ?? "Гость",
                FromEmail = MainPage.CurrentUser?.Email ?? "",
                To = toName,
                ToEmail = toEmail,
                Text = text,
                Date = DateTime.Now
            };

            MainPage.Messages.Add(newMessage);

            // СОХРАНЕНИЕ
            FileHelper.SaveData(MainPage.Users, MainPage.Orders, MainPage.Bids, MainPage.Messages, MainPage.CurrentUser);

            _messages.Add(new ChatMessage
            {
                From = newMessage.From,
                Text = text,
                Date = DateTime.Now,
                IsMine = true
            });

            ChatMessageBox.Clear();

            if (MessagesList.Items.Count > 0)
                MessagesList.ScrollIntoView(MessagesList.Items[MessagesList.Items.Count - 1]);
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

    public class ChatMessage
    {
        public string From { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsMine { get; set; }
    }
}