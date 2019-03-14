using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Models
{
    public class ParkingSlot
    {
        public ParkingSlot()
        {
            this.ParkingReservation = new HashSet<ParkingReservation>();
        }
        [Key]
        public int Id { get; set; }
        public string SlotCode { get; set; }
        public virtual ICollection<ParkingReservation> ParkingReservation { get; set; }
    }
}
