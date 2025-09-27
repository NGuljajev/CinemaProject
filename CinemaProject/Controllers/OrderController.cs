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
    public class OrderController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public OrderController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var list = await _context.Orders
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderReference = o.OrderReference,
                    CinemaId = o.CinemaId,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                    PaymentMethod = o.PaymentMethod,
                    PaymentId = o.PaymentId
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(long id)
        {
            var o = await _context.Orders.FindAsync(id);
            if (o == null) return NotFound();

            var dto = new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                OrderReference = o.OrderReference,
                CinemaId = o.CinemaId,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt,
                PaymentMethod = o.PaymentMethod,
                PaymentId = o.PaymentId
            };

            return Ok(dto);
        }

        // POST: api/order
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderDto dto)
        {
            if (dto == null) return BadRequest();

            var entity = new Order
            {
                UserId = dto.UserId,
                OrderReference = dto.OrderReference,
                CinemaId = dto.CinemaId,
                TotalAmount = dto.TotalAmount,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,
                PaymentMethod = dto.PaymentMethod,
                PaymentId = dto.PaymentId
            };

            _context.Orders.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetOrder), new { id = dto.Id }, dto);
        }

        // PUT: api/order/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(long id, [FromBody] OrderDto dto)
        {
            if (dto == null || id != dto.Id) return BadRequest();

            var entity = await _context.Orders.FindAsync(id);
            if (entity == null) return NotFound();

            entity.UserId = dto.UserId;
            entity.OrderReference = dto.OrderReference;
            entity.CinemaId = dto.CinemaId;
            entity.TotalAmount = dto.TotalAmount;
            entity.Status = dto.Status;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.PaymentMethod = dto.PaymentMethod;
            entity.PaymentId = dto.PaymentId;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            var entity = await _context.Orders.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Orders.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
