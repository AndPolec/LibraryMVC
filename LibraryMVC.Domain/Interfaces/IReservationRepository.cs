using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IReservationRepository
    {
        int AddReservation(Reservation reservation);
        void DeleteReservation(int reservationId);
        IQueryable<Reservation> GetAllReservations();
        Reservation GetReservationById(int reservationId);
        void UpdateReservation(Reservation reservation);
    }
}