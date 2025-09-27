using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Dto
{
    public class OrderItemDto
    {
        public long Id { get; set; }

        [Required]
        public long OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ItemType { get; set; } // ticket, food, gift_card

        public long? ReferenceId { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal LineTotal { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
