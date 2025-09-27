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
    public class SeatReservationController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public SeatReservationController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/seatreservation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeatReservationDto>>> GetReservations()
        {
            var list = await _context.SeatReservations
                .Select(r => new SeatReservationDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    ScreeningId = r.ScreeningId,
                    SeatId = r.SeatId,
                    ReservedAt = r.ReservedAt,
                    ExpiresAt = r.ExpiresAt,
                    OrderId = r.OrderId,
                    Status = r.Status
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/seatreservation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeatReservationDto>> GetReservation(long id)
        {
            var r = await _context.SeatReservations.FindAsync(id);
            if (r == null) return NotFound();

            var dto = new SeatReservationDto
            {
                Id = r.Id,
                UserId = r.UserId,
                ScreeningId = r.ScreeningId,
                SeatId = r.SeatId,
                ReservedAt = r.ReservedAt,
                ExpiresAt = r.ExpiresAt,
                OrderId = r.OrderId,
                Status = r.Status
            };

            return Ok(dto);
        }

        // POST: api/seatreservation
        [HttpPost]
        public async Task<ActionResult<SeatReservationDto>> CreateReservation([FromBody] SeatReservationDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is missing or invalid.");

            // Validate required fields
            if (dto.ScreeningId <= 0)
                return BadRequest("ScreeningId is required and must be greater than 0.");

            if (dto.ExpiresAt == default)
                return BadRequest("ExpiresAt is required.");

            // Validate related entities exist
            var screeningExists = await _context.Screenings.AnyAsync(s => s.Id == dto.ScreeningId);
            if (!screeningExists)
                return BadRequest($"Screening with id {dto.ScreeningId} does not exist.");

            if (dto.SeatId.HasValue)
            {
                var seatExists = await _context.Seats.AnyAsync(s => s.Id == dto.SeatId.Value);
                if (!seatExists)
                    return BadRequest($"Seat with id {dto.SeatId.Value} does not exist.");
            }

            if (dto.UserId.HasValue)
            {
                var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId.Value);
                if (!userExists)
                    return BadRequest($"User with id {dto.UserId.Value} does not exist.");
            }

            if (dto.OrderId.HasValue)
            {
                var orderExists = await _context.Orders.AnyAsync(o => o.Id == dto.OrderId.Value);
                if (!orderExists)
                    return BadRequest($"Order with id {dto.OrderId.Value} does not exist.");
            }

            var entity = new SeatReservation
            {
                UserId = dto.UserId,
                ScreeningId = dto.ScreeningId,
                SeatId = dto.SeatId,
                ReservedAt = dto.ReservedAt ?? DateTime.UtcNow,
                ExpiresAt = dto.ExpiresAt,
                OrderId = dto.OrderId,
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "reserved" : dto.Status
            };

            _context.SeatReservations.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.ReservedAt = entity.ReservedAt;

            return CreatedAtAction(nameof(GetReservation), new { id = dto.Id }, dto);
        }

        // PUT: api/seatreservation/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(long id, [FromBody] SeatReservationDto dto)
        {
            if (dto == null || id != dto.Id)
                return BadRequest();

            var entity = await _context.SeatReservations.FindAsync(id);
            if (entity == null) return NotFound();

            if (dto.ScreeningId <= 0)
                return BadRequest("ScreeningId is required and must be greater than 0.");

            var screeningExists = await _context.Screenings.AnyAsync(s => s.Id == dto.ScreeningId);
            if (!screeningExists)
                return BadRequest($"Screening with id {dto.ScreeningId} does not exist.");

            if (dto.SeatId.HasValue)
            {
                var seatExists = await _context.Seats.AnyAsync(s => s.Id == dto.SeatId.Value);
                if (!seatExists)
                    return BadRequest($"Seat with id {dto.SeatId.Value} does not exist.");
            }

            if (dto.UserId.HasValue)
            {
                var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId.Value);
                if (!userExists)
                    return BadRequest($"User with id {dto.UserId.Value} does not exist.");
            }

            entity.UserId = dto.UserId;
            entity.ScreeningId = dto.ScreeningId;
            entity.SeatId = dto.SeatId;
            entity.ReservedAt = dto.ReservedAt;
            entity.ExpiresAt = dto.ExpiresAt;
            entity.OrderId = dto.OrderId;
            entity.Status = dto.Status;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/seatreservation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(long id)
        {
            var entity = await _context.SeatReservations.FindAsync(id);
            if (entity == null) return NotFound();

            _context.SeatReservations.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
