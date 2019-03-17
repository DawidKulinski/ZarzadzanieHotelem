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
    /// Logika interakcji dla klasy PracownicyAddView.xaml
    /// </summary>
    public partial class PracownicyAddView : UserControl
    {
        public PracownicyAddView()
        {
            InitializeComponent();
        }

        public PracownicyAddView(Worker worker)
        {
            InitializeComponent();

            PracAddId.Text = worker.Id.ToString();
            PracAddName.Text = worker.Name.ToString();
            PracAddLName.Text = worker.LastName.ToString();
        }

        private void PracownicyAddModBtnClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(PracAddName.Text)
                & !String.IsNullOrWhiteSpace(PracAddLName.Text))
            {

            }
            else
                MessageBox.Show("Nie uzupełniono wszystkich pól.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.MainWindow.DataContext = new PracownicyView();
        }

        private void PracatanieAddModBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
