using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS.Models;
using SMSGateway.Data;
using SMSGateway.ViewModels;

namespace SMSGateway.Controllers;
public class SmsController : Controller
{
    private readonly AppDbContext _context;

    public SmsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var messages = _context.SmsMessages.OrderByDescending(m => m.CreatedAt).ToList();
        var smsTemplateList = new SelectList(_context.SmsSetups.ToList(),"Id","TemplateName");
        return View(new SmsIndexVm(){SmsMessages = messages,SmsTemplates=smsTemplateList});
    }

    [HttpPost]
    public IActionResult Send(SmsIndexVm vm)
    {
        if (string.IsNullOrWhiteSpace(vm.PhoneNumber) || string.IsNullOrWhiteSpace(vm.Message))
            return BadRequest("Phone number and message are required.");

        // Normalize Nepalese number
        vm.PhoneNumber = NormalizeNepalNumber(vm.PhoneNumber);

        // Add header/footer
        string finalMessage = ApplyMessageTemplate(vm);

        var sms = new SmsMessage
        {
            PhoneNumber = vm.PhoneNumber,
            Message = finalMessage,
            Status = "Pending"
        };
        _context.SmsMessages.Add(sms);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    private string ApplyMessageTemplate(SmsIndexVm vm)
    {
        var settings = _context.SmsSetups.FirstOrDefault(x=>x.Id==vm.TemplateId);

        string header = settings?.Header ?? "";
        string footer = settings?.Footer ?? "";
        string final = $"{header} {vm.Message} {footer}".Trim();

        return final;
    }
    private string NormalizeNepalNumber(string number)
    {
        number = number.Trim();

        // Remove any spaces or dashes
        number = number.Replace(" ", "").Replace("-", "");

        if (number.StartsWith("+977"))
            return number;

        if (number.StartsWith("977"))
            return "+" + number;

        // Add +977 if missing and number starts with 97/98
        if (number.StartsWith("97") || number.StartsWith("98"))
            return "+977" + number;

        return number; // fallback (invalid but unchanged)
    }
}