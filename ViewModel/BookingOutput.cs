
using FlightBooking.Models;

namespace FlightBooking.ViewModel
{
    public class BookingOutput
    {
        public int BookingId { get; set; }

        public string Airline { get; set; }
        public string FlightNumer { get; set; }

        public string PNR { get; set; }
        public List<PassengerInput> Passengers { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public DateOnly DepartureDate { get; set; }

        public TimeOnly DepartureTime { get; set; }
        public DateOnly ArraivalDate { get; set; }

        public TimeOnly ArrivalTime { get; set; }

        public string EmailId { get; set; }

        public Direction Direction { get; set; }


        public Status Status { get; set; }
    }
}
