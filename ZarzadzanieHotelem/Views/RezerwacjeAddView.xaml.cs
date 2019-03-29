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
using ZarzadzanieHotelem.Utils;

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
            IdStack.Visibility = Visibility.Collapsed;
            PriceStack.Visibility = Visibility.Collapsed;
        }

        public RezerwacjeAddView(Reservation reservation)
        {
            InitializeComponent();
            _reservationController = new ReservationCotroller();

            RezAddIDR.Text = reservation.Id.ToString();
            RezAddIDC.Text = reservation.IdCustomer.ToString();
            RezAddIDP.Text = reservation.IdRoom.ToString();
            RezerwacjeStartDate.SelectedDate = reservation.StartTime;
            RezerwacjeEndDate.SelectedDate = reservation.StopTime;
            RezEditPrice.Text = reservation.Price.ToString();

            RezerwacjeAddModButton.Content = "Modyfikuj";
            IdStack.Visibility = Visibility.Visible;
            PriceStack.Visibility = Visibility.Visible;
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
                        if (RezerwacjeStartDate.SelectedDate.Value > RezerwacjeEndDate.SelectedDate.Value)
                        {
                            MessageBox.Show("Data zakończenia pobytu nie może być mniejsza niż rozpoczęcie pobytu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            _reservationController.Add(new Reservation
                            {
                                IdCustomer = int.TryParse(RezAddIDC.Text, out temp) ? temp : 1,
                                IdRoom = int.TryParse(RezAddIDP.Text, out temp) ? temp : 1,
                                StartTime = RezerwacjeStartDate.SelectedDate.Value,
                                StopTime = RezerwacjeEndDate.SelectedDate.Value,
                                Price = decimal.TryParse(RezEditPrice.Text, out decimal temp1) ? temp1 : 0.0m
                            });
                        }
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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
                            StopTime = RezerwacjeEndDate.SelectedDate.Value,
                            Price = decimal.TryParse(RezEditPrice.Text, out decimal temp1) ? temp1 : 1,
                        });
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                Application.Current.MainWindow.DataContext = new RezerwacjeView();
            }
            else
                MessageBox.Show("Nie uzupełniono wszystkich pól.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void dpick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? startTime = RezerwacjeStartDate.SelectedDate;
            DateTime? stopTime = RezerwacjeEndDate.SelectedDate;

            if(startTime != null && stopTime != null)
            {
                using (var context = new SqliteContext())
                {
                    try
                    {
                        int roomId = int.Parse(RezAddIDP.Text);
                        decimal roomPrice = context.Rooms.First(x => x.Id == roomId).Price;

                        int roomCount = context.Rooms.Count();
                        int reservedRooms = context.Reservations.Where(x =>
                            (x.StartTime < startTime && x.StopTime > stopTime) || (x.StartTime < stopTime && x.StopTime > stopTime))
                                .Select(x => x.IdRoom)
                                .Distinct()
                                .Count();

                        decimal roomUsage = (decimal)reservedRooms / (roomCount == 0 ? 1 : roomCount); //preventing to divide by 0.

                        if (roomUsage < 0.5m)
                        {
                            RezEditPrice.Text = (roomPrice * 0.8m).ToString();
                        }
                        else if (roomUsage < 0.9m && roomUsage >= 0.5m)
                        {
                            RezEditPrice.Text = roomPrice.ToString();
                        }
                        else
                        {
                            RezEditPrice.Text = (roomPrice * 1.2m).ToString();
                        }
                    }
                    catch(InvalidOperationException)
                    {
                        MessageBox.Show($"Nie ma pokoju o ID {RezAddIDP.Text}");
                    }
                    catch(FormatException)
                    {
                        MessageBox.Show($"Nie podano identyfikatora pokoju.");
                    }
                }
            }
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
    }
}
