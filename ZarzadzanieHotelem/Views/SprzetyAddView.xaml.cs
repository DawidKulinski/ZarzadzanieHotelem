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
    /// Interaction logic for SprzetyAddView.xaml
    /// </summary>
    public partial class SprzetyAddView : UserControl
    {
        public SprzetyAddView()
        {
            InitializeComponent();
        }

        public SprzetyAddView(Equipment equipment)
        {
            InitializeComponent();

            SprzetyAddId.Text = equipment.Id.ToString();
            SprzetyAddNazwa.Text = equipment.Name.ToString();
            SprzetyAddIlosc.Text = equipment.Count.ToString();

            SprzetyAddModBtn.Content = "Modyfikuj";
            IdStack.Visibility = Visibility.Visible;
        }

        private void SprzetyAddModBtn_Click(object sender, RoutedEventArgs e)
        {
                if (SprzetyAddModBtn.Content.ToString() == "Dodaj")
                {
                    try
                    {
                    using (var context = new SqliteContext())
                    {
                        if (context.Equipments.Any(x => x.Name == SprzetyAddNazwa.Text))
                        {
                            var equipment = context.Equipments.FirstOrDefault(x => x.Name == SprzetyAddNazwa.Text);
                            if (equipment != null)
                                EquipmentController.Edit(new Equipment()
                                {
                                    Id = equipment.Id,
                                    Name = equipment.Name,
                                    Count = int.Parse(SprzetyAddIlosc.Text) + equipment.Count
                                });
                        }
                        else
                        {
                            EquipmentController.Add(new Equipment()
                            {
                                Id = int.TryParse(SprzetyAddId.Text, out int temp) ? temp : 1,
                                Name = SprzetyAddNazwa.Text,
                                Count = int.TryParse(SprzetyAddIlosc.Text, out int temp1) ? temp1 : -1
                            });
                        }

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
                        int.TryParse(SprzetyAddIlosc.Text, out int number);
                        int.TryParse(SprzetyAddId.Text, out var id);
                        if (number != 0)
                        {
                            EquipmentController.Edit(new Equipment()
                            {
                                Id = int.TryParse(SprzetyAddId.Text, out int temp) ? temp : throw new Exception(),
                                Name = SprzetyAddNazwa.Text,
                                Count = number
                            });
                        }
                        else
                        {
                            using (var context = new SqliteContext())
                            {
                                var toDelete = context.Equipments.FirstOrDefault(x => x.Id == id);
                                EquipmentController.Delete(toDelete);
                            }
                        }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Application.Current.MainWindow.DataContext = new SprzetyView();
        }
    }
}
