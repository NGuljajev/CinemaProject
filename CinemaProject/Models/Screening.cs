using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("screenings")]
public class Screening
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("cinema_id")]
    public int CinemaId { get; set; }

    [Required]
    [Column("hall_id")]
    public int HallId { get; set; }

    [Required]
    [Column("film_id")]
    public int FilmId { get; set; }

    [Required]
    [Column("start_at")]
    public DateTime StartAt { get; set; }

    [Required]
    [Column("end_at")]
    public DateTime EndAt { get; set; }

    [Required]
    [Column("price")]
    public decimal Price { get; set; }

    [Column("language")]
    public string? Language { get; set; }

    [Column("format")]
    public string Format { get; set; } // 2D, 3D, IMAX, 4DX, Other

    [Column("seats_total")]
    public int SeatsTotal { get; set; }

    [Column("seats_available")]
    public int SeatsAvailable { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
