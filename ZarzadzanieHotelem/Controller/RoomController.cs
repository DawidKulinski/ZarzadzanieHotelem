using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Controller
{
    public static class RoomController
    {
        public static List<Tuple<DateTime, DateTime>> GetReservations(int roomId)
        {
            using (var DbContext = new SqliteContext())
            {
                return DbContext.Reservations.Where(x => x.IdRoom == roomId).ToList()
                    .Select(x => new Tuple<DateTime, DateTime>(x.StartTime, x.StopTime)).ToList();
            }
        }

        public static void Add(Room room)
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                if (conn.Rooms.Any(p => p.Id == room.Id))
                    throw new Exception("Pokój o tym ID już istnieje");
                if (conn.Rooms.Any(p => p.RoomNumber == room.RoomNumber))
                    throw new Exception("Pokój o tym numerze już istnieje");

                conn.Rooms.Add(room);
                conn.SaveChanges();
            }
        }

        public static void Delete(Room room)
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                Room toDelete = conn.Rooms.FirstOrDefault(p => p.Id == room.Id);
                if (toDelete != null)
                {
                    conn.Rooms.Remove(toDelete);
                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono pokoju do usunięcia");
            }
        }

        public static void Edit(Room room)
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                Room roomToChange = conn.Rooms.Where(p => p.Id == room.Id).FirstOrDefault();
                if (roomToChange != null)
                {
                    roomToChange.Id = room.Id;
                    roomToChange.RoomNumber = room.RoomNumber;
                    roomToChange.RoomStandard = room.RoomStandard;

                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono pokoju do usunięcia");
            }
        }
    }
}
