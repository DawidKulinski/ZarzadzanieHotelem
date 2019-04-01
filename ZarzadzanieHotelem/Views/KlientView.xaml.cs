//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//namespace ZarzadzanieHotelem.Views
//{
//    /// <summary>
//    /// Interaction logic for KlientView.xaml
//    /// </summary>
//    public partial class KlientView : Page
//    {
//        public KlientView()
//        {
//            InitializeComponent();
//        }
//    }
//}


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
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Interaction logic for KlientView.xaml
    /// </summary>
    public partial class KlientView : UserControl
    {
        public KlientView()
        {
            InitializeComponent();

            var NrPokoju = new DataGridTextColumn();
            NrPokoju.Header = "Numer Pokoju";
            NrPokoju.Binding = new Binding("RoomNumber");
            RezerwacjeDG.Columns.Add(NrPokoju);

            var rezerwStart = new DataGridTextColumn();
            rezerwStart.Header = "Przyjazd";
            rezerwStart.Binding = new Binding("StartTime");
            RezerwacjeDG.Columns.Add(rezerwStart);

            var rezerwEnd = new DataGridTextColumn();
            rezerwEnd.Header = "Czas";
            rezerwEnd.Binding = new Binding("StopTime");
            RezerwacjeDG.Columns.Add(rezerwEnd);

            using (var context = new SqliteContext())
            {
                context.Customers.ToList()
                    .ForEach(x => KlientDG.Items.Add(x));
            }

        }
        private void KlientDGMenuAdd(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new KlientAddView();
        }

        private void KlientDGMenuMod(object sender, RoutedEventArgs e)
        {
            if (KlientDG.SelectedItem != null)
                Application.Current.MainWindow.DataContext = new KlientAddView(KlientDG.SelectedItem as Customer);
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void KlientDGMenuDel(object sender, RoutedEventArgs e)
        {
            if (KlientDG.SelectedItem != null)
            {
                CustomerController.Delete(KlientDG.SelectedItem as Customer);
                Application.Current.MainWindow.DataContext = new KlientView();
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void KlientDG_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(KlientDG.SelectedItem != null)
            {
                Application.Current.MainWindow.DataContext = new KlientAddView(KlientDG.SelectedItem as Customer);
            }
            else
            {
                Application.Current.MainWindow.DataContext = new KlientAddView();
            }
        }

        private void KlientDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RezerwacjeDG.Items.Clear();
            using (var context = new SqliteContext())
            {
                Customer customer = e.AddedItems[0] as Customer;
                foreach (var x in context.Reservations.Where(x => x.IdCustomer == customer.Id))
                    RezerwacjeDG.Items.Add(new { x.Room.RoomNumber, x.StartTime, x.StopTime });
            }
        }
    }
}
