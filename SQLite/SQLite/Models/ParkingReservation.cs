using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Models
{
    public class ParkingReservation
    {
        [Key]
        public int Id { get; set; }
        public int IdReservation { get; set; }
        public int IdParkingSlot { get; set; }
        [ForeignKey("IdReservation")]
        public virtual Reservation Reservation { get; set; }
        [ForeignKey("IdParkingSlot")]
        public virtual ParkingSlot ParkingSlot { get; set; }
    }
}
