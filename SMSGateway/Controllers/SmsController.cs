using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS.Constants;
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

    public IActionResult Index(SmsIndexVm vm)
    {
        vm.SmsMessages = _context.SmsMessages.OrderByDescending(m => m.CreatedAt).ToList();
        vm.SmsTemplates = _context.SmsSetups.ToList();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Send(SmsIndexVm vm)
    {
        if (string.IsNullOrWhiteSpace(vm.PhoneNumber) || string.IsNullOrWhiteSpace(vm.Message))
        {
            ViewBag.Messages="Phone number or message cannot be empty";
            return RedirectToAction(nameof(Index), vm);
        }
        var phoneNumbers = vm.PhoneNumber?
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(n => NormalizeNepalNumber(n.Trim()))
            .Distinct()
            .ToList();
        // Normalize Nepalese number
        //vm.PhoneNumber = NormalizeNepalNumber(vm.PhoneNumber);

        // Add header/footer
        string finalMessage = ApplyMessageTemplate(vm);
        var smsList = phoneNumbers.Select(phoneNumber => new SmsMessage()
        {
            PhoneNumber = phoneNumber,
            Message = finalMessage,
            Status = SmsStatus.Pending
        }).ToList();
        await _context.SmsMessages.AddRangeAsync(smsList);
        await _context.SaveChangesAsync();
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