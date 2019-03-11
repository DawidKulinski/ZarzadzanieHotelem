using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using SQLite.CodeFirst;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZarzadzanieHotelem.Models;


namespace ZarzadzanieHotelem.Utils
{
    public class SqliteContext : DbContext
    {

        public SqliteContext(string connString = "testDb"):
            base(connString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var SqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<SqliteContext>(modelBuilder);
            Database.SetInitializer(SqliteConnectionInitializer);
        }

        //public DbSet<TestData> TestDatas { get; set; }
        //public DbSet<Cleaning> Cleanings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        //public DbSet<Equipment> Equipments { get; set; }
        //public DbSet<EquipmentPerRoom> EquipmentPerRooms { get; set; }
        //public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        //public DbSet<Worker> Workers { get; set; }
    }
}
