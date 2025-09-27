using System;

namespace CinemaProject.Dto
{
    public class SeatReservationDto
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public int ScreeningId { get; set; }
        public int? SeatId { get; set; }
        public DateTime? ReservedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public long? OrderId { get; set; }
        public string Status { get; set; }   // reserved, completed, expired, cancelled
    }
}
