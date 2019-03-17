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
    /// Logika interakcji dla klasy SprzatanieAddView.xaml
    /// </summary>
    public partial class SprzatanieAddView : UserControl
    {
        public SprzatanieAddView()
        {
            InitializeComponent();
        }

        public SprzatanieAddView(Cleaning cleaning)
        {
            InitializeComponent();

            SprzAddId.Text = cleaning.Id.ToString();
            SprzAddIdPokoju.Text = cleaning.IdRoom.ToString();
            SprzAddIdPracownika.Text = cleaning.IdWorker.ToString();
            SprzStartDate.SelectedDate = cleaning.CleanTime;
        }

        private void SprzatanieAddModBtnClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(SprzAddIdPokoju.Text)
                & !String.IsNullOrWhiteSpace(SprzAddIdPracownika.Text))
            {

            }
            else
                MessageBox.Show("Nie uzupełniono wszystkich pól.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
