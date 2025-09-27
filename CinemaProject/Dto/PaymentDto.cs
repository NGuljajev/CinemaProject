using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Dto
{
    public class PaymentDto
    {
        public long Id { get; set; }

        [Required]
        public long OrderId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [MaxLength(10)]
        public string? Currency { get; set; }

        [MaxLength(50)]
        public string? Method { get; set; }

        [MaxLength(255)]
        public string? ProviderPaymentId { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
