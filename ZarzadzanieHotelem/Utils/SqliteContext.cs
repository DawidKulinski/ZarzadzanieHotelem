using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZarzadzanieHotelem.Models;


namespace ZarzadzanieHotelem.Utils
{
    public class SqliteContext : DbContext
    {
        //for later usage
        //private readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public SqliteContext(string connString):
            base(connString)
        {
            Database.ExecuteSqlCommand("PRAGMA journal_mode=WAL;");
        }



        public DbSet<Cleaning> Cleanings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentPerRoom> EquipmentPerRooms { get; set; }
        public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Worker> Workers { get; set; }

    }
}
