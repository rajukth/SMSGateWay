using Microsoft.EntityFrameworkCore;
using SMS.Models;

namespace SMSGateway.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<SmsMessage> SmsMessages { get; set; }
    public DbSet<SmsSetup> SmsSetups { get; set; }
    public DbSet<StartingNumber> StartingNumbers { get; set; }
}