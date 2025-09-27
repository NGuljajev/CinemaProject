using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("gift_card_transactions")]
public class GiftCardTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("gift_card_id")]
    public long GiftCardId { get; set; }

    [Required]
    [Column("type")]
    public string Type { get; set; } // issue, redeem, topup, transfer

    [Required]
    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("performed_by")]
    public int? PerformedBy { get; set; }

    [Column("performed_at")]
    public DateTime? PerformedAt { get; set; }

    [Column("related_order_id")]
    public long? RelatedOrderId { get; set; }

    [Column("note")]
    public string? Note { get; set; }
}
