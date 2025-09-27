using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.Models
{
    [Table("ads")]
    public class Ad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("cinema_id")]
        public long? CinemaId { get; set; }

        [Required]
        [Column("title", TypeName = "varchar(200)")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [Column("client_name", TypeName = "varchar(200)")]
        [MaxLength(200)]
        public string ClientName { get; set; }

        [Required]
        [Column("media_url", TypeName = "varchar(500)")]
        [MaxLength(500)]
        public string MediaUrl { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("placement", TypeName = "varchar(500)")]
        [MaxLength(500)]
        public string Placement { get; set; }

        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column("status", TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string Status { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
