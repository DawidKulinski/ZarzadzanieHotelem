using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.CodeFirst;
using SQLite.Models;

namespace SQLite
{
    public class TestData
    {
        [Key]
        public int id { get; set; }
        public string Test { get; set; }
    }

    public class SqliteContext : DbContext
    {
        //for later usage
        //private readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public SqliteContext(string connString) :
            base(connString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var SqliteConnection = new SqliteCreateDatabaseIfNotExists<SqliteContext>(modelBuilder);
            Database.SetInitializer(SqliteConnection);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Cleaning> Cleanings { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentPerRoom> EquipmentPerRooms { get; set; }
        public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<ParkingReservation> ParkingReservations { get; set; }
        public DbSet<Worker> Workers { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                conn.Rooms.Add(new Room()
                {
                    RoomNumber = 2137,
                    RoomStandard = 4
                });

                conn.Customers.Add(new Customer()
                {
                    Name = "Test",
                    LastName = "Test2"
                });

                conn.Reservations.Add(new Reservation()
                {
                    IdCustomer = conn.Customers.Select(x => x.Id).FirstOrDefault(),
                    IdRoom = conn.Rooms.Select(x => x.Id).FirstOrDefault(),
                    StartTime = DateTime.Now,
                    StopTime = DateTime.MaxValue
                });

                conn.SaveChanges();

                var xd = conn.Reservations.FirstOrDefault();

                Console.ReadKey();

            }

        }
    }
}
