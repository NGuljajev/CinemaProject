using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Dto
{
    public class TicketDto
    {
        public long Id { get; set; }

        [Required]
        public long OrderId { get; set; }

        [Required]
        public int ScreeningId { get; set; }

        public int? SeatId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string? TicketType { get; set; } // adult, child, senior, student, vip

        [MaxLength(50)]
        public string? Status { get; set; } // active, cancelled, used

        [MaxLength(500)]
        public string? QrCode { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
