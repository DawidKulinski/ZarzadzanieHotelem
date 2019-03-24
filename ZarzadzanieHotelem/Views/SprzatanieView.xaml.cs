using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZarzadzanieHotelem.Controller;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy SprzatanieView.xaml
    /// </summary>
    public partial class SprzatanieView : UserControl
    {
        public SprzatanieView()
        {
            InitializeComponent();

            using (var context = new SqliteContext())
            {
                context.Cleanings.ToList()
                    .ForEach(x => SprzatanieDG.Items.Add(x));
            }
        }

        private void SprzatanieDGMenuAdd(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new SprzatanieAddView();
        }

        private void SprzatanieDGMenuMod(object sender, RoutedEventArgs e)
        {

            if (SprzatanieDG.SelectedItem != null)
            {
                Application.Current.MainWindow.DataContext = new SprzatanieAddView(SprzatanieDG.SelectedItem as Cleaning);
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SprzatanieDGMenuDel(object sender, RoutedEventArgs e)
        {
            if (SprzatanieDG.SelectedItem != null)
            {
                CleaningController.Delete(SprzatanieDG.SelectedItem as Cleaning);
                Application.Current.MainWindow.DataContext = new SprzatanieView();
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
