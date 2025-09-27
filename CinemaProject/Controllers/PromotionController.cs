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
    public class PromotionController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public PromotionController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/promotion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromotionDto>>> GetPromotions()
        {
            var list = await _context.Promotions
                .Select(p => new PromotionDto
                {
                    Id = p.Id,
                    EventId = p.EventId,
                    Name = p.Name,
                    Description = p.Description,
                    PromoCode = p.PromoCode,
                    DiscountType = p.DiscountType,
                    DiscountValue = p.DiscountValue,
                    ValidFrom = p.ValidFrom,
                    ValidTo = p.ValidTo,
                    UsageLimit = p.UsageLimit,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/promotion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionDto>> GetPromotion(int id)
        {
            var p = await _context.Promotions.FindAsync(id);
            if (p == null) return NotFound();

            var dto = new PromotionDto
            {
                Id = p.Id,
                EventId = p.EventId,
                Name = p.Name,
                Description = p.Description,
                PromoCode = p.PromoCode,
                DiscountType = p.DiscountType,
                DiscountValue = p.DiscountValue,
                ValidFrom = p.ValidFrom,
                ValidTo = p.ValidTo,
                UsageLimit = p.UsageLimit,
                CreatedAt = p.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/promotion
        [HttpPost]
        public async Task<ActionResult<PromotionDto>> CreatePromotion([FromBody] PromotionDto dto)
        {
            if (dto == null) return BadRequest();

            var entity = new Promotion
            {
                EventId = dto.EventId,
                Name = dto.Name,
                Description = dto.Description,
                PromoCode = dto.PromoCode,
                DiscountType = dto.DiscountType,
                DiscountValue = dto.DiscountValue,
                ValidFrom = dto.ValidFrom,
                ValidTo = dto.ValidTo,
                UsageLimit = dto.UsageLimit,
                CreatedAt = DateTime.UtcNow
            };

            _context.Promotions.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetPromotion), new { id = dto.Id }, dto);
        }

        // PUT: api/promotion/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePromotion(int id, [FromBody] PromotionDto dto)
        {
            if (dto == null || id != dto.Id) return BadRequest();

            var entity = await _context.Promotions.FindAsync(id);
            if (entity == null) return NotFound();

            entity.EventId = dto.EventId;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.PromoCode = dto.PromoCode;
            entity.DiscountType = dto.DiscountType;
            entity.DiscountValue = dto.DiscountValue;
            entity.ValidFrom = dto.ValidFrom;
            entity.ValidTo = dto.ValidTo;
            entity.UsageLimit = dto.UsageLimit;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/promotion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            var entity = await _context.Promotions.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Promotions.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
