using Microsoft.AspNetCore.Mvc;
using SMS.Models;
using SMSGateway.Base.DataContext.Interface;
using SMSGateway.Data;
using SMSGateway.Repositories.Interfaces;
using SMSGateway.ViewModels;

namespace SMSGateway.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _context;
    private readonly ISMSSetupRepository _smsSetupRepository;
    private readonly IUow _uow;

    public AdminController(AppDbContext context, ISMSSetupRepository smsSetupRepository, IUow uow)
    {
        _context = context;
        _smsSetupRepository = smsSetupRepository;
        _uow = uow;
    }

    // GET: /Admin/Settings
    public IActionResult Settings()
    {
        var settings = _smsSetupRepository.GetBaseQueryable().ToList();
        return View(new SmsSetupVm(){Setups=settings});
    }

    // POST: /Admin/Settings
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(SmsSetupVm vm)
    {
        var model = new SmsSetup()
        {
            TemplateName = vm.TemplateName,
            CountryCode = vm.CountryCode,
            Header = vm.Header,
            Footer = vm.Footer
        };
        await _uow.CreateAsync(model);
        await _uow.SaveChangesAsync();
        ViewBag.Message = "✅ Settings updated successfully!";
        return RedirectToAction("Settings");
    }
}