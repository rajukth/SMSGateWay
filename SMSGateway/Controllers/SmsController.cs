using Microsoft.AspNetCore.Mvc;
using SMS.Constants;
using SMS.Models;
using SMSGateway.Base.DataContext.Interface;
using SMSGateway.Data;
using SMSGateway.Repositories.Interfaces;
using SMSGateway.ViewModels;

namespace SMSGateway.Controllers;
public class SmsController : Controller
{
    private readonly AppDbContext _context;
    private readonly ISMSSetupRepository _smsSetupRepository;
    private readonly ISMSMessageRepository _smsMessageRepository;
    private readonly IUow _uow;

    public SmsController(AppDbContext context, ISMSSetupRepository smsSetupRepository, ISMSMessageRepository smsMessageRepository, IUow uow)
    {
        _context = context;
        _smsSetupRepository = smsSetupRepository;
        _smsMessageRepository = smsMessageRepository;
        _uow = uow;
    }

    public IActionResult Index(SmsIndexVm vm)
    {
        vm.SmsMessages = _smsMessageRepository.GetBaseQueryable().OrderByDescending(m => m.CreatedAt).ToList();
        vm.SmsTemplates = _smsSetupRepository.GetBaseQueryable().ToList();
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
        await _uow.CreateMultipleAsync(smsList);
        await _uow.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    private string ApplyMessageTemplate(SmsIndexVm vm)
    {
        var settings = _smsSetupRepository.GetBaseQueryable().FirstOrDefault(x=>x.Id==vm.TemplateId);

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