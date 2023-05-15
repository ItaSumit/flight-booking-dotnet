using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FlightBooking.ViewModel
{
    public class FlightInput
    {
        [Required]
        public string FlightNumber { get; set; }

        [Required]
        public string Airline { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        /// <summary>
        /// Mention time only i.e. 11:00
        /// </summary>
        /// <example>11:00</example>
        public TimeOnly StartAt { get; set; }

        [Required]
        /// <summary>
        /// Mention time only i.e. 11:00
        /// </summary>
        /// <example>13:00</example>
        public TimeOnly EndAt { get; set; }

        [Required]
        [MinLength(1)]
        /// <summary>
        /// Mention days in array
        /// </summary>
        /// <example>[Monday, Tuesday, Wednesday...]</example>
        public string[] Days { get; set; }

        [Required]
        public string Instrument { get; set; }

        [Required]
        public int BusinessClassSeats { get; set; }

        [Required]
        public int NonBusinessClassSeats { get; set; }

        [Required]
        public int Rows { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public bool IsVeg { get; set; }

        [Required]
        public bool IsNonVeg { get; set; }
    }
}
