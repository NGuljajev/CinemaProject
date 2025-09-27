using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Dto
{
    public class OrderDto
    {
        public long Id { get; set; }

        public int? UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string OrderReference { get; set; }

        public int? CinemaId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [MaxLength(100)]
        public string? PaymentId { get; set; }
    }
}
