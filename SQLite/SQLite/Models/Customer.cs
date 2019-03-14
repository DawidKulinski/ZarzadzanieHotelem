using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Reservation = new HashSet<Reservation>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}
