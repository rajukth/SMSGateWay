using Microsoft.AspNetCore.Mvc;
using SMS.Models;
using SMSGateway.Data;
using SMSGateway.ViewModels;

namespace SMSGateway.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /Admin/Settings
    public IActionResult Settings()
    {
        var settings = _context.SmsSetups.ToList();
        return View(new SmsSetupVm(){Setups=settings});
    }

    // POST: /Admin/Settings
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Settings(SmsSetupVm vm)
    {
        var model = new SmsSetup()
        {
            TemplateName = vm.TemplateName,
            CountryCode = vm.CountryCode,
            Header = vm.Header,
            Footer = vm.Footer
        };
            _context.SmsSetups.Add(model);
        

        _context.SaveChanges();
        ViewBag.Message = "✅ Settings updated successfully!";
        return RedirectToAction("Settings");
    }
}