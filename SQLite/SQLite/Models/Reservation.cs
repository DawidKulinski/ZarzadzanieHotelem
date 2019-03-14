using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Models
{
    public class Reservation
    {
        public Reservation()
        {
            this.ParkingReservation = new HashSet<ParkingReservation>();
        }
        [Key]
        public int Id { get; set; }
        public int IdCustomer { get; set; }
        public int IdRoom { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }

        [ForeignKey("IdCustomer")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("IdRoom")]
        public virtual Room Room { get; set; }

        public virtual ICollection<ParkingReservation> ParkingReservation { get; set; }
    }
}
