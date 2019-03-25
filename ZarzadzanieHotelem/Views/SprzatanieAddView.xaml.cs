using System;
using System.Windows;
using System.Windows.Controls;
using ZarzadzanieHotelem.Controller;
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

            SprzatanieAddModBtn.Content = "Dodaj";
            IdStack.Visibility = Visibility.Collapsed;
        }
        
        public SprzatanieAddView(Cleaning cleaning)
        {
            InitializeComponent();

            SprzAddId.Text = cleaning.Id.ToString();
            SprzAddIdPokoju.Text = cleaning.IdRoom.ToString();
            SprzAddIdPracownika.Text = cleaning.IdWorker.ToString();
            SprzStartDate.SelectedDate = cleaning.CleanTime;

            SprzatanieAddModBtn.Content = "Modyfikuj";
            IdStack.Visibility = Visibility.Visible;
        }

        private void SprzatanieAddModBtnClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(SprzAddIdPokoju.Text)
                && SprzStartDate.SelectedDate != null)
            {
                if (SprzatanieAddModBtn.Content.ToString() == "Dodaj")
                {
                    try
                    {
                        CleaningController.Add(new Cleaning
                        {
                            IdRoom = int.TryParse(SprzAddIdPokoju.Text, out int temp) ? temp : 1,
                            IdWorker = int.TryParse(SprzAddIdPracownika.Text, out temp) ? temp : -1,
                            CleanTime = SprzStartDate.SelectedDate.Value
                        });
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
                        CleaningController.Edit(new Cleaning
                        {
                            Id = int.TryParse(SprzAddId.Text, out int temp) ? temp : throw new Exception(),
                            IdRoom = int.TryParse(SprzAddIdPokoju.Text, out temp) ? temp : throw new Exception(),
                            IdWorker = int.TryParse(SprzAddIdPracownika.Text, out temp) ? temp : throw new Exception(),
                            CleanTime = SprzStartDate.SelectedDate.Value
                        });
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                Application.Current.MainWindow.DataContext = new SprzatanieView();
            }
            else
                MessageBox.Show("Nie uzupełniono wszystkich pól.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
