using FlightBooking.Models;
using FlightBooking.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlightBooking.Database
{
    public class UserRepository
    {
        private readonly FlightDbContext _flightDbContext;
        public UserRepository(FlightDbContext flightDbContext)
        {
            this._flightDbContext = flightDbContext;
        }
        public async Task<List<User>> GetAllUsers()
        {
            var users = await _flightDbContext.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUser(Expression<Func<User, bool>> filter)
        {
            var users = await _flightDbContext.Users.Where(filter).ToListAsync();
            return users.FirstOrDefault();
        }

        public async Task AddUser(Register user)
        {
            var dbUser = new User { FirstName = user.FirstName, LastName = user.LastName, EmailId = user.EmailId, Password = user.Password, Role = Role.User };
            _flightDbContext.Users.Add(dbUser);
            await _flightDbContext.SaveChangesAsync();
        }

        public async Task<User?> LoginUser(Login login)
        {
            return await _flightDbContext.Users.FirstOrDefaultAsync(u => u.EmailId == login.Email && u.Password == login.Password);
        }
    }
}
