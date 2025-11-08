using Microsoft.EntityFrameworkCore;

namespace SMSGateway.Base;

public static class EntityRegistrar
{
    public static ModelBuilder AddBase(this ModelBuilder builder)
    {
         //builder.Entity<SourceType>();
        return builder; 
    }
}