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
            this.Cleaning = new HashSet<Cleaning>();
            this.EquipmentPerRoom = new HashSet<EquipmentPerRoom>();
        }

        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int RoomStandard { get; set; }
        public virtual ICollection<Reservation> Reservation { get; set; }
        public virtual ICollection<Cleaning> Cleaning { get; set; }
        public virtual ICollection<EquipmentPerRoom> EquipmentPerRoom { get; set; }
    }
}
