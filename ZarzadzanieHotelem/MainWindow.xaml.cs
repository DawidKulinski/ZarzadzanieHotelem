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

namespace ZarzadzanieHotelem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ReservationCotroller _reservationController;

        public MainWindow()
        {
            InitializeComponent();
            _reservationController = new ReservationCotroller();
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

        #region Rezerwacje
        //Rezerwacje
        private void ReloadReservations()
        {
            RezerwacjeDG.Items.Clear();

            foreach (var reservation in _reservationController.GetAll())
            {
                RezerwacjeDG.Items.Add(reservation);
            }
        }

        private void RezerwacjeMenuClick(object sender, RoutedEventArgs e)
        {
            Rezerwacje.Visibility = Visibility.Visible;
            RezerwacjeAdd.Visibility = Visibility.Collapsed;
            ReloadReservations();
        }

        private void RezerwacjeMenuAddClick(object sender, RoutedEventArgs e)
        {
            Rezerwacje.Visibility = Visibility.Collapsed;
            RezerwacjeAdd.Visibility = Visibility.Visible;
            DodajModyfikujButton.Content = "Dodaj";
            ClearWindow();
        }

        private void RezerwacjeDGMenuMod(object sender, RoutedEventArgs e)
        {
            Reservation selectedReservation = RezerwacjeDG.SelectedItem as Reservation;
            if (selectedReservation != null)
            {
                Rezerwacje.Visibility = Visibility.Collapsed;
                RezerwacjeAdd.Visibility = Visibility.Visible;
                DodajModyfikujButton.Content = "Modyfikuj";

                RezAddIDR.Text = selectedReservation.Id.ToString();
                RezAddIDC.Text = selectedReservation.IdCustomer.ToString();
                RezAddIDP.Text = selectedReservation.IdRoom.ToString();
                RezerwacjeStartDate.SelectedDate = selectedReservation.StartTime;
                RezerwacjeEndDate.SelectedDate = selectedReservation.StopTime;
            }
            else
                MessageBox.Show("Nie wybrano elementu");

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
                    MessageBox.Show(er.Message);
                }
            }
            else
                MessageBox.Show("Nie wybrano elementu");
        }

        private void RezerwacjeAddButton(object sender, RoutedEventArgs e)
        {
            if (IsReservationValid())
            {
                if (DodajModyfikujButton.Content.ToString() == "Dodaj")
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
                else if (DodajModyfikujButton.Content.ToString() == "Modyfikuj")
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
                ReloadReservations();
                Rezerwacje.Visibility = Visibility.Visible;
                RezerwacjeAdd.Visibility = Visibility.Collapsed;
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
        #endregion Rezerwacje
    }
}
