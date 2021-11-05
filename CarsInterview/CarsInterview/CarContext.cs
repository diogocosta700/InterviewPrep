using Microsoft.EntityFrameworkCore;

namespace CarsInterview
{
    public class CarContext : DbContext
    {
        public CarContext(DbContextOptions<CarContext> options)
            : base(options)
        {
        }
 
        public DbSet<Car> Cars { get; set; }
        
        public DbSet<Client> Clients { get; set; }
    }
}