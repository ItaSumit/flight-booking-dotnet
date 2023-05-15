using System.Text.Json.Serialization;

namespace FlightBooking.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public string PNR { get; set; } 

        public int FlightId { get; set; }

        public virtual Flight Flight { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public DateOnly StartDate { get; set; }

        public TripType TripType { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public ICollection<Passenger> Passengers { get; set; } 

        public Direction Direction { get; set; }

        public Status Status { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TripType
    {
        OneWay = 1,
        RoundTrip
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Direction
    {
        Up = 1,
        Down
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        Confirmed = 1,
        Cancelled
    }
}
