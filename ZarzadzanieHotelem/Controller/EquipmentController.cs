using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Controller
{
    public class EquipmentController
    {
        public static void Add(Equipment equipment)
        {
            using (var conn = new SqliteContext())
            {
                conn.Equipments.Add(equipment);
                conn.SaveChanges();
            }
        }

        public static void Delete(Equipment equipment)
        {
            using (var conn = new SqliteContext())
            {
                Equipment toDelete = conn.Equipments.FirstOrDefault(w => w.Id == equipment.Id);
                if (toDelete != null)
                {
                    conn.Equipments.Remove(toDelete);
                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono klienta do usunięcia");
            }
        }

        public static void Edit(Equipment equipment)
        {
            using (var conn = new SqliteContext())
            {
                Equipment toChange = conn.Equipments
                    .Where(w => w.Id == equipment.Id)
                    .FirstOrDefault();
                if (toChange != null)
                {
                    toChange = ClassUtils
                        .EditElement(toChange, equipment);

                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono sprzętu do edycji");
            }
        }
    }
}
