using FlightBooking.Models;
using System.ComponentModel.DataAnnotations;

namespace FlightBooking.ViewModel
{
    public class BookingInput
    {
        [Required]
        public int FromFlightId { get; set; }

        public int? ReturnFlightId { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public List<PassengerInput> Passengers { get; set; }

        [Required]
        public DateOnly FromTravelDate { get; set; }

        [Required]
        public TripType TripType { get; set; }  

        public DateOnly? ReturnTravelDate { get; set; }
    }
}
