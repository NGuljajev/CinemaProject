using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("payments")]
public class Payment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("order_id")]
    public long OrderId { get; set; }

    [Required]
    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("currency")]
    public string? Currency { get; set; }

    [Column("method")]
    public string? Method { get; set; }

    [Column("provider_payment_id")]
    public string? ProviderPaymentId { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("paid_at")]
    public DateTime? PaidAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
