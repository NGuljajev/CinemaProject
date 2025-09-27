using System;

namespace CinemaProject.Dto
{
    public class HallDto
    {
        public int Id { get; set; }
        public int CinemaId { get; set; }
        public string Name { get; set; }
        public int SeatCount { get; set; }
        public string? Description { get; set; }
        public bool Is3D { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
