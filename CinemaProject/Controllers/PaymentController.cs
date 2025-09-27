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
    public class PaymentController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public PaymentController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/payment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments()
        {
            var list = await _context.Payments
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    OrderId = p.OrderId,
                    Amount = p.Amount,
                    Currency = p.Currency,
                    Method = p.Method,
                    ProviderPaymentId = p.ProviderPaymentId,
                    Status = p.Status,
                    PaidAt = p.PaidAt,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/payment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPayment(long id)
        {
            var p = await _context.Payments.FindAsync(id);
            if (p == null) return NotFound();

            var dto = new PaymentDto
            {
                Id = p.Id,
                OrderId = p.OrderId,
                Amount = p.Amount,
                Currency = p.Currency,
                Method = p.Method,
                ProviderPaymentId = p.ProviderPaymentId,
                Status = p.Status,
                PaidAt = p.PaidAt,
                CreatedAt = p.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/payment
        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreatePayment([FromBody] PaymentDto dto)
        {
            if (dto == null) return BadRequest();

            var entity = new Payment
            {
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                Currency = dto.Currency,
                Method = dto.Method,
                ProviderPaymentId = dto.ProviderPaymentId,
                Status = dto.Status,
                PaidAt = dto.PaidAt,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetPayment), new { id = dto.Id }, dto);
        }

        // PUT: api/payment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(long id, [FromBody] PaymentDto dto)
        {
            if (dto == null || id != dto.Id) return BadRequest();

            var entity = await _context.Payments.FindAsync(id);
            if (entity == null) return NotFound();

            entity.OrderId = dto.OrderId;
            entity.Amount = dto.Amount;
            entity.Currency = dto.Currency;
            entity.Method = dto.Method;
            entity.ProviderPaymentId = dto.ProviderPaymentId;
            entity.Status = dto.Status;
            entity.PaidAt = dto.PaidAt;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/payment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(long id)
        {
            var entity = await _context.Payments.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Payments.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
