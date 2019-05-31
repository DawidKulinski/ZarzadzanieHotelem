using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZarzadzanieHotelem.Controller;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ParkingView.xaml
    /// </summary>
    public partial class ParkingView : UserControl
    {
        public ParkingView()
        {
            InitializeComponent();

            using (var context = new SqliteContext())
            {
                context.ParkingSlots.ToList()
                    .ForEach(x => ParkingDG.Items.Add(x));
            }
        }

        private void ParkingShowFree_Checked(object sender, RoutedEventArgs e)
        {
            ParkingDG.Items.Clear();
            using (var context = new SqliteContext())
            {
                context.ParkingSlots.Where(x => x.Occupied == false).ToList()
                    .ForEach(x => ParkingDG.Items.Add(x));
            }
        }

        private void ParkingShowFree_Unchecked(object sender, RoutedEventArgs e)
        {
            ParkingDG.Items.Clear();
            using (var context = new SqliteContext())
            {
                context.ParkingSlots.ToList()
                    .ForEach(x => ParkingDG.Items.Add(x));
            }

        }

        private void ParkingDGMenuAdd(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new ParkingAddView();
        }

        private void ParkingDGMenuMod(object sender, RoutedEventArgs e)
        {

            if (ParkingDG.SelectedItem != null)
            {
                Application.Current.MainWindow.DataContext = new ParkingAddView(ParkingDG.SelectedItem as ParkingSlot);
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ParkingDGMenuDel(object sender, RoutedEventArgs e)
        {
            if (ParkingDG.SelectedItem != null)
            {
                var parkingSlot = ParkingDG.SelectedItem as ParkingSlot;



                ParkingController.Delete(parkingSlot);
                Application.Current.MainWindow.DataContext = new ParkingView();
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ParkingDG_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ParkingDG.SelectedItem != null)
                Application.Current.MainWindow.DataContext = new ParkingAddView(ParkingDG.SelectedItem as ParkingSlot);
            else
                Application.Current.MainWindow.DataContext = new ParkingAddView();
        }
    }
}
