using SMSGateway.Base.GenericRepository;
using SMS.Models;
using SMSGateway.Base.DataContext;
using SMSGateway.Repositories.Interfaces;

namespace SMSGateway.Repositories;

public class SMSSetupRepository:GenericRepository<SmsSetup>,ISMSSetupRepository
{
    public SMSSetupRepository(AppDbContext context) : base(context)
    {
    }
}