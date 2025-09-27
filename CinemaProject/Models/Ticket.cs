using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("tickets")]
public class Ticket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("order_id")]
    public long OrderId { get; set; }

    [Required]
    [Column("screening_id")]
    public int ScreeningId { get; set; }

    [Column("seat_id")]
    public int? SeatId { get; set; }

    [Required]
    [Column("price")]
    public decimal Price { get; set; }

    [Column("ticket_type")]
    public string TicketType { get; set; } // adult, child, senior, student, vip

    [Column("status")]
    public string Status { get; set; } // active, cancelled, used

    [Column("qr_code")]
    public string? QrCode { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
