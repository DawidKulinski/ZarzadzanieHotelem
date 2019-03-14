using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Models
{
    public enum Position
    {
        Manager,
        Clerk,
        Maid
    }

    public class Worker
    {
        public Worker()
        {
            this.Cleaning = new HashSet<Cleaning>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Position Position { get; set; }

        public ICollection<Cleaning> Cleaning { get; set; }
    }
}
