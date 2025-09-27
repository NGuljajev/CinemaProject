using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Dto
{
    public class RoleDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
