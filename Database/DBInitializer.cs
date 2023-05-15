using FlightBooking.Models;

namespace FlightBooking.Database
{
    public class DBInitializer
    {
        public static void InIt(FlightDbContext dbContext)
        {
            dbContext.Flights.AddRange(new Flight
            {
                Id = 1,
                Airline = "AirIndia",
                FlightNumber = "AI001",
                IsNonVeg = true,
                IsVeg = true,
                From = "BLR",
                To = "DEL",
                Cost = 5000.00,
                BusinessClassSeats = 100,
                NonBusinessClassSeats = 200,
                StartAt = new TimeOnly(11, 0),
                EndAt = new TimeOnly(13, 0),
                Days = "Monday;Tuesday",// new string[] { "Monday", "Tuesday" },
                Instrument = "B-747",
                Rows = 100,
            }, new Flight
            {
                Id = 2,
                Airline = "AirIndia",
                FlightNumber = "AI002",
                IsNonVeg = true,
                IsVeg = true,
                From = "DEL",
                To = "BLR",
                Cost = 5000.00,
                BusinessClassSeats = 100,
                NonBusinessClassSeats = 200,
                StartAt = new TimeOnly(11, 0),
                EndAt = new TimeOnly(13, 0),
                Days = "Monday;Tuesday",// new string[] { "Monday", "Tuesday" },
                Instrument = "B-747",
                Rows = 100,

            }); ;

            dbContext.Users.Add(new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Admin",
                EmailId = "Admin@gagan.com",
                Password = "admin@1234",
                Role = Role.Admin
            });

            dbContext.SaveChanges();
        }
    }
}
