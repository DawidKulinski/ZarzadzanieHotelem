using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for ParkingAddView.xaml
    /// </summary>
    public partial class ParkingAddView : UserControl
    {
        public ParkingAddView()
        {
            InitializeComponent();
        }

        public ParkingAddView(ParkingSlot parkingSlot)
        {
            InitializeComponent();

            ParkingAddId.Text = parkingSlot.Id.ToString();
            ParkingAddOznaczenie.Text = parkingSlot.SlotCode.ToString();
            ParkingAddZajete.IsChecked = parkingSlot.Occupied;

            ParkingAddModBtn.Content = "Modyfikuj";
            IdStack.Visibility = Visibility.Visible;
        }

        private void ParkingAddModBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ParkingAddModBtn.Content.ToString() == "Dodaj")
            {
                try
                {
                    using (var context = new SqliteContext())
                    {

                        ParkingController.Add(new ParkingSlot()
                        {
                            Id = int.TryParse(ParkingAddId.Text, out int temp) ? temp : 1,
                            SlotCode = ParkingAddOznaczenie.Text,
                            Occupied = ParkingAddZajete.IsChecked.Value
                        });
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    int.TryParse(ParkingAddId.Text, out var id);
                    ParkingController.Edit(new ParkingSlot()
                    {
                        Id = int.TryParse(ParkingAddId.Text, out int temp) ? temp : throw new Exception(),
                        SlotCode = ParkingAddOznaczenie.Text,
                        Occupied = ParkingAddZajete.IsChecked.Value
                    });
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Application.Current.MainWindow.DataContext = new ParkingView();
        }
    }
}
