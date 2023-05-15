using FlightBooking.Database;
using FlightBooking.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "RequireAdmin")]
        public IActionResult GetBookings([FromServices] BookingRepository bookingRepository)
        {
            var bookings = bookingRepository.GetBookings();
            List<BookingOutput> bookingsOutput = bookings.Select(b =>
             new BookingOutput
             {
                 BookingId = b.Id,
                 PNR = b.PNR,
                 ArraivalDate = b.Direction == Models.Direction.Up ? b.StartDate : b.ReturnDate.GetValueOrDefault(),
                 ArrivalTime = b.Flight.EndAt,
                 DepartureDate = b.Direction == Models.Direction.Up ? b.StartDate : b.ReturnDate.GetValueOrDefault(),
                 DepartureTime = b.Flight.StartAt,
                 EmailId = b.User.EmailId,
                 FlightNumer = b.Flight.FlightNumber,
                 Airline = b.Flight.Airline,
                 From = b.Flight.From,
                 To = b.Flight.To,
                 Direction = b.Direction,
                 Status = b.Status,
                 Passengers = b.Passengers.Select(p => new PassengerInput
                 {
                     Age = p.Age,
                     FirstName = p.FirstName,
                     LastName = p.LastName,
                     MealType = p.MealType,
                     SeatNumber = p.SeatNumber,
                 }).ToList()
             }).ToList();


            return Ok(bookingsOutput);
        }

        [HttpGet]
        [Route("history/{emailId}")]
        [Authorize(Policy = "RequireUser")]
        public IActionResult GetUserBooking(string emailId, [FromServices] BookingRepository bookingRepository)
        {
            var bookings = bookingRepository.GetBookings().Where(b => b.User.EmailId == emailId);
            List<BookingOutput> bookingsOutput = bookings.Select(b =>
             new BookingOutput
             {
                 BookingId = b.Id,
                 PNR = b.PNR,
                 ArraivalDate = b.Direction == Models.Direction.Up ? b.StartDate : b.ReturnDate.GetValueOrDefault(),
                 ArrivalTime = b.Flight.EndAt,
                 DepartureDate = b.Direction == Models.Direction.Up ? b.StartDate : b.ReturnDate.GetValueOrDefault(),
                 DepartureTime = b.Flight.StartAt,
                 EmailId = b.User.EmailId,
                 FlightNumer = b.Flight.FlightNumber,
                 Airline = b.Flight.Airline,
                 From = b.Flight.From,
                 To = b.Flight.To,
                 Direction = b.Direction,
                 Status = b.Status,
                 Passengers = b.Passengers.Select(p => new PassengerInput
                 {
                     Age = p.Age,
                     FirstName = p.FirstName,
                     LastName = p.LastName,
                     MealType = p.MealType,
                     SeatNumber = p.SeatNumber,
                 }).ToList()

             }).ToList();
            return Ok(bookingsOutput);
        }

        [HttpGet]
        [Route("booking/{pnr}")]
        [Authorize(Policy = "RequireUser")]
        public async Task<IActionResult> GetPNRBookings(string pnr, [FromServices] BookingRepository bookingRepository)
        {
            var bookings = bookingRepository.GetBookings().Where(b => b.PNR == pnr);
            List<BookingOutput> bookingsOutput = bookings.Select(b =>
             new BookingOutput
             {
                 BookingId = b.Id,
                 PNR = b.PNR,
                 ArraivalDate = b.Direction == Models.Direction.Up ? b.StartDate : b.ReturnDate.GetValueOrDefault(),
                 ArrivalTime = b.Flight.EndAt,
                 DepartureDate = b.Direction == Models.Direction.Up ? b.StartDate : b.ReturnDate.GetValueOrDefault(),
                 DepartureTime = b.Flight.StartAt,
                 EmailId = b.User.EmailId,
                 FlightNumer = b.Flight.FlightNumber,
                 Airline = b.Flight.Airline,
                 From = b.Flight.From,
                 To = b.Flight.To,
                 Direction = b.Direction,
                 Status = b.Status,
                 Passengers = b.Passengers.Select(p => new PassengerInput
                 {
                     Age = p.Age,
                     FirstName = p.FirstName,
                     LastName = p.LastName,
                     MealType = p.MealType,
                     SeatNumber = p.SeatNumber,
                 }).ToList()

             }).ToList();
            return Ok(bookingsOutput);
        }

        [HttpPost]
        [Route("book-flight")]
        [Authorize(Policy = "RequireUser")]
        public async Task<IActionResult> BookFlight(BookingInput bookingInput, [FromServices] UserRepository userRepository, [FromServices] FlightRepository flightRepository, [FromServices] BookingRepository bookingRepository)
        {
            var errors = new List<string>();
            var user = await userRepository.GetUser(u => u.EmailId == bookingInput.UserEmail);

            var fromFlight = await flightRepository.GetFlight(x => x.Id == bookingInput.FromFlightId);
            var returnFlight = await flightRepository.GetFlight(x => x.Id == bookingInput.ReturnFlightId);

            if (user == null)
            {
                errors.Add("User does not exists");
            }

            {
                if (fromFlight == null || fromFlight.IsBlocked)
                    errors.Add("From flight not avaialble or flight is blocked.");
            }

            if (returnFlight != null || returnFlight.IsBlocked)
            {
                errors.Add("Return flight is not available or flight is blocked.");
            }

            if (errors.Count > 0)
            {
                return BadRequest(errors);
            }
            var pnr = await bookingRepository.BookFlight(bookingInput);
            return Ok(pnr);
        }

        [HttpDelete]
        [Route("cancel/{pnr}")]
        [Authorize(Policy = "RequireUser")]
        public async Task<IActionResult> CancelBookinng(string pnr, [FromServices] BookingRepository bookingRepository)
        {
            await bookingRepository.CancelBooking(pnr);
            return Ok("Booking cancelled successfully");
        }
    }
}

