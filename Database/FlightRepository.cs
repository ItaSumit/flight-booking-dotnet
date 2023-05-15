
using FlightBooking.Models;
using FlightBooking.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlightBooking.Database
{
    public class FlightRepository
    {
        private readonly FlightDbContext _flightDbContext;
        public FlightRepository(FlightDbContext flightDbContext)
        {
            this._flightDbContext = flightDbContext;
        }

        public async Task<List<Flight>> GetAllFlights()
        {
            var flights = await _flightDbContext.Flights.ToListAsync();
            return flights;
        }

        public async Task<Flight> GetFlight(Expression<Func<Flight, bool>> filter)
        {
            var flights = await _flightDbContext.Flights.Where(filter).ToListAsync();
            return flights.FirstOrDefault();
        }

        public async Task<List<FlightResult>> SearchFlights(FlightSearchCriteria criteria)
        {
            var fromTravelDay = criteria.FromTravelDate.DayOfWeek.ToString();

            string returnTravelDay = "";
            if (criteria.TripType == TripType.RoundTrip)
            {
                returnTravelDay = criteria.ReturnTravelDate.GetValueOrDefault().DayOfWeek.ToString();
            }

            var flights = await _flightDbContext.Flights.Where(f =>
            !f.IsBlocked &&
            (criteria.MealType == MealType.Veg && f.IsVeg || criteria.MealType == MealType.NonVeg && f.IsNonVeg)
            && (f.From == criteria.From && f.To == criteria.To && f.Days.Contains(fromTravelDay)
            || (criteria.TripType == TripType.RoundTrip && returnTravelDay != ""
                && f.From == criteria.To && f.To == criteria.From && f.Days.Contains(returnTravelDay)))).ToListAsync();


            var flightResults = new List<FlightResult>();

            flights.ForEach(flight =>
            {
                var flightResult = new FlightResult
                {
                    FlightNumber = flight.FlightNumber,
                    Airline = flight.Airline,
                    From = flight.From,
                    To = flight.To,
                    Instrument = flight.Instrument,
                    MealType = criteria.MealType,
                    Departure = flight.From == criteria.From ? criteria.FromTravelDate.ToDateTime(flight.StartAt) : criteria.ReturnTravelDate.GetValueOrDefault().ToDateTime(flight.StartAt),
                    Arrival = flight.From == criteria.From ? criteria.FromTravelDate.ToDateTime(flight.EndAt) : criteria.ReturnTravelDate.GetValueOrDefault().ToDateTime(flight.EndAt),
                    Cost = flight.Cost
                };
                flightResults.Add(flightResult);
            });
            return flightResults;
        }


        public async Task AddFlight(FlightInput flightInput)
        {
            var flight = new Flight
            {
                Airline = flightInput.Airline,
                FlightNumber = flightInput.FlightNumber,
                From = flightInput.From,
                To = flightInput.To,
                StartAt = flightInput.StartAt,
                EndAt = flightInput.EndAt,
                Instrument = flightInput.Instrument,
                IsNonVeg = flightInput.IsNonVeg,
                IsVeg = flightInput.IsVeg,
                Cost = flightInput.Cost,
                Days = string.Join(";", flightInput.Days),
            };

            _flightDbContext.Flights.Add(flight);
            await _flightDbContext.SaveChangesAsync();
        }

        public async Task BlockFlight(int flightId, bool isBlokced)
        {
            var flight = _flightDbContext.Find<Flight>(flightId);
            flight.IsBlocked = isBlokced;

            await _flightDbContext.SaveChangesAsync();
        }
    }
}
