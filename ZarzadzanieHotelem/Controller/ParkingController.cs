using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Controller
{
    public class ParkingController
    {
        public static void Add(ParkingSlot parkingSlot)
        {
            using (var conn = new SqliteContext())
            {
                conn.ParkingSlots.Add(parkingSlot);
                conn.SaveChanges();
            }
        }

        public static void Delete(ParkingSlot parkingSlot)
        {
            using (var conn = new SqliteContext())
            {
                ParkingSlot toDelete = conn.ParkingSlots.FirstOrDefault(w => w.Id == parkingSlot.Id);
                if (toDelete != null)
                {
                    conn.ParkingSlots.Remove(toDelete);
                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono miejsca parkingowego do usunięcia");
            }
        }

        public static void Edit(ParkingSlot parkingSlot)
        {
            using (var conn = new SqliteContext())
            {
                ParkingSlot toChange = conn.ParkingSlots
                    .Where(w => w.Id == parkingSlot.Id)
                    .FirstOrDefault();
                if (toChange != null)
                {
                    toChange = ClassUtils
                        .EditElement(toChange, parkingSlot);

                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono sprzętu do edycji");
            }
        }
    }
}
