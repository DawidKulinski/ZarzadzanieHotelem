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
        /// <summary>
        /// Standard constructor of the DbContext with default Connection String.
        /// It points to the D:\test.sqlite
        /// </summary>
        /// <param name="connString">
        /// Connection string or its name in App.config
        /// </param>
        public SqliteContext(string connString = "testDb"):
            base(connString)
        {
        }

        /// <summary>
        /// Implemented for the automated DB creation if does not exist.
        /// </summary>
        /// <param name="modelBuilder">
        /// Default Param
        /// </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var SqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<SqliteContext>(modelBuilder);
            Database.SetInitializer(SqliteConnectionInitializer);
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
