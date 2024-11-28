using movieReservationSystem.Models;
using movieReservationSystem.Repositories;
using System.Collections.Generic;

namespace movieReservationSystem.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservationRepository.GetAllReservations();
        }

        public Reservation GetReservationById(string id)
        {
            return _reservationRepository.GetReservationById(id);
        }

        public Reservation AddReservation(Reservation reservation)
        {
            return _reservationRepository.AddReservation(reservation);
        }

        public Reservation UpdateReservation(Reservation reservation)
        {
            return _reservationRepository.UpdateReservation(reservation);
        }

        public void DeleteReservation(string id)
        {
            _reservationRepository.DeleteReservation(id);
        }
    }
}