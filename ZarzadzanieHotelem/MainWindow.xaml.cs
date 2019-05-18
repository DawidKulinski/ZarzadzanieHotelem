using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZarzadzanieHotelem.Views;

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

        private void PokojMenuClick(object sender, RoutedEventArgs e)
        {
            DataContext = new PokojeView();
        }

        private void PokojMenuAddClick(object sender, RoutedEventArgs e)
        {
            DataContext = new PokojeAddView();
        }

        private void RezerwacjeMenuClick(object sender, RoutedEventArgs e)
        {
            DataContext = new RezerwacjeView();
        }

        private void RezerwacjeMenuAddClick(object sender, RoutedEventArgs e)
        {
            DataContext = new RezerwacjeAddView();
        }

        private void SprzatanieMenuClick(object sender, MouseButtonEventArgs e)
        {
            DataContext = new SprzatanieView();
        }

        private void SprzatanieMenuAddClick(object sender, MouseButtonEventArgs e)
        {
            DataContext = new SprzatanieAddView();
        }

        private void PracownicyMenuClick(object sender, MouseButtonEventArgs e)
        {
            DataContext = new PracownicyView();
        }

        private void PracownicyMenuAddClick(object sender, MouseButtonEventArgs e)
        {
            DataContext = new PracownicyAddView();
        }
        private void KlienciMenuClick(object sender, MouseButtonEventArgs e)
        {
            DataContext = new KlientView();
        }

        private void KlienciMenuAddClick(object sender, MouseButtonEventArgs e)
        {
            DataContext = new KlientAddView();
        }

        private void SprzetyMenuClick(object sender, MouseButtonEventArgs e)
        {
            DataContext = new SprzetyView();
        }

        private void SprzetyAddMenuClick(object sender, MouseButtonEventArgs e)
        {
            DataContext = new SprzetyAddView();
        }
    }
}
