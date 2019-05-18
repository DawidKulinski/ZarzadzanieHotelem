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
            var SqliteConnectionInitializer = new SqliteContextInitializer(modelBuilder);
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

    public class SqliteContextInitializer : SqliteCreateDatabaseIfNotExists<SqliteContext>
    {
        public SqliteContextInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder) { }

        protected override void Seed(SqliteContext context)
        {
            context.Rooms.Add(new Room() { RoomNumber = 101, RoomStandard = 4, Price = 100.0m });
            context.Rooms.Add(new Room() { RoomNumber = 103, RoomStandard = 3, Price = 70.0m });
            context.Rooms.Add(new Room() { RoomNumber = 201, RoomStandard = 4, Price = 100.0m });
            context.Rooms.Add(new Room() { RoomNumber = 204, RoomStandard = 4, Price = 100.0m });
            context.Rooms.Add(new Room() { RoomNumber = 301, RoomStandard = 4, Price = 100.0m });
            context.Rooms.Add(new Room() { RoomNumber = 302, RoomStandard = 4, Price = 100.0m });

            context.Workers.Add(new Worker() { Name = "Dawid", LastName = "Kuliński", Position = Position.Clerk });
            context.Workers.Add(new Worker() { Name = "Waldemar", LastName = "Pawlik", Position = Position.Maid });
            context.Workers.Add(new Worker() { Name = "Izabela", LastName = "Dyba", Position = Position.Maid });

            context.Equipments.Add(new Equipment() { Name = "Ręczniki", Count = 12 });
            context.Equipments.Add(new Equipment() { Name = "Sejf", Count = 4 });
            context.Equipments.Add(new Equipment() { Name = "Szklanki", Count = 44 });
            context.Equipments.Add(new Equipment() { Name = "Wieszaki", Count = 22 });
            context.Equipments.Add(new Equipment() { Name = "Pościele", Count = 44 });

            context.SaveChanges();
        }
    }
}
