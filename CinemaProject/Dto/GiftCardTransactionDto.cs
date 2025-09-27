public class GiftCardTransactionDto
{
    public long Id { get; set; }
    public long GiftCardId { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public int? PerformedBy { get; set; }
    public DateTime? PerformedAt { get; set; }
    public long? RelatedOrderId { get; set; }
    public string? Note { get; set; }
}

public class CreateGiftCardTransactionDto
{
    public long GiftCardId { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public int? PerformedBy { get; set; }
    public DateTime? PerformedAt { get; set; }
    public long? RelatedOrderId { get; set; }
    public string? Note { get; set; }
}
