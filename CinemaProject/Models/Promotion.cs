using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("promotions")]
public class Promotion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("event_id")]
    public int? EventId { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("promo_code")]
    public string? PromoCode { get; set; }

    [Column("discount_type")]
    public string DiscountType { get; set; } // percent, amount

    [Column("discount_value")]
    public decimal DiscountValue { get; set; }

    [Column("valid_from")]
    public DateTime? ValidFrom { get; set; }

    [Column("valid_to")]
    public DateTime? ValidTo { get; set; }

    [Column("usage_limit")]
    public int? UsageLimit { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
