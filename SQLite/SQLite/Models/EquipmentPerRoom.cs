using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Models
{
    public class EquipmentPerRoom
    {
        [Key]
        public int Id { get; set; }
        public int IdEquipment { get; set; }
        public int IdRoom { get; set; }
        public int Count { get; set; }

        [ForeignKey("IdEquipment")]
        public virtual Equipment Equipment { get; set; }
        [ForeignKey("IdRoom")]
        public virtual Room Room { get; set; }

    }
}
