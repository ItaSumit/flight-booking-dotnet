using FlightBooking.Models;
using System.ComponentModel.DataAnnotations;

namespace FlightBooking.ViewModel
{
    public class PassengerInput
    {
        [Required]
        public string FirstName { get; set; }

        [Required]

        public string LastName { get; set; }

        [Required]

        public int Age { get; set; }


        [Required]

        public MealType MealType { get; set; }


        [Required]

        public int SeatNumber { get; set; }
    }
}
