using System;

namespace CinemaProject.Dto
{
    public class ScreeningDto
    {
        public int Id { get; set; }
        public int CinemaId { get; set; }
        public int HallId { get; set; }
        public int FilmId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public decimal Price { get; set; }
        public string? Language { get; set; }
        public string Format { get; set; }    // 2D, 3D, IMAX, 4DX, Other
        public int SeatsTotal { get; set; }
        public int SeatsAvailable { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
