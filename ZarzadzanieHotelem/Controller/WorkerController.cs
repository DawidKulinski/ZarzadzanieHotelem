using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Controller
{
    class WorkerController
    {

        public static void Add(Worker worker)
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                if (conn.Workers.Any(w => w.Id == worker.Id))
                    throw new Exception("Pracownik o tym ID już istnieje");

                conn.Workers.Add(worker);
                conn.SaveChanges();
            }
        }

        public static void Delete(Worker worker)
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                Worker toDelete = conn.Workers.FirstOrDefault(w => w.Id == worker.Id);
                if (toDelete != null)
                {
                    conn.Workers.Remove(toDelete);
                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono pracownika do usunięcia");
            }
        }

        public static void Edit(Worker worker)
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                Worker toChange = conn.Workers.Where(w => w.Id == worker.Id).FirstOrDefault();
                if (toChange != null)
                {
                    toChange.Id = worker.Id;
                    toChange.Name = worker.Name;
                    toChange.LastName = worker.LastName;
                    toChange.Position = worker.Position;
                    toChange.Cleaning = worker.Cleaning;

                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono pracownika do edycji");
            }
        }
    }
}
