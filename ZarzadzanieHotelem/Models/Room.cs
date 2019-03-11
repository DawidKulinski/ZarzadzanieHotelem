using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZarzadzanieHotelem.Models
{
    public class Room
    {
        public Room()
        {
            this.Reservation = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int RoomStandard { get; set; }
        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}
