using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SMS.Models;
using SMSGateway.Data;
using SMSGateway.Hubs;

namespace SMSGateway.Controllers;

[Route("api/sms")]
[ApiController]
public class ApiSmsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHubContext<SmsHub> _hub;

    public ApiSmsController(AppDbContext context, IHubContext<SmsHub> hub)
    {
        _context = context;
        _hub = hub;
    }

    [HttpGet("pending")]
    public IActionResult GetPending()
    {
        var pending = _context.SmsMessages
            .Where(s => s.Status == "Pending")
            .OrderBy(s => s.CreatedAt)
            .Take(5)
            .ToList();
        return Ok(pending);
    }

    [HttpPost("update-status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateSmsStatusDto dto)
    {
        var sms = await _context.SmsMessages.FindAsync(dto.Id);
        if (sms == null) return NotFound();

        sms.Status = dto.Status;
        sms.Response = dto.Response;
        sms.SentAt = DateTime.Now;
        await _context.SaveChangesAsync();

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