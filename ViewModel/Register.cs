using System.ComponentModel.DataAnnotations;

namespace FlightBooking.ViewModel
{
    public class Register
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailId { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
