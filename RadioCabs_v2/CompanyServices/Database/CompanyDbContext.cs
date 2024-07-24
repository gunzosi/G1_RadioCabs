using Microsoft.EntityFrameworkCore;

namespace CompanyServices.Database;

public class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
    {
    }
    
    
}