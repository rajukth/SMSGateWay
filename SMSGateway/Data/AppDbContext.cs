using Microsoft.EntityFrameworkCore;
using SMS;
using SMS.Models;
using SMSGateway.Base;

namespace SMSGateway.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.AddSMSGateway();
        builder.AddBase();
        base.OnModelCreating(builder);
    }
}