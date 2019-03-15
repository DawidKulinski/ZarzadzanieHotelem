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

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy PokojeView.xaml
    /// </summary>
    public partial class PokojeView : UserControl
    {
        public PokojeView()
        {
            InitializeComponent();
        }

        private void PokojeDGMenuAdd(object sender, RoutedEventArgs e) { Application.Current.MainWindow.DataContext = new PokojeAddView(); }

        private void PokojeDGMenuDel(object sender, RoutedEventArgs e)
        {
            if (PokojeDG.SelectedItem != null)
            {
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void PokojeDGMenuMod(object sender, RoutedEventArgs e)
        {
            if (PokojeDG.SelectedItem != null)
            {
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
