using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
