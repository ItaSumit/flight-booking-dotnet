using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace FlightBooking.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailId { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        Admin = 1,
        User
    }
}
