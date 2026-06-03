using System;
using System.Windows;
using System.Windows.Controls;

namespace Kyrsovai
{
    public partial class EditOrderPage : Page
    {
        private Order _order;

        public EditOrderPage(Order order)
        {
            InitializeComponent();
            _order = order;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TitleBox.Text = _order.OrderTitle;
            DescriptionBox.Text = _order.Description;
            DeadlineBox.SelectedDate = _order.Deadline;
            PriceBox.Text = _order.Price.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text.Trim();
            string description = DescriptionBox.Text.Trim();
            DateTime? deadline = DeadlineBox.SelectedDate;
            string priceText = PriceBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Введите название");
                return;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Введите описание");
                return;
            }
            if (deadline == null)
            {
                MessageBox.Show("Выберите дату");
                return;
            }
            if (!int.TryParse(priceText, out int price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену");
                return;
            }

            _order.OrderTitle = title;
            _order.Description = description;
            _order.Deadline = deadline.Value;
            _order.Price = price;

            FileHelper.SaveData(MainPage.Users, MainPage.Orders, MainPage.Bids, MainPage.Messages, MainPage.CurrentUser);

            MessageBox.Show("Заказ обновлён!");

            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new OrderDetailsPage(_order));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new OrderDetailsPage(_order));
        }
    }
}
