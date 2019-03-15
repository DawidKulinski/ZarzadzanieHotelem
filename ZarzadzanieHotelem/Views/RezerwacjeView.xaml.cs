﻿using System;
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

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy Rezerwacje.xaml
    /// </summary>
    public partial class RezerwacjeView : UserControl
    {
        private ReservationCotroller _reservationController;

        public RezerwacjeView()
        {
            InitializeComponent();
            _reservationController = new ReservationCotroller();
            ReloadReservations();
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
    }
}
