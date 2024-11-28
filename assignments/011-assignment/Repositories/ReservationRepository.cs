using MongoDB.Driver;
using movieReservationSystem.Models;
using movieReservationSystem.Utils;
using System.Collections.Generic;
using System.Linq;

namespace movieReservationSystem.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly IMongoCollection<Reservation> _reservations;

        public ReservationRepository(MongoDbContext dbContext)
        {
            _reservations = dbContext.Reservations;
        }

        public List<Reservation> GetAllReservations() =>
            _reservations.Find(reservation => true).ToList();

        public Reservation GetReservationById(string id) =>
            _reservations.Find<Reservation>(reservation => reservation.Id == id).FirstOrDefault();

        public Reservation AddReservation(Reservation reservation)
        {
            _reservations.InsertOne(reservation);
            return reservation;
        }

        public Reservation UpdateReservation(Reservation reservationIn)
        {
            _reservations.ReplaceOne(reservation => reservation.Id == reservationIn.Id, reservationIn);
            return reservationIn;
        }

        public void DeleteReservation(string id) =>
            _reservations.DeleteOne(reservation => reservation.Id == id);
    }
}