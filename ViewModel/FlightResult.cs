using FlightBooking.Models;

namespace FlightBooking.ViewModel
{
    public class FlightResult
    {
        public string FlightNumber { get; set; }

        public string Airline { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

        public string Instrument { get; set; }

        public double Cost { get; set; }

        public MealType MealType { get; set; }
    }
}
