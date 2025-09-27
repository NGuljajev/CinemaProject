using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Dto
{
    public class FoodItemDto
    {
        public int Id { get; set; }

        public int? CinemaId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsVegetarian { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
