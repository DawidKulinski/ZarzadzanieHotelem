using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Controller
{
    class ReservationCotroller
    {

        public void Add(Reservation rez)
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                conn.Customers.Add(new Customer()
                {
                    Id = 1,
                    Name = "Test",
                    LastName = "Testowy"
                });
                Customer cus = conn.Customers.FirstOrDefault(p => p.Id == rez.IdCustomer);
                Room room = conn.Rooms.FirstOrDefault(p => p.Id == rez.IdRoom);
                rez.Customer = cus ?? throw new Exception("Nie ma klienta o ID: " + rez.IdCustomer);
                rez.Room = room ?? throw new Exception("Nie ma pokoju o ID: " + rez.IdRoom);

                if (conn.Reservations.Any(p => p.IdRoom == rez.IdRoom && (p.StartTime == rez.StartTime || p.StartTime == rez.StopTime
                || p.StopTime == rez.StopTime || p.StopTime == rez.StartTime
                || (p.StartTime > rez.StartTime && p.StartTime < rez.StopTime)
                || (p.StartTime < rez.StartTime && p.StopTime > rez.StartTime)
                || (p.StartTime > rez.StartTime && p.StopTime < rez.StopTime)
                || (p.StartTime < rez.StartTime && p.StopTime > rez.StopTime))))
                    throw new Exception("W tym terminie pokój jest już zarezerwowany");


                conn.Reservations.Add(rez);
                AddCleaning(rez);
                conn.SaveChanges();
            }
        }

        public void Delete(Reservation rez)
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                Reservation toDelete = conn.Reservations.FirstOrDefault(p => p.Id == rez.Id);
                if (toDelete != null)
                {
                    conn.Reservations.Remove(toDelete);
                    conn.SaveChanges();

                    DeleteClining(rez, conn);
                }
                else
                    throw new Exception("Nie znaleziono rezerwacji do usunięcia");
            }
        }

        public void Edit(Reservation rez)
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                if (rez.StartTime > rez.StopTime)
                {
                    throw new Exception("Data zakończenia pobytu nie może być mniejsza niż rozpoczęcie pobytu.");
                }
                Reservation rezToChange = conn.Reservations.Where(p => p.Id == rez.Id).FirstOrDefault();
                Reservation rezBeforeChanges = new Reservation
                {
                    StartTime = rezToChange.StartTime,
                    StopTime = rezToChange.StopTime
                };
                if (rezToChange != null)
                {
                    rezToChange.IdCustomer = rez.IdCustomer;
                    rezToChange.IdRoom = rez.IdRoom;
                    rezToChange.StartTime = rez.StartTime;
                    rezToChange.StopTime = rez.StopTime;

                    if (conn.Reservations.Any(p => p.Id != rez.Id && p.IdRoom == rez.IdRoom && (p.StartTime == rez.StartTime || p.StartTime == rez.StopTime
                         || p.StopTime == rez.StopTime || p.StopTime == rez.StartTime
                         || (p.StartTime > rez.StartTime && p.StartTime < rez.StopTime)
                         || (p.StartTime < rez.StartTime && p.StopTime > rez.StartTime)
                         || (p.StartTime > rez.StartTime && p.StopTime < rez.StopTime)
                         || (p.StartTime < rez.StartTime && p.StopTime > rez.StopTime))))
                        throw new Exception("W tym terminie pokój jest już zarezerwowany, nie można zmienić terminu rezerwacji");

                    Customer cus = conn.Customers.FirstOrDefault(p => p.Id == rez.IdCustomer);
                    Room room = conn.Rooms.FirstOrDefault(p => p.Id == rez.IdRoom);

                    rez.Customer = cus ?? throw new Exception("Nie ma klienta o ID: " + rez.IdCustomer);
                    rez.Room = room ?? throw new Exception("Nie ma pokoju o ID: " + rez.IdRoom);

                    conn.SaveChanges();

                    EditClining(rezBeforeChanges, rezToChange, conn);
                }
                else
                    throw new Exception("Nie znaleziono rezerwacji do zmodyfikowania");
            }
        }

        public IEnumerable<Reservation> GetAll()
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                return conn.Reservations.ToList();
            }
        }

        private void AddCleaning(Reservation rez)
        {
            for (DateTime date = rez.StartTime.Date; date < rez.StopTime.Date; date = date.AddDays(1))
            {
                Cleaning clining = new Cleaning
                {
                    IdRoom = rez.IdRoom,
                    CleanTime = date.Date,
                    IdWorker = -1
                };
                CleaningController.Add(clining);
            }

            Cleaning lastClining = new Cleaning
            {
                IdRoom = rez.IdRoom,
                CleanTime = rez.StopTime.Date,
                IdWorker = -1
            };
            CleaningController.Add(lastClining, true);
        }

        private void DeleteClining(Reservation rez, SqliteContext conn)
        {
            var cleanings = conn.Cleanings.Where(r => r.IdRoom == rez.IdRoom).ToList();

            for (DateTime date = rez.StartTime.Date; date <= rez.StopTime.Date; date = date.AddDays(1))
            {
                var cleaningsForDay = cleanings.Where(t => t.CleanTime.Date == date.Date);
                foreach (var cleaning in cleaningsForDay)
                    conn.Cleanings.Remove(cleaning);
            }
            conn.SaveChanges();
        }

        private void EditClining(Reservation rezBeforeChange, Reservation rezAfterChange, SqliteContext conn)
        {
            var cleanings = conn.Cleanings.Where(r => r.IdRoom == rezAfterChange.IdRoom).ToList();

            if (rezBeforeChange.StartTime.Date < rezAfterChange.StartTime.Date)
            {
                for (DateTime date = rezBeforeChange.StartTime.Date; date < rezAfterChange.StartTime.Date; date = date.AddDays(1))
                {
                    var cleaningsForDay = cleanings.Where(t => t.CleanTime.Date == date.Date);
                    foreach (var cleaning in cleaningsForDay)
                        conn.Cleanings.Remove(cleaning);
                }
                conn.SaveChanges();
            }
            if (rezBeforeChange.StartTime.Date > rezAfterChange.StartTime.Date)
            {
                for (DateTime date = rezAfterChange.StartTime.Date; date < rezBeforeChange.StartTime.Date; date = date.AddDays(1))
                {
                    Cleaning clining = new Cleaning
                    {
                        IdRoom = rezAfterChange.IdRoom,
                        CleanTime = date.Date,
                        IdWorker = -1
                    };
                    CleaningController.Add(clining);
                }
            }
            if (rezAfterChange.StopTime.Date < rezBeforeChange.StopTime.Date)
            {
                for (DateTime date = rezAfterChange.StopTime.Date; date <= rezBeforeChange.StopTime.Date; date = date.AddDays(1))
                {
                    var cleaningsForDay = cleanings.Where(t => t.CleanTime.Date == date.Date);
                    foreach (var cleaning in cleaningsForDay)
                        conn.Cleanings.Remove(cleaning);
                }
                conn.SaveChanges();

                Cleaning endingClining = new Cleaning
                {
                    IdRoom = rezAfterChange.IdRoom,
                    CleanTime = rezAfterChange.StopTime.Date,
                    IdWorker = -1
                };
                CleaningController.Add(endingClining, true);
            }
            if (rezAfterChange.StopTime.Date > rezBeforeChange.StopTime.Date)
            {
                var cleaningsForDay = cleanings.Where(t => t.CleanTime.Date == rezBeforeChange.StopTime.Date).OrderBy(t => t.CleanTime).LastOrDefault();
                if (cleaningsForDay != null)
                {
                    conn.Cleanings.Remove(cleaningsForDay);
                    conn.SaveChanges();
                }

                for (DateTime date = rezBeforeChange.StopTime.Date.AddDays(1); date < rezAfterChange.StopTime.Date; date = date.AddDays(1))
                {
                    Cleaning clining = new Cleaning
                    {
                        IdRoom = rezAfterChange.IdRoom,
                        CleanTime = date.Date,
                        IdWorker = -1
                    };
                    CleaningController.Add(clining);
                }
                Cleaning endingClining = new Cleaning
                {
                    IdRoom = rezAfterChange.IdRoom,
                    CleanTime = rezAfterChange.StopTime.Date,
                    IdWorker = -1
                };
                CleaningController.Add(endingClining, true);
            }
        }
    }
}
