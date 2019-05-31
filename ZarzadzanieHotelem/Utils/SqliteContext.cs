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

            context.Customers.Add(new Customer() {Id = 1, LastName = "Kowalski", Name = "Andrzej"});
            context.Customers.Add(new Customer() {Id = 2, LastName = "Kowalski", Name = "Jan"});
            context.Customers.Add(new Customer() {Id = 3, LastName = "Testowe", Name = "Konto"});
            context.Customers.Add(new Customer() {Id = 4, LastName = "Wróbel", Name = "Jan"});

            context.SaveChanges();

            context.Reservations.Add(new Reservation(){IdCustomer = 1, IdRoom = 1, StartTime = DateTime.MinValue, StopTime = DateTime.MaxValue, Price = 1234});
            context.Reservations.Add(new Reservation(){IdCustomer = 2, IdRoom = 2, StartTime = DateTime.MinValue, StopTime = DateTime.MaxValue, Price = 1234});
            context.Reservations.Add(new Reservation(){IdCustomer = 3, IdRoom = 3, StartTime = DateTime.MinValue, StopTime = DateTime.MaxValue, Price = 1234});
            context.Reservations.Add(new Reservation(){IdCustomer = 4, IdRoom = 4, StartTime = DateTime.MinValue, StopTime = DateTime.MaxValue, Price = 1234});
            context.Reservations.Add(new Reservation(){IdCustomer = 1, IdRoom = 1, StartTime = DateTime.MinValue, StopTime = DateTime.MaxValue, Price = 1234});

            context.ParkingSlots.Add(new ParkingSlot() { Id = 1, SlotCode = "A1", Occupied = false });
            context.ParkingSlots.Add(new ParkingSlot() { Id = 2, SlotCode = "A2", Occupied = true });
            context.ParkingSlots.Add(new ParkingSlot() { Id = 3, SlotCode = "A3", Occupied = false });
            context.ParkingSlots.Add(new ParkingSlot() { Id = 4, SlotCode = "A4", Occupied = false });
            context.ParkingSlots.Add(new ParkingSlot() { Id = 5, SlotCode = "A5", Occupied = false });

            context.SaveChanges();
        }
    }
}
