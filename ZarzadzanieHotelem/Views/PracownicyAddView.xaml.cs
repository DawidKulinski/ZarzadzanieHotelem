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
    /// Logika interakcji dla klasy PracownicyAddView.xaml
    /// </summary>
    public partial class PracownicyAddView : UserControl
    {
        public PracownicyAddView()
        {
            InitializeComponent();

            PracownicyAddModBtn.Content = "Dodaj";
            IdStack.Visibility = Visibility.Collapsed;
        }

        public PracownicyAddView(Worker worker)
        {
            InitializeComponent();

            PracAddId.Text = worker.Id.ToString();
            PracAddName.Text = worker.Name.ToString();
            PracAddLName.Text = worker.LastName.ToString();
            PracAddPosition.SelectedItem = worker.Position.ToString();

            PracownicyAddModBtn.Content = "Modyfikuj";
            IdStack.Visibility = Visibility.Visible;
        }

        private void PracownicyAddModBtnClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(PracAddName.Text)
                & !String.IsNullOrWhiteSpace(PracAddLName.Text))
            {
                if (PracownicyAddModBtn.Content.ToString() == "Dodaj")
                {
                    try
                    {
                        if (PracAddPosition.SelectedItem == null)
                            throw new InvalidOperationException("Nie wybrano stanowiska");
                        WorkerController.Add(new Worker
                        {
                            Name = PracAddName.Text.Any(c => char.IsDigit(c)) ? throw new InvalidOperationException("Imie zawiera liczby") : PracAddName.Text,
                            LastName = PracAddLName.Text.Any(c => char.IsDigit(c)) ? throw new InvalidOperationException("Nazwisko zawiera liczby") : PracAddLName.Text,
                            Position = Enum.TryParse(PracAddPosition.SelectedItem.ToString(), out Position position) ? position : throw new InvalidOperationException("Nie mamy takiego stanowiska")
                        });
                    }
                    catch(InvalidOperationException er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        if (PracAddPosition.SelectedItem == null)
                            throw new InvalidOperationException("Nie wybrano stanowiska");
                        WorkerController.Edit(new Worker
                        {
                            Id = int.TryParse(PracAddId.Text, out int temp) ? temp : 1,
                            Name = PracAddName.Text.Any(c => char.IsDigit(c)) ? throw new InvalidOperationException("Imie zawiera liczby") : PracAddName.Text,
                            LastName = PracAddLName.Text.Any(c => char.IsDigit(c)) ? throw new InvalidOperationException("Nazwisko zawiera liczby") : PracAddLName.Text,
                            Position = Enum.TryParse(PracAddPosition.SelectedItem.ToString(), out Position position) ? position : throw new InvalidOperationException("Nie mamy takiego stanowiska")
                        });
                    }
                    catch (InvalidOperationException er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
                MessageBox.Show("Nie uzupełniono wszystkich potrzebnych pól.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.MainWindow.DataContext = new PracownicyView();
        }
    }
}
