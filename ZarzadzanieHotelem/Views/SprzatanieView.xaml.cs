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
using ZarzadzanieHotelem.Models;

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
        }

        private void SprzatanieDGMenuAdd(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new SprzatanieAddView();
        }

        private void SprzatanieDGMenuMod(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new SprzatanieAddView(SprzatanieDG.SelectedItem as Cleaning);
        }

        private void SprzatanieDGMenuDel(object sender, RoutedEventArgs e)
        {

        }
    }
}
