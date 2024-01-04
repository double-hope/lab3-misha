using Hotel.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.DAL.DbSeeder
{
    public class DbSeeder : IDbSeeder
    {
        private readonly HotelDataContext _context;
        public DbSeeder(HotelDataContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            try
            {
                if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
