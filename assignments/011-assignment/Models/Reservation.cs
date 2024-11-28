using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace movieReservationSystem.Models
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public string ShowtimeId { get; set; } = string.Empty;
        public List<Seat> Seats { get; set; } = new List<Seat>();
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
    }
}