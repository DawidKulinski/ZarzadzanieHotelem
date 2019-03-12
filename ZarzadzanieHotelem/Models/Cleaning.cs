using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZarzadzanieHotelem.Models
{
    public class Cleaning
    {
        public int Id { get; set; }
        public int IdRoom { get; set; }
        public int IdWorker { get; set; }
        public DateTime CleanTime { get; set; }

        [ForeignKey("IdRoom")]
        public virtual Room Room { get; set; }
        [ForeignKey("IdWorker")]
        public virtual Worker Worker { get; set; }
    }
}
