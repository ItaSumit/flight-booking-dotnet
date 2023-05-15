namespace FlightBooking.Models
{
    public class Passenger
    {
        public int Id { get; set; }

        public int BookingId { get;set; }

        public virtual Booking Booking { get; set; }

        public string FirstName { get; set; }  
        
        public string LastName { get; set; }

        public  int Age { get; set; }

        public MealType MealType { get;set; }

        public int SeatNumber { get; set; }
    }
}
