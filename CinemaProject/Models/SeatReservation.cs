using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("seat_reservations")]
public class SeatReservation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Required]
    [Column("screening_id")]
    public int ScreeningId { get; set; }

    [Column("seat_id")]
    public int? SeatId { get; set; }

    [Column("reserved_at")]
    public DateTime? ReservedAt { get; set; }

    [Required]
    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [Column("order_id")]
    public long? OrderId { get; set; }

    [Column("status")]
    public string Status { get; set; } // reserved, completed, expired, cancelled
}
