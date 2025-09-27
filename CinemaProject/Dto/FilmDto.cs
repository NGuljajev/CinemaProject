using System;

namespace CinemaProject.Dto
{
    public class FilmDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Description { get; set; }
        public int? DurationMinutes { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string AgeRating { get; set; }
        public string Language { get; set; }
        public string Genres { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public string PosterUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
