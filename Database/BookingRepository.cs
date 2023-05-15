using FlightBooking.Models;
using FlightBooking.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.Database
{
    public class BookingRepository
    {
        private readonly FlightDbContext _flightDbContext;
        public BookingRepository(FlightDbContext flightDbContext)
        {
            this._flightDbContext = flightDbContext;
        }

        public IQueryable<Booking> GetBookings()
        {
            return _flightDbContext.Bookings;
        }




        public async Task<string> BookFlight(BookingInput bookingInput)
        {
            var fromFlight = await _flightDbContext.Flights.FindAsync(bookingInput.FromFlightId);
            var user = await _flightDbContext.Users.FirstOrDefaultAsync(u => u.EmailId == bookingInput.UserEmail);
            var tripTypeCode = bookingInput.TripType == TripType.OneWay ? "OW" : "RT";
            var pnr =
                $"{fromFlight.From}-{fromFlight.To}-{tripTypeCode}-{Guid.NewGuid().ToString().Split("-")[0].ToUpper()}";

            var fromBooking = new Booking
            {
                FlightId = bookingInput.FromFlightId,
                User = user,
                StartDate = bookingInput.FromTravelDate,
                TripType = bookingInput.TripType,
                ReturnDate = bookingInput.ReturnTravelDate,
                Passengers =
                bookingInput.Passengers.
                Select(p => new Passenger
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Age = p.Age,
                    MealType = p.MealType,
                    SeatNumber = p.SeatNumber,
                }).ToList(),
                PNR = pnr,
                Direction = Direction.Up,
                Status = Status.Confirmed
            };

            _flightDbContext.Bookings.Add(fromBooking);


            if (bookingInput.TripType == TripType.RoundTrip && bookingInput.ReturnTravelDate != null && bookingInput.ReturnFlightId != null)
            {
                var returnFlight = await _flightDbContext.Flights.FindAsync(bookingInput.ReturnFlightId);


                var returnBooking = new Booking
                {
                    FlightId = bookingInput.ReturnFlightId.GetValueOrDefault(),
                    User = user,
                    StartDate = bookingInput.FromTravelDate,
                    TripType = bookingInput.TripType,
                    ReturnDate = bookingInput.ReturnTravelDate,
                    Passengers = bookingInput.Passengers.Select(p => new Passenger
                    {
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Age = p.Age,
                        MealType = p.MealType,
                        SeatNumber = p.SeatNumber,
                    }).ToList(),
                    PNR = pnr,
                    Direction = Direction.Down,
                    Status = Status.Confirmed
                };

                _flightDbContext.Bookings.Add(returnBooking);
            }
            await _flightDbContext.SaveChangesAsync();
            return pnr;
        }


        public async Task CancelBooking(string pnr)
        {
            var bookings = _flightDbContext.Bookings.Where(b => b.PNR == pnr).ToList();

            foreach (var booking in bookings)
            {
                booking.Status = Status.Cancelled;
            }
            await _flightDbContext.SaveChangesAsync();
        }
    }
}
