using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZarzadzanieHotelem.Controller;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy Rezerwacje.xaml
    /// </summary>
    public partial class RezerwacjeView : UserControl
    {
        private ReservationCotroller _reservationController;

        private int CalculatePlaces()
        {
            using (var context = new SqliteContext())
            {
                var today = DateTime.Today;

                var reservationCount = context.Reservations
                    .Count(x =>
                        x.StartTime < today &&
                        today < x.StopTime);

                var reservations = (int)Math.Ceiling(reservationCount * 0.66);
                return reservations;
            }
        }

        private int CalculateDishes()
        {
            using (var context = new SqliteContext())
            {
                var today = DateTime.Today;

                var reservationCount = context.Reservations
                    .Count(x =>
                        x.StartTime < today &&
                        today < x.StopTime);
                return reservationCount;
            }
        }

        public RezerwacjeView()
        {
            InitializeComponent();
            _reservationController = new ReservationCotroller();
            ReloadReservations();

            EstimatedPlaces.Text = CalculatePlaces().ToString();
            EstimatedDishes.Text = CalculateDishes().ToString();
        }

        private void ReloadReservations()
        {
            RezerwacjeDG.Items.Clear();

            foreach (var reservation in _reservationController.GetAll())
            {
                RezerwacjeDG.Items.Add(reservation);
            }
        }

        private void RezerwacjeDGMenuAdd(object sender, RoutedEventArgs e) { Application.Current.MainWindow.DataContext = new RezerwacjeAddView(); }

        private void RezerwacjeDGMenuMod(object sender, RoutedEventArgs e)
        {
            Reservation selectedReservation = RezerwacjeDG.SelectedItem as Reservation;
            if (selectedReservation != null)
            {
                Application.Current.MainWindow.DataContext = new RezerwacjeAddView(RezerwacjeDG.SelectedItem as Reservation);
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

        }
        private void RezerwacjeDGMenuDel(object sender, RoutedEventArgs e)
        {
            if (RezerwacjeDG.SelectedItem != null)
            {
                try
                {
                    _reservationController.Delete((Reservation)RezerwacjeDG.SelectedItem);
                    ReloadReservations();
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RezerwacjeDG_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RezerwacjeDG.SelectedItem != null)
                Application.Current.MainWindow.DataContext = new RezerwacjeAddView(RezerwacjeDG.SelectedItem as Reservation);
            else
                Application.Current.MainWindow.DataContext = new RezerwacjeAddView();
        }
    }
}
