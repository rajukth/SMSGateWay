using System.ComponentModel.DataAnnotations;

namespace SMS.Models;

public class SmsMessage
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string Message { get; set; }

    public string Status { get; set; } = "Pending"; // Pending, Sent, Failed
    public string? Response { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? SentAt { get; set; }
}