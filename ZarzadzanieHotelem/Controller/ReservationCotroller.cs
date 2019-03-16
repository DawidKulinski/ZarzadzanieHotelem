﻿using System;
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
                Customer cus = conn.Customers.FirstOrDefault(p => p.Id == rez.IdCustomer);
                Room room = conn.Rooms.FirstOrDefault(p => p.Id == rez.IdRoom);
                rez.Customer = cus ?? throw new Exception("Nie ma klienta o ID: " + rez.IdCustomer);
                rez.Room = room ?? throw new Exception("Nie ma pokoju o ID: " + rez.IdRoom);
                conn.Reservations.Add(rez);

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
                }
                else
                    throw new Exception("Nie znaleziono rezerwacji do usunięcia");
            }
        }

        public void Edit(Reservation rez)
        {
            using (var conn = new SqliteContext(@"testDb"))
            {
                Reservation rezToChange = conn.Reservations.Where(p => p.Id == rez.Id).FirstOrDefault();
                if (rezToChange != null)
                {
                    rezToChange.IdCustomer = rez.IdCustomer;
                    rezToChange.IdRoom = rez.IdRoom;
                    rezToChange.StartTime = rez.StartTime;
                    rezToChange.StopTime = rez.StopTime;

                    Customer cus = conn.Customers.FirstOrDefault(p => p.Id == rez.IdCustomer);
                    Room room = conn.Rooms.FirstOrDefault(p => p.Id == rez.IdRoom);

                    rez.Customer = cus ?? throw new Exception("Nie ma klienta o ID: " + rez.IdCustomer);
                    rez.Room = room ?? throw new Exception("Nie ma pokoju o ID: " + rez.IdRoom);

                    conn.SaveChanges();
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
    }
}