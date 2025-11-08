using Microsoft.EntityFrameworkCore;
using SMS.Models;

namespace SMS;

public static class EntityRegistrar
{
    public static ModelBuilder AddSMSGateway(this ModelBuilder builder)
    {
         builder.Entity<SmsMessage>();
         builder.Entity<SmsSetup>();
         builder.Entity<StartingNumber>();
        return builder; 
    }
}