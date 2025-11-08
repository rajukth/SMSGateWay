using SMSGateway.Base.GenericRepository;
using Microsoft.EntityFrameworkCore;
using SMS.Models;
using SMSGateway.Repositories.Interfaces;

namespace SMSGateway.Repositories;

public class SMSMessageRepository:GenericRepository<SmsMessage>,ISMSMessageRepository
{
    public SMSMessageRepository(DbContext context) : base(context)
    {
    }
}