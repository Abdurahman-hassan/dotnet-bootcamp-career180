using System;
using System.Collections.Generic;

namespace movieReservationSystem.Models
{
    public class Showtime
    {
        public string Id { get; set; } = string.Empty;
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public List<Seat> Seats { get; set; } = new List<Seat>();
    }
}