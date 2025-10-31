using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS.Models;

namespace SMSGateway.ViewModels;

public class SmsIndexVm
{
    [Required]
    [RegularExpression(@"^(?:\+977)?(97|98)\d{8}$", ErrorMessage = "Enter a valid Nepalese mobile number.")]
    public string PhoneNumber { get; set; }

    [Required]
    public string Message { get; set; }
    public IEnumerable<SmsMessage> SmsMessages { get; set; }
    public SelectList SmsTemplates { get; set; }
    public int TemplateId { get; set; }
}