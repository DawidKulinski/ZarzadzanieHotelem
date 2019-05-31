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
            using (var conn = new SqliteContext())
            {
                Customer cus = conn.Customers.FirstOrDefault(p => p.Id == rez.IdCustomer);
                Room room = conn.Rooms.FirstOrDefault(p => p.Id == rez.IdRoom);
                rez.Customer = cus ?? throw new Exception("Nie ma klienta o ID: " + rez.IdCustomer);
                rez.Room = room ?? throw new Exception("Nie ma pokoju o ID: " + rez.IdRoom);

                IList<Reservation> reservations = conn.Reservations.Where(p => p.IdRoom == rez.IdRoom).ToList();

                if (reservations.Any(p => p.StartTime.Date == rez.StartTime.Date || p.StopTime.Date == rez.StopTime.Date
                || (p.StartTime.Date > rez.StartTime.Date && p.StartTime.Date < rez.StopTime.Date)
                || (p.StartTime.Date < rez.StartTime.Date && p.StopTime.Date > rez.StartTime.Date)
                || (p.StartTime.Date > rez.StartTime.Date && p.StopTime.Date < rez.StopTime.Date)
                || (p.StartTime.Date < rez.StartTime.Date && p.StopTime.Date > rez.StopTime.Date)))
                    throw new Exception("W tym terminie pokój jest już zarezerwowany");

                rez.StartTime = rez.StartTime.AddHours(15);
                rez.StopTime = rez.StopTime.AddHours(10);

                conn.Reservations.Add(rez);
                AddCleaning(rez, conn);
                conn.SaveChanges();
            }
        }

        public void Delete(Reservation rez)
        {
            using (var conn = new SqliteContext())
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
            using (var conn = new SqliteContext())
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

                    IList<Reservation> reservations = conn.Reservations.Where(p => p.Id != rez.Id && p.IdRoom == rez.IdRoom).ToList();

                    if (reservations.Any(p => p.StartTime.Date == rez.StartTime.Date || p.StopTime.Date == rez.StopTime.Date
                         || (p.StartTime.Date > rez.StartTime.Date && p.StartTime.Date < rez.StopTime.Date)
                         || (p.StartTime.Date < rez.StartTime.Date && p.StopTime.Date > rez.StartTime.Date)
                         || (p.StartTime.Date > rez.StartTime.Date && p.StopTime.Date < rez.StopTime.Date)
                         || (p.StartTime.Date < rez.StartTime.Date && p.StopTime.Date > rez.StopTime.Date)))
                        throw new Exception("W tym terminie pokój jest już zarezerwowany, nie można zmienić terminu rezerwacji");

                    rezToChange.StartTime = rezToChange.StartTime.AddHours(15);
                    rezToChange.StopTime = rezToChange.StopTime.AddHours(10);

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
            using (var conn = new SqliteContext())
            {
                return conn.Reservations.ToList();
            }
        }

        private void AddCleaning(Reservation rez, SqliteContext conn)
        {
            DateTime dateStart = rez.StartTime.Date;

            var clinings = conn.Cleanings.Where(p => p.IdRoom == rez.IdRoom).ToList();
            if (clinings.Any(p => p.CleanTime.Date == dateStart))
                dateStart = dateStart.AddDays(1);

            for (DateTime date = dateStart; date < rez.StopTime.Date; date = date.AddDays(1))
            {
                Cleaning clining = new Cleaning
                {
                    IdRoom = rez.IdRoom,
                    CleanTime = date.Date,
                    IdWorker = -1
                };
                CleaningController.Add(clining);
            }

            if (clinings.Any(p => p.CleanTime.Date == rez.StopTime.Date))
            {
                Cleaning cliningToDelete = clinings.First(p => p.CleanTime.Date == rez.StopTime.Date);
                CleaningController.Delete(cliningToDelete);
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
            DateTime dateStart = rez.StartTime.Date;
            bool deleteOnlyOne = false;

            var reservations = conn.Reservations.Where(p => p.IdRoom == rez.IdRoom).ToList();
            if (reservations.Any(p => p.StopTime.Date == rez.StartTime.Date))
                dateStart = dateStart.AddDays(1);

            if (reservations.Any(p => p.StartTime.Date == rez.StopTime.Date))
                deleteOnlyOne = true;

            for (DateTime date = dateStart; date <= rez.StopTime.Date; date = date.AddDays(1))
            {
                var cleaningsForDay = cleanings.Where(t => t.CleanTime.Date == date.Date);
                if (deleteOnlyOne && date == rez.StopTime.Date && cleaningsForDay.Count() == 2)
                {
                    conn.Cleanings.Remove(cleaningsForDay.First());
                }
                else
                {
                    foreach (var cleaning in cleaningsForDay)
                        conn.Cleanings.Remove(cleaning);
                }
            }

            conn.SaveChanges();
        }

        private void EditClining(Reservation rezBeforeChange, Reservation rezAfterChange, SqliteContext conn)
        {
            var cleanings = conn.Cleanings.Where(r => r.IdRoom == rezAfterChange.IdRoom).ToList();
            var reservations = conn.Reservations.Where(p => p.IdRoom == rezAfterChange.IdRoom).ToList();

            if (rezBeforeChange.StartTime.Date < rezAfterChange.StartTime.Date)
            {
                DateTime dateStart = rezBeforeChange.StartTime.Date;

                if (reservations.Any(p => p.StopTime.Date == rezBeforeChange.StartTime.Date))
                    dateStart = dateStart.AddDays(1);

                for (DateTime date = dateStart; date < rezAfterChange.StartTime.Date; date = date.AddDays(1))
                {
                    var cleaningsForDay = cleanings.Where(t => t.CleanTime.Date == date.Date);
                    foreach (var cleaning in cleaningsForDay)
                        conn.Cleanings.Remove(cleaning);
                }
                conn.SaveChanges();
            }
            if (rezBeforeChange.StartTime.Date > rezAfterChange.StartTime.Date)
            {
                DateTime dateStart = rezAfterChange.StartTime.Date;
                if (reservations.Any(p => p.StopTime.Date == rezAfterChange.StartTime.Date))
                    dateStart = dateStart.AddDays(1);

                for (DateTime date = dateStart; date < rezBeforeChange.StartTime.Date; date = date.AddDays(1))
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
                bool isReservation = false;
                if (reservations.Any(p => p.StartTime.Date == rezBeforeChange.StopTime.Date))
                {
                    isReservation = true;
                }

                for (DateTime date = rezAfterChange.StopTime.Date; date <= rezBeforeChange.StopTime.Date; date = date.AddDays(1))
                {
                    var cleaningsForDay = cleanings.Where(t => t.CleanTime.Date == date.Date);

                    if (isReservation && date == rezBeforeChange.StopTime.Date && cleaningsForDay.Count() == 2)
                    {
                        conn.Cleanings.Remove(cleaningsForDay.First());
                    }
                    else
                    {
                        foreach (var cleaning in cleaningsForDay)
                            conn.Cleanings.Remove(cleaning);
                    }
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

                if (cleanings.Any(p => p.CleanTime.Date == rezAfterChange.StopTime.Date))
                {
                    Cleaning clean = cleanings.First(p => p.CleanTime.Date == rezAfterChange.StopTime.Date);
                    conn.Cleanings.Remove(clean);
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
