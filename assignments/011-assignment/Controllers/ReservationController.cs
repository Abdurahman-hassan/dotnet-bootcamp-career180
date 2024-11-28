using Microsoft.AspNetCore.Mvc;
using movieReservationSystem.Models;
using movieReservationSystem.Services;
using System.Collections.Generic;

namespace movieReservationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public ActionResult<List<Reservation>> Get()
        {
            return _reservationService.GetAllReservations();
        }

        [HttpGet("{id:length(24)}", Name = "GetReservation")]
        public ActionResult<Reservation> Get(string id)
        {
            var reservation = _reservationService.GetReservationById(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return reservation;
        }

        [HttpPost]
        public ActionResult<Reservation> Create(Reservation reservation)
        {
            _reservationService.AddReservation(reservation);
            return CreatedAtRoute("GetReservation", new { id = reservation.Id.ToString() }, reservation);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Reservation reservationIn)
        {
            var reservation = _reservationService.GetReservationById(id);
            if (reservation == null)
            {
                return NotFound();
            }
            _reservationService.UpdateReservation(reservationIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var reservation = _reservationService.GetReservationById(id);
            if (reservation == null)
            {
                return NotFound();
            }
            _reservationService.DeleteReservation(reservation.Id);
            return NoContent();
        }
    }
}