using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("films")]
public class Film
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; }

    [Column("original_title")]
    public string? OriginalTitle { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("duration_minutes")]
    public int? DurationMinutes { get; set; }

    [Column("release_date")]
    public DateTime? ReleaseDate { get; set; }

    [Column("age_rating")]
    public string? AgeRating { get; set; }

    [Column("language")]
    public string? Language { get; set; }

    [Column("genres")]
    public string? Genres { get; set; }

    [Column("director")]
    public string? Director { get; set; }

    [Column("cast")]
    public string? Cast { get; set; }

    [Column("poster_url")]
    public string? PosterUrl { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}
