using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZarzadzanieHotelem.Utils
{
    public static class RoomScheduler
    {
        public static IEnumerable<Tuple<DateTime, DateTime>> GetReservations(int roomId)
        {
            using (var DbContext = new SqliteContext())
            {
                return DbContext.Reservations.Where(x => x.IdRoom == roomId)
                    .Select(x => new Tuple<DateTime, DateTime>(x.StartTime, x.StopTime));
            }
        }
    }
}
