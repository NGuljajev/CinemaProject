using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Dto
{
    public class PromotionDto
    {
        public int Id { get; set; }

        public int? EventId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? PromoCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string DiscountType { get; set; } // percent, amount

        [Required]
        [Range(0, double.MaxValue)]
        public decimal DiscountValue { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public int? UsageLimit { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
