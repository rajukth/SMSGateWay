using SMSGateway.Base.GenericRepository;
using SMS.Models;
using SMSGateway.Base.DataContext;
using SMSGateway.Repositories.Interfaces;

namespace SMSGateway.Repositories;

public class SMSMessageRepository:GenericRepository<SmsMessage>,ISMSMessageRepository
{
    public SMSMessageRepository(AppDbContext context) : base(context)
    {
    }
}