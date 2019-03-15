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

namespace ZarzadzanieHotelem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Collapse(object sender, RoutedEventArgs e)
        {
            this.Menu.Visibility = this.Menu.Visibility == Visibility.Visible
                                            ? Visibility.Collapsed
                                            : Visibility.Visible;
            this.ExapndCollapse.Content = this.Menu.Visibility == Visibility.Visible
                                            ? this.ExapndCollapse.Content = "<"
                                            : this.ExapndCollapse.Content = ">";
        }

        //Pokoje
        private void PokojMenuCollapse(object sender, RoutedEventArgs e)
        {
            this.PokojMenu.Visibility = this.PokojMenu.Visibility == Visibility.Visible
                                            ? Visibility.Collapsed
                                            : Visibility.Visible;
            this.Pokoje.Visibility = this.Pokoje.Visibility == Visibility.Visible
                                            ? Visibility.Collapsed
                                            : Visibility.Visible;
        }

        private void PokojeDGMenuMod(object sender, RoutedEventArgs e)
        {
        }

        private void PokojeDGMenuDel(object sender, RoutedEventArgs e)
        {
        }

        //Rezerwacje 
        private void RezerwacjeMenuClick(object sender, RoutedEventArgs e)
        {
            Rezerwacje.Visibility = Visibility.Visible;
            RezerwacjeAdd.Visibility = Visibility.Collapsed;
        }

        private void RezerwacjeMenuAddClick(object sender, RoutedEventArgs e)
        {
            Rezerwacje.Visibility = Visibility.Collapsed;
            RezerwacjeAdd.Visibility = Visibility.Visible;
        }

        private void RezerwacjeDGMenuMod(object sender, RoutedEventArgs e)
        {
        }

        private void RezerwacjeDGMenuDel(object sender, RoutedEventArgs e)
        {
        }

        private void RezerwacjeAddButton(object sender, RoutedEventArgs e)
        {
        }
    }
}
