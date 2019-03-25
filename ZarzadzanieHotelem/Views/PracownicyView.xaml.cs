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
    }
}
