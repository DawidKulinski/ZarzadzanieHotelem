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
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy PokojeView.xaml
    /// </summary>
    public partial class PokojeView : UserControl
    {
        public PokojeView()
        {
            InitializeComponent();

            var IdPracownika = new DataGridTextColumn();
            IdPracownika.Header = "Imię";
            IdPracownika.Binding = new Binding("Name");
            SprzatanieDG.Columns.Add(IdPracownika);

            var LNamePracownika = new DataGridTextColumn();
            LNamePracownika.Header = "Nazwisko";
            LNamePracownika.Binding = new Binding("LastName");
            SprzatanieDG.Columns.Add(LNamePracownika);

            var cleaning = new DataGridTextColumn();
            cleaning.Header = "Czas";
            cleaning.Binding = new Binding("CleanTime");
            SprzatanieDG.Columns.Add(cleaning);

            using (var context = new SqliteContext())
            {
                context.Rooms.ToList()
                    .ForEach(x=> PokojeDG.Items.Add(x));
            }

            PokojeDG.SelectionChanged += Row_SelectionChanged;
        }

        private void PokojeDGMenuAdd(object sender, RoutedEventArgs e) { Application.Current.MainWindow.DataContext = new PokojeAddView(); }

        private void PokojeDGMenuDel(object sender, RoutedEventArgs e)
        {
            if (PokojeDG.SelectedItem != null)
            {
                RoomController.Delete(PokojeDG.SelectedItem as Room);
                Application.Current.MainWindow.DataContext = new PokojeView();
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void PokojeDGMenuMod(object sender, RoutedEventArgs e)
        {
            if (PokojeDG.SelectedItem != null)
            {
                Application.Current.MainWindow.DataContext = new PokojeAddView(PokojeDG.SelectedItem as Room);
            }
            else
                MessageBox.Show("Nie wybrano elementu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Kalendarz.BlackoutDates.Clear();
            var RoomList = e.AddedItems;
            if (RoomList != null)
            {
                foreach (Room item in RoomList)
                {
                    var Reservations = RoomController.GetReservations(item.Id);
                    Reservations
                        .ForEach(
                            x=>Kalendarz
                                .BlackoutDates
                                .Add(new CalendarDateRange(x.Item1, x.Item2)));
                }
            }
            SprzatanieDG.Items.Clear();
            using (var context = new SqliteContext())
            {
                Room r = e.AddedItems[0] as Room;
                foreach (var x in context.Cleanings.Where(x => x.IdRoom == r.Id))
                    SprzatanieDG.Items.Add(new { x.Worker.Name, x.Worker.LastName, x.CleanTime });
            }
        }

        private void PokojeDG_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PokojeDG.SelectedItem != null)
            {
                Application.Current.MainWindow.DataContext = new PokojeAddView(PokojeDG.SelectedItem as Room);
            }
            else
                Application.Current.MainWindow.DataContext = new PokojeAddView();
        }
    }
}
