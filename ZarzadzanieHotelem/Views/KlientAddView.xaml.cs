using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZarzadzanieHotelem.Controller;
using ZarzadzanieHotelem.Models;

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy KlientAddView.xaml
    /// </summary>
    public partial class KlientAddView : UserControl
    {
        public KlientAddView()
        {
            InitializeComponent();

            KlienciAddModBtn.Content = "Dodaj";
            IdStack.Visibility = Visibility.Collapsed;
        }

        public KlientAddView(Customer customer)
        {
            InitializeComponent();

            ClientId.Text = customer.Id.ToString();
            ClientName.Text = customer.Name.ToString();
            ClientLName.Text = customer.LastName.ToString();

            KlienciAddModBtn.Content = "Modyfikuj";
            IdStack.Visibility = Visibility.Visible;
        }

        private void ClientAddBtnClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ClientName.Text)
                & !String.IsNullOrWhiteSpace(ClientLName.Text))
            {
                if (KlienciAddModBtn.Content.ToString() == "Dodaj")
                {
                    try
                    {
                        CustomerController.Add(new Customer
                        {
                            Name = ClientName.Text.Any(c => char.IsDigit(c)) ? throw new InvalidOperationException("Imie zawiera liczby") : ClientName.Text,
                            LastName = ClientLName.Text.Any(c => char.IsDigit(c)) ? throw new InvalidOperationException("Nazwisko zawiera liczby") : ClientLName.Text,
                        });
                    }
                    catch (InvalidOperationException er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    try
                    {
                        CustomerController.Edit(new Customer
                        {
                            Id = int.TryParse(ClientId.Text, out int temp) ? temp : 1,
                            Name = ClientName.Text.Any(c => char.IsDigit(c)) ? throw new InvalidOperationException("Imie zawiera liczby") : ClientName.Text,
                            LastName = ClientLName.Text.Any(c => char.IsDigit(c)) ? throw new InvalidOperationException("Nazwisko zawiera liczby") : ClientLName.Text,
                        });
                    }
                    catch (InvalidOperationException er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
                MessageBox.Show("Nie uzupełniono wszystkich potrzebnych pól.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.MainWindow.DataContext = new KlientView();
        }
    }
}
