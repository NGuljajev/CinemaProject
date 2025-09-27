using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("orders")]
public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Required]
    [Column("order_reference")]
    public string OrderReference { get; set; }

    [Column("cinema_id")]
    public int? CinemaId { get; set; }

    [Required]
    [Column("total_amount")]
    public decimal TotalAmount { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("payment_method")]
    public string? PaymentMethod { get; set; }

    [Column("payment_id")]
    public string? PaymentId { get; set; }
}
