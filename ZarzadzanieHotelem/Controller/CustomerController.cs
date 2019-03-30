using System;
using System.Linq;
using ZarzadzanieHotelem.Models;
using ZarzadzanieHotelem.Utils;

namespace ZarzadzanieHotelem.Controller
{
    class CustomerController
    {

        public static void Add(Customer customer)
        {
            using (var conn = new SqliteContext())
            {
                //if (conn.Customers.Any(w => w.Id == customer.Id))
                //    throw new Exception("Pracownik o tym ID już istnieje");

                conn.Customers.Add(customer);
                conn.SaveChanges();
            }
        }

        public static void Delete(Customer customer)
        {
            using (var conn = new SqliteContext())
            {
                Customer toDelete = conn.Customers.FirstOrDefault(w => w.Id == customer.Id);
                if (toDelete != null)
                {
                    conn.Customers.Remove(toDelete);
                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono klienta do usunięcia");
            }
        }

        public static void Edit(Customer customer)
        {
            using (var conn = new SqliteContext())
            {
                Customer toChange = conn.Customers.Where(w => w.Id == customer.Id).FirstOrDefault();
                if (toChange != null)
                {
                    //toChange.Id = customer.Id;
                    toChange.Name = customer.Name;
                    toChange.LastName = customer.LastName;

                    conn.SaveChanges();
                }
                else
                    throw new Exception("Nie znaleziono pracownika do edycji");
            }
        }
    }
}
