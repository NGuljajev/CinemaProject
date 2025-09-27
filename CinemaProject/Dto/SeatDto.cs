using System;

namespace CinemaProject.Dto
{
    public class SeatDto
    {
        public int Id { get; set; }
        public int HallId { get; set; }
        public string? RowLabel { get; set; }
        public int? SeatNumber { get; set; }
        public string? SeatCode { get; set; }
        public string SeatType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
