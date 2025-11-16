using Microsoft.EntityFrameworkCore;
using SMSGateway.Base;

namespace SMSGateway.Base.DataContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.AddBase();
        base.OnModelCreating(builder);
    }
}