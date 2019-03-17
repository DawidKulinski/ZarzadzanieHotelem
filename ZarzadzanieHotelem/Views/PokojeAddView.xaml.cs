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

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy PokojeAddView.xaml
    /// </summary>
    public partial class PokojeAddView : UserControl
    {
        public PokojeAddView()
        {
            InitializeComponent();

            PokojeAddModButton.Content = "Dodaj";
        }

        public PokojeAddView(Room room)
        {
            InitializeComponent();

            RoomAddIDP.Text = room.Id.ToString();
            RoomAddRoomNumber.Text = room.RoomNumber.ToString();
            RoomAddRoomStandard.Text = room.RoomStandard.ToString();

            PokojeAddModButton.Content = "Modyfikuj";
        }

        private void PokojeAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                if(PokojeAddModButton.Content.ToString() == "Dodaj")
                {
                    try
                    {
                        RoomController.Add(new Room
                        {
                            Id = int.TryParse(RoomAddIDP.Text, out int temp) ? temp : 1,
                            RoomNumber = int.TryParse(RoomAddRoomNumber.Text, out temp) ? temp : 1,
                            RoomStandard = int.TryParse(RoomAddRoomStandard.Text, out temp) ? temp : 1
                        });
                    }
                    catch(Exception er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    try
                    {
                        RoomController.Edit(new Room
                        {
                            Id = int.TryParse(RoomAddIDP.Text, out int temp) ? temp : throw new Exception(),
                            RoomNumber = int.TryParse(RoomAddRoomNumber.Text, out temp) ? temp : throw new Exception(),
                            RoomStandard = int.TryParse(RoomAddRoomStandard.Text, out temp) ? temp : throw new Exception()
                        });
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                Application.Current.MainWindow.DataContext = new PokojeView();
            }
            else
                MessageBox.Show("Nie uzupełniono wszystkich pól.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool IsValid()
        {
            if (!String.IsNullOrWhiteSpace(RoomAddIDP.Text)
                & !String.IsNullOrWhiteSpace(RoomAddRoomNumber.Text)
                & !String.IsNullOrWhiteSpace(RoomAddRoomStandard.Text))
                return true;
            else return false;
        }
    }
}
