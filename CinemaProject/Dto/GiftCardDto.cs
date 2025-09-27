

public class GiftCardDto
{
    public long Id { get; set; }
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
    public string? Currency { get; set; }
    public int? IssuedByUser { get; set; }
    public string? IssuedToEmail { get; set; }
    public string? Message { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateGiftCardDto
{
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
    public string? Currency { get; set; }
    public int? IssuedByUser { get; set; }
    public string? IssuedToEmail { get; set; }
    public string? Message { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
}
