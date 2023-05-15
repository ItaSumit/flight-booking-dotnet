using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FlightBooking.Models
{
    public class Flight
    {
        public int Id { get; set; }

        public string FlightNumber { get; set; }

        public string Airline { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public TimeOnly StartAt { get; set; }

        public TimeOnly EndAt { get; set; }

        //public string[] Days { get; set; }

        public string Days { get; set; }

        public string Instrument { get; set; }

        public int BusinessClassSeats { get; set; }

        public int NonBusinessClassSeats { get; set; }

        public int Rows { get; set; }

        public double Cost { get; set; }

        public bool IsVeg { get; set; }

        public bool IsNonVeg { get; set; }

        public bool IsBlocked { get; set; }

    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MealType
    {
        [Display(Name = "Vegeterian")]
        Veg = 1,

        [Display(Name = "Non Vegeterian")]
        NonVeg
    }
}
