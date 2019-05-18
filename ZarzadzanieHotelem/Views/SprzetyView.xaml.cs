using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZarzadzanieHotelem.Controller;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Views
{
    /// <summary>
    /// Logika interakcji dla klasy SprzetyView.xaml
    /// </summary>
    public partial class SprzetyView : UserControl
    {
        public SprzetyView()
        {
            InitializeComponent();

            using (var context = new SqliteContext())
            {
                context.Equipments.ToList()
                    .ForEach(x => SprzetyDG.Items.Add(x));
            }
        }
    }
}
