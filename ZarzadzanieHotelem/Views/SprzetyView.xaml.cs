using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZarzadzanieHotelem.Controller;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy SprzetyView.xaml
    /// </summary>
    public partial class SprzetyView : UserControl
    {
        public SprzetyView()
        {
            InitializeComponent();

            using (var context = new SqliteContext())
            {
                context.Equipments.ToList()
                    .ForEach(x => SprzetyDG.Items.Add(x));
            }
        }

        private void SprzetyDGMenuAdd(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new SprzetyAddView();
        }

        private void SprzetyDGMenuMod(object sender, RoutedEventArgs e)
        {

            if (SprzetyDG.SelectedItem != null)
            {
                Application.Current.MainWindow.DataContext = new SprzetyAddView(SprzetyDG.SelectedItem as Equipment);
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SprzetyDGMenuDel(object sender, RoutedEventArgs e)
        {
            if (SprzetyDG.SelectedItem != null)
            {
                var sprzet = SprzetyDG.SelectedItem as Equipment;



                EquipmentController.Delete(sprzet);
                Application.Current.MainWindow.DataContext = new SprzetyView();
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SprzetyDG_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SprzetyDG.SelectedItem != null)
                Application.Current.MainWindow.DataContext = new SprzetyAddView(SprzetyDG.SelectedItem as Equipment);
            else
                Application.Current.MainWindow.DataContext = new SprzetyAddView();
        }
    }
}
