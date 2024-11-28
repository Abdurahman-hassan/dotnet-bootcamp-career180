using movieReservationSystem.Models;
using System.Collections.Generic;

namespace movieReservationSystem.Repositories
{
    public interface IReservationRepository
    {
        List<Reservation> GetAllReservations();
        Reservation GetReservationById(string id);
        Reservation AddReservation(Reservation reservation);
        Reservation UpdateReservation(Reservation reservation);
        void DeleteReservation(string id);
    }
}