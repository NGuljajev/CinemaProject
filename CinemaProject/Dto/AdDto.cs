using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Dto
{
    public class AdDto
    {
        public long Id { get; set; }

        public long? CinemaId { get; set; } // optional foreign key

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } //s

        [Required]
        [MaxLength(255)]
        public string ClientName { get; set; }

        [Required]
        [MaxLength(500)]
        public string MediaUrl { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string Placement { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; } // Usually set by DB, not required in POST
    }
}
