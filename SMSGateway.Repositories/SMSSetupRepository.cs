using SMSGateway.Base.GenericRepository;
using Microsoft.EntityFrameworkCore;
using SMS.Models;
using SMSGateway.Repositories.Interfaces;

namespace SMSGateway.Repositories;

public class SMSSetupRepository:GenericRepository<SmsSetup>,ISMSSetupRepository
{
    public SMSSetupRepository(DbContext context) : base(context)
    {
    }
}