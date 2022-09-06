using LibraryMVC.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class ReservationRepository
    {
        private readonly Context _context;
        public ReservationRepository(Context context)
        {
            _context = context;
        }
        public int AddReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return reservation.Id;
        }

        public Reservation GetReservationById(int reservationId)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);
            return reservation;
        }

        public IQueryable<Reservation> GetAllReservations()
        {
            var reservation = _context.Reservations;
            return reservation;
        }

        public void UpdateReservation(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            _context.SaveChanges();
        }

        public void DeleteReservation(int reservationId)
        {
            var reservation = _context.Reservations.Find(reservationId);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
            }
        }
    }
}
