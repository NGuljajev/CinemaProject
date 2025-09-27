using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("halls")]
public class Hall
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("cinema_id")]
    public int CinemaId { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Column("seat_count")]
    public int SeatCount { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("is_3d")]
    public bool Is3D { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}
