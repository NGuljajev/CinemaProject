using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaBackend.Data;
using CinemaProject.Models;
using CinemaProject.Dto;

namespace CinemaProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public OrderItemController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/orderitem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems()
        {
            var list = await _context.OrderItems
                .Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    OrderId = oi.OrderId,
                    ItemType = oi.ItemType,
                    ReferenceId = oi.ReferenceId,
                    Name = oi.Name,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity,
                    LineTotal = oi.LineTotal,
                    CreatedAt = oi.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/orderitem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemDto>> GetOrderItem(long id)
        {
            var oi = await _context.OrderItems.FindAsync(id);
            if (oi == null) return NotFound();

            var dto = new OrderItemDto
            {
                Id = oi.Id,
                OrderId = oi.OrderId,
                ItemType = oi.ItemType,
                ReferenceId = oi.ReferenceId,
                Name = oi.Name,
                UnitPrice = oi.UnitPrice,
                Quantity = oi.Quantity,
                LineTotal = oi.LineTotal,
                CreatedAt = oi.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/orderitem
        [HttpPost]
        public async Task<ActionResult<OrderItemDto>> CreateOrderItem([FromBody] OrderItemDto dto)
        {
            if (dto == null) return BadRequest();

            var entity = new OrderItem
            {
                OrderId = dto.OrderId,
                ItemType = dto.ItemType,
                ReferenceId = dto.ReferenceId,
                Name = dto.Name,
                UnitPrice = dto.UnitPrice,
                Quantity = dto.Quantity,
                LineTotal = dto.LineTotal,
                CreatedAt = DateTime.UtcNow
            };

            _context.OrderItems.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetOrderItem), new { id = dto.Id }, dto);
        }

        // PUT: api/orderitem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(long id, [FromBody] OrderItemDto dto)
        {
            if (dto == null || id != dto.Id) return BadRequest();

            var entity = await _context.OrderItems.FindAsync(id);
            if (entity == null) return NotFound();

            entity.OrderId = dto.OrderId;
            entity.ItemType = dto.ItemType;
            entity.ReferenceId = dto.ReferenceId;
            entity.Name = dto.Name;
            entity.UnitPrice = dto.UnitPrice;
            entity.Quantity = dto.Quantity;
            entity.LineTotal = dto.LineTotal;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/orderitem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(long id)
        {
            var entity = await _context.OrderItems.FindAsync(id);
            if (entity == null) return NotFound();

            _context.OrderItems.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
