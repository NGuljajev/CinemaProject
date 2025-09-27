using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaBackend.Data;
using CinemaProject.Models;
using CinemaProject.Dto;

namespace T240P01.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public TicketController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            var list = await _context.Tickets
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    OrderId = t.OrderId,
                    ScreeningId = t.ScreeningId,
                    SeatId = t.SeatId,
                    Price = t.Price,
                    TicketType = t.TicketType,
                    Status = t.Status,
                    QrCode = t.QrCode,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(long id)
        {
            var t = await _context.Tickets.FindAsync(id);
            if (t == null) return NotFound();

            var dto = new TicketDto
            {
                Id = t.Id,
                OrderId = t.OrderId,
                ScreeningId = t.ScreeningId,
                SeatId = t.SeatId,
                Price = t.Price,
                TicketType = t.TicketType,
                Status = t.Status,
                QrCode = t.QrCode,
                CreatedAt = t.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/ticket
        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] TicketDto dto)
        {
            if (dto == null) return BadRequest();

            var entity = new Ticket
            {
                OrderId = dto.OrderId,
                ScreeningId = dto.ScreeningId,
                SeatId = dto.SeatId,
                Price = dto.Price,
                TicketType = dto.TicketType,
                Status = dto.Status,
                QrCode = dto.QrCode,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tickets.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetTicket), new { id = dto.Id }, dto);
        }

        // PUT: api/ticket/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(long id, [FromBody] TicketDto dto)
        {
            if (dto == null || id != dto.Id) return BadRequest();

            var entity = await _context.Tickets.FindAsync(id);
            if (entity == null) return NotFound();

            entity.OrderId = dto.OrderId;
            entity.ScreeningId = dto.ScreeningId;
            entity.SeatId = dto.SeatId;
            entity.Price = dto.Price;
            entity.TicketType = dto.TicketType;
            entity.Status = dto.Status;
            entity.QrCode = dto.QrCode;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ticket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(long id)
        {
            var entity = await _context.Tickets.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Tickets.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
