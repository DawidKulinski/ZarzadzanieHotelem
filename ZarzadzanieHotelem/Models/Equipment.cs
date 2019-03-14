using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZarzadzanieHotelem.Models
{
    public class Equipment
    {
        public Equipment()
        {
            this.EquipmentPerRoom = new HashSet<EquipmentPerRoom>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

        public virtual ICollection<EquipmentPerRoom> EquipmentPerRoom { get; set; }
    }
}
