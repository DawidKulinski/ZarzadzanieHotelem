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
    /// Logika interakcji dla klasy PracownicyView.xaml
    /// </summary>
    public partial class PracownicyView : UserControl
    {
        public PracownicyView()
        {
            InitializeComponent();
        }
        private void PracownicyDGMenuAdd(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new PracownicyAddView();
        }

        private void PracownicyDGMenuMod(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new PracownicyAddView(PracownicyDG.SelectedItem as Worker);
        }

        private void PracownicyDGMenuDel(object sender, RoutedEventArgs e)
        {

        }
    }
}
