using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SMS.Constants;
using SMSGateway.Base.DataContext.Interface;
using SMSGateway.Hubs;
using SMSGateway.Repositories.Interfaces;

namespace SMSGateway.Controllers;

[Route("api/sms")]
[ApiController]
public class ApiSmsController : ControllerBase
{
    private readonly IHubContext<SmsHub> _hub;
    private readonly ISMSMessageRepository _smsMessageRepository;
    private readonly IUow _uow;

    public ApiSmsController(IHubContext<SmsHub> hub, ISMSMessageRepository smsMessageRepository, IUow uow)
    {
        _hub = hub;
        _smsMessageRepository = smsMessageRepository;
        _uow = uow;
    }

    [HttpGet("pending")]
    public IActionResult GetPending()
    {
        var pending = _smsMessageRepository.GetBaseQueryable()
            .Where(s => s.Status == SmsStatus.Pending)
            .OrderBy(s => s.CreatedAt)
            .Take(5)
            .ToList();
        return Ok(pending);
    }

    [HttpPost("update-status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateSmsStatusDto dto)
    {
        var sms = await _smsMessageRepository.FindAsync(dto.Id);
        if (sms == null) return NotFound();

        sms.Status = dto.Status;
        sms.Response = dto.Response;
        sms.SentAt = DateTime.Now;
        await _uow.CreateAsync(sms);
        await _uow.SaveChangesAsync();

        await _hub.Clients.All.SendAsync("ReceiveStatusUpdate", sms.Id, sms.Status,sms.SentAt, sms.Response);
        return Ok();
    }

    public class UpdateSmsStatusDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Response { get; set; }
    }
}