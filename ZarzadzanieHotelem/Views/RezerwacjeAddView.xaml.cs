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
using ZarzadzanieHotelem;

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy RezerwacjeAdd.xaml
    /// </summary>
    public partial class RezerwacjeAddView : UserControl
    {
        private ReservationCotroller _reservationController;

        public RezerwacjeAddView()
        {
            InitializeComponent();
            _reservationController = new ReservationCotroller();

            RezerwacjeAddModButton.Content = "Dodaj";
        }

        public RezerwacjeAddView(Reservation reservation)
        {
            InitializeComponent();
            _reservationController = new ReservationCotroller();

            RezAddIDR.Text = reservation.Id.ToString();
            RezAddIDC.Text = reservation.IdCustomer.ToString();
            RezAddIDR.Text = reservation.IdRoom.ToString();
            RezerwacjeStartDate.SelectedDate = reservation.StartTime;
            RezerwacjeEndDate.SelectedDate = reservation.StopTime;
            RezerwacjeAddModButton.Content = "Modyfikuj";
        }

        private void RezerwacjeAddButton(object sender, RoutedEventArgs e)
        {
            if (IsReservationValid())
            {
                if (RezerwacjeAddModButton.Content.ToString() == "Dodaj")
                {
                    int temp;
                    try
                    {
                        _reservationController.Add(new Reservation
                        {
                            IdCustomer = int.TryParse(RezAddIDC.Text, out temp) ? temp : 1,
                            IdRoom = int.TryParse(RezAddIDP.Text, out temp) ? temp : 1,
                            StartTime = RezerwacjeStartDate.SelectedDate.Value,
                            StopTime = RezerwacjeEndDate.SelectedDate.Value
                        });
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message);
                    }
                }
                else if (RezerwacjeAddModButton.Content.ToString() == "Modyfikuj")
                {
                    int temp;
                    try
                    {
                        _reservationController.Edit(new Reservation
                        {
                            Id = int.TryParse(RezAddIDR.Text, out temp) ? temp : 1,
                            IdCustomer = int.TryParse(RezAddIDC.Text, out temp) ? temp : 1,
                            IdRoom = int.TryParse(RezAddIDP.Text, out temp) ? temp : 1,
                            StartTime = RezerwacjeStartDate.SelectedDate.Value,
                            StopTime = RezerwacjeEndDate.SelectedDate.Value
                        });
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message);
                    }
                }
                DataContext = new RezerwacjeView();
            }
            else
                MessageBox.Show("Nie uzupełniono wszystkich pól.");
        }

        private bool IsReservationValid()
        {
            if (String.IsNullOrWhiteSpace(RezAddIDC.Text)
                || String.IsNullOrWhiteSpace(RezAddIDP.Text)
                || !RezerwacjeStartDate.SelectedDate.HasValue
                || !RezerwacjeEndDate.SelectedDate.HasValue)
                return false;
            return true;
        }

        private void ClearWindow()
        {
            RezAddIDR.Text = null;
            RezAddIDC.Text = null;
            RezAddIDP.Text = null;
            RezerwacjeStartDate.SelectedDate = null;
            RezerwacjeEndDate.SelectedDate = null;
        }
    }
}
