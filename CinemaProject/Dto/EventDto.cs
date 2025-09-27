using System;

namespace CinemaProject.Dto
{
    public class EventDto
    {
        public int Id { get; set; }
        public int? CinemaId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
