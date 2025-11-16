using Microsoft.EntityFrameworkCore;
using SMS.Models;

namespace SMSGateway.Base;

public static class EntityRegistrar
{
    public static ModelBuilder AddBase(this ModelBuilder builder)
    {
         //builder.Entity<SourceType>();
         builder.Entity<SmsMessage>();
         builder.Entity<SmsSetup>();
         builder.Entity<StartingNumber>();
        return builder; 
    }
}