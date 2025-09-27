using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("seats")]
public class Seat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("hall_id")]
    public int HallId { get; set; }

    [Column("row_label")]
    public string? RowLabel { get; set; }

    [Column("seat_number")]
    public int? SeatNumber { get; set; }

    [Column("seat_code")]
    public string? SeatCode { get; set; }

    [Column("seat_type")]
    public string SeatType { get; set; } // standard, vip, couple, handicap

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
