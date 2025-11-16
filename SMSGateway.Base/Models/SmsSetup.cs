using System.ComponentModel.DataAnnotations;

namespace SMS.Models;

public class SmsSetup
{
    [Key] public int Id { get; set; }
    [Required, StringLength(20)] public string TemplateName { get; set; }
    [Required, StringLength(10)] public string CountryCode { get; set; } = "+977";

    [Required, StringLength(50)] public string Header { get; set; } = "[MyCompany]";

    [StringLength(100)] public string Footer { get; set; } = "-- Thank you, MyCompany SMS Gateway";
}