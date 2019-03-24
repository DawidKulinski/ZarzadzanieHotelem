using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Controller
{
    public static class CleaningController
    {
        public static void Add(Cleaning cleaning, bool endClinning = false)
        {
            //sprzątamy od 10:00 do 15:00
            using (var conn = new SqliteContext())
            {
                if (!conn.Rooms.Any(p => p.Id == cleaning.IdRoom))
                    throw new Exception("Pokój o tym Id nie istnieje");
                if (cleaning.IdWorker != -1 && !conn.Workers.Any(p => p.Id == cleaning.IdWorker))
                    throw new Exception("Pracownik o tym Id nie istnieje");

                TimeSpan startHour = new TimeSpan(10, 0, 0);
                DateTime startTime = cleaning.CleanTime.Date + startHour; //dany dzień godz. 10:00

                if (cleaning.IdWorker != -1)
                {
                    if (endClinning)
                    {
                        DateTime? dateToSetFirst = CheckIfWorkerHaveDubbleTimeAndReturnFirstFreeDate(conn, cleaning.IdWorker, startTime);

                        if (dateToSetFirst.HasValue)
                        {
                            cleaning.CleanTime = dateToSetFirst.Value;

                            //dodanie drugiego pół godziny do grafiku - sprzątanie na koniec wyjazdu trwa 1h
                            Cleaning cleaningSecond = new Cleaning
                            {
                                IdRoom = cleaning.IdRoom,
                                IdWorker = cleaning.IdWorker,
                                CleanTime = cleaning.CleanTime.AddMinutes(30)
                            };
                            conn.Cleanings.Add(cleaningSecond);
                        }
                        else
                            throw new Exception("Pracownik o tym Id nie jest dostępny w wybranym dniu. Pozostaw pole puste lub wybierze inny dzień.");
                    }
                    else
                    {
                        DateTime? dateToSet = CheckIfWorkerHaveTimeAndReturnFirstFreeDate(conn, cleaning.IdWorker, startTime);

                        if (dateToSet.HasValue)
                            cleaning.CleanTime = dateToSet.Value;
                        else
                            throw new Exception("Pracownik o tym Id nie jest dostępny w wybranym dniu. Pozostaw pole puste lub wybierze inny dzień.");
                    }
                }
                else
                {
                    bool isAnyWorkerFree = false;
                    IEnumerable<int> workers = conn.Workers.Include(p => p.Cleaning).Where(p => p.Position == Position.Maid)
                        .OrderBy(p => p.Cleaning.Count())
                        .Select(p => p.Id);

                    if (endClinning)
                    {
                        foreach (var worker in workers)
                        {
                            DateTime? dateToSetFirst = CheckIfWorkerHaveDubbleTimeAndReturnFirstFreeDate(conn, cleaning.IdWorker, startTime);

                            if (dateToSetFirst.HasValue)
                            {
                                cleaning.CleanTime = dateToSetFirst.Value;
                                cleaning.IdWorker = worker;
                                isAnyWorkerFree = true;

                                //dodanie drugiego pół godziny do grafiku - sprzątanie na koniec wyjazdu trwa 1h
                                Cleaning cleaningSecond = new Cleaning
                                {
                                    IdRoom = cleaning.IdRoom,
                                    IdWorker = worker,
                                    CleanTime = cleaning.CleanTime.AddMinutes(30)
                                };
                                conn.Cleanings.Add(cleaningSecond);
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var worker in workers)
                        {
                            DateTime? dateToSet = CheckIfWorkerHaveTimeAndReturnFirstFreeDate(conn, worker, startTime);

                            if (dateToSet.HasValue)
                            {
                                cleaning.CleanTime = dateToSet.Value;
                                cleaning.IdWorker = worker;
                                isAnyWorkerFree = true;
                                break;
                            }
                        }
                    }

                    if (!isAnyWorkerFree)
                        throw new Exception("W tym dniu żaden z pracowników nie ma już wolnego czasu na sprzątanie pokoju.");
                }
                conn.Cleanings.Add(cleaning);
                conn.SaveChanges();
            }
        }

        public static void Delete(Cleaning cleaning)
        {
            using (var conn = new SqliteContext())
            {
                Cleaning toDelete = conn.Cleanings.FirstOrDefault(p => p.Id == cleaning.Id);
                if (toDelete != null)
                {
                    conn.Cleanings.Remove(toDelete);
                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono wpisu sprzątania do usunięcia");
            }
        }

        public static void Edit(Cleaning cleaning)
        {
            using (var conn = new SqliteContext())
            {
                Cleaning cleaningToChange = conn.Cleanings.Where(p => p.Id == cleaning.Id).FirstOrDefault();
                if (cleaningToChange != null)
                {
                    cleaningToChange.IdRoom = cleaning.IdRoom;
                    cleaningToChange.IdWorker = cleaning.IdWorker;

                    TimeSpan startHour = new TimeSpan(10, 0, 0);
                    DateTime startTime = cleaning.CleanTime.Date + startHour; //dany dzień godz. 10:00

                    DateTime? dateToSet = CheckIfWorkerHaveTimeAndReturnFirstFreeDate(conn, cleaning.IdWorker, startTime);

                    if (dateToSet.HasValue)
                        cleaningToChange.CleanTime = dateToSet.Value;
                    else
                        throw new Exception("Pracownik o tym Id nie jest dostępny w wybranym dniu. Wybierze inny dzień lub innego pracownika.");

                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono wpisu sprzątania do zmodyfikowania");
            }
        }

        private static DateTime? CheckIfWorkerHaveTimeAndReturnFirstFreeDate(SqliteContext conn, int IdWorker, DateTime date)
        {
            var cliningForWorker = conn.Cleanings.Where(p => p.IdWorker == IdWorker).ToList().Where(x => x.CleanTime.Date == date.Date).OrderBy(z => z.CleanTime);

            if (!cliningForWorker.Any() || cliningForWorker.Count() < 10)
            {
                DateTime endDate = date.AddHours(5);
                while (date < endDate)
                {
                    if (cliningForWorker.Any(p => p.CleanTime == date))
                        date = date.AddMinutes(30);
                    else
                        return date;
                };
                return null;
            }
            else
                return null;
        }

        //dla ostatniego dnia rezewacji wyszukuje 1h wolnego pracownika (czyli 2 x 30 min wolne)
        private static DateTime? CheckIfWorkerHaveDubbleTimeAndReturnFirstFreeDate(SqliteContext conn, int IdWorker, DateTime date)
        {
            var cliningForWorker = conn.Cleanings.Where(p => p.IdWorker == IdWorker).ToList().Where(x => x.CleanTime.Date == date.Date).OrderBy(z => z.CleanTime);

            if (!cliningForWorker.Any() || cliningForWorker.Count() < 9)
            {
                DateTime endDate = date.AddHours(4.5);
                while (date < endDate)
                {
                    if (cliningForWorker.Any(p => p.CleanTime == date))
                        date = date.AddMinutes(30);
                    else
                    {
                        date = date.AddMinutes(30);
                        if (!cliningForWorker.Any(p => p.CleanTime == date))
                            return date.AddMinutes(-30);
                    }
                };
                return null;
            }
            else
                return null;
        }
    }
}
