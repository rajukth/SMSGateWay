using SMS.Models;

namespace SMSGateway.ViewModels;

public class SmsSetupVm
{
    public List<SmsSetup> Setups { get; set; }
    public string CountryCode { get; set; }
    public string Header { get; set; }
    public string Footer { get; set; }
    public string TemplateName { get; set; }
}