using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("order_items")]
public class OrderItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("order_id")]
    public long OrderId { get; set; }

    [Required]
    [Column("item_type")]
    public string ItemType { get; set; } // ticket, food, gift_card

    [Column("reference_id")]
    public long? ReferenceId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Required]
    [Column("unit_price")]
    public decimal UnitPrice { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("line_total")]
    public decimal LineTotal { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
