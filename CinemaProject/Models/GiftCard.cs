using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("gift_cards")]
public class GiftCard
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("code")]
    public string Code { get; set; }

    [Required]
    [Column("amount")]
    public decimal Amount { get; set; }

    [Required]
    [Column("balance")]
    public decimal Balance { get; set; }

    [Column("currency")]
    public string? Currency { get; set; }

    [Column("issued_by_user")]
    public int? IssuedByUser { get; set; }

    [Column("issued_to_email")]
    public string? IssuedToEmail { get; set; }

    [Column("message")]
    public string? Message { get; set; }

    [Column("expires_at")]
    public DateTime? ExpiresAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
