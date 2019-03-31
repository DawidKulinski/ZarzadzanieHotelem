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
    /// Logika interakcji dla klasy PracownicyView.xaml
    /// </summary>
    public partial class PracownicyView : UserControl
    {
        public PracownicyView()
        {
            InitializeComponent();

            var IdPokoju = new DataGridTextColumn();
            IdPokoju.Header = "Numer Pokoju";
            IdPokoju.Binding = new Binding("RoomNumber");
            SprzatanieDG.Columns.Add(IdPokoju);

            var cleaning = new DataGridTextColumn();
            cleaning.Header = "Czas";
            cleaning.Binding = new Binding("CleanTime");
            SprzatanieDG.Columns.Add(cleaning);

            using (var context = new SqliteContext())
            {
                context.Workers.ToList()
                    .ForEach(x => PracownicyDG.Items.Add(x));
            }

        }
        private void PracownicyDGMenuAdd(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new PracownicyAddView();
        }

        private void PracownicyDGMenuMod(object sender, RoutedEventArgs e)
        {
            if (PracownicyDG.SelectedItem != null)
                Application.Current.MainWindow.DataContext = new PracownicyAddView(PracownicyDG.SelectedItem as Worker);
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void PracownicyDGMenuDel(object sender, RoutedEventArgs e)
        {
            if (PracownicyDG.SelectedItem != null)
            {
                WorkerController.Delete(PracownicyDG.SelectedItem as Worker);
                Application.Current.MainWindow.DataContext = new PracownicyView();
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void SpratanieDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SprzatanieDG.Items.Clear();
            using (var context = new SqliteContext())
            {
                Worker worker = e.AddedItems[0] as Worker;
                foreach (var x in context.Cleanings.Where(x => x.IdWorker == worker.Id))
                    SprzatanieDG.Items.Add(new { x.Room.RoomNumber, x.CleanTime});
            }
        }

        private void PracownicyDG_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PracownicyDG.SelectedItem != null)
                Application.Current.MainWindow.DataContext = new PracownicyAddView(PracownicyDG.SelectedItem as Worker);
            else
                Application.Current.MainWindow.DataContext = new PracownicyAddView();
        }
    }
}
