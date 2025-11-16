namespace SMS.Models;

public class StartingNumber
{
    public int Id { get; set; }
    public long StartingNo { get; set; } = 0;
    public int TypeId { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public string Status { get; set; } = "A";
    public long StartFrom { get; set; }
    public long? EndAt { get; set; }
    public long? NoOfDigit { get; set; }
}