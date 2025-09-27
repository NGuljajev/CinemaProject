using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaProject.Models;
using CinemaBackend.Data;
using CinemaProject.Dto;

namespace CinemaProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public FilmController(CinemaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDto>>> GetFilms()
        {
            var list = await _context.Films
                .Select(f => new FilmDto
                {
                    Id = f.Id,
                    Title = f.Title,
                    OriginalTitle = f.OriginalTitle,
                    Description = f.Description,
                    DurationMinutes = f.DurationMinutes,
                    ReleaseDate = f.ReleaseDate,
                    AgeRating = f.AgeRating,
                    Language = f.Language,
                    Genres = f.Genres,
                    Director = f.Director,
                    Cast = f.Cast,
                    PosterUrl = f.PosterUrl,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FilmDto>> GetFilm(int id)
        {
            var f = await _context.Films.FindAsync(id);
            if (f == null) return NotFound();

            return Ok(new FilmDto
            {
                Id = f.Id,
                Title = f.Title,
                OriginalTitle = f.OriginalTitle,
                Description = f.Description,
                DurationMinutes = f.DurationMinutes,
                ReleaseDate = f.ReleaseDate,
                AgeRating = f.AgeRating,
                Language = f.Language,
                Genres = f.Genres,
                Director = f.Director,
                Cast = f.Cast,
                PosterUrl = f.PosterUrl,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt
            });
        }

        [HttpPost]
        public async Task<ActionResult<FilmDto>> CreateFilm(FilmDto dto)
        {
            var entity = new Film
            {
                Title = dto.Title,
                OriginalTitle = dto.OriginalTitle,
                Description = dto.Description,
                DurationMinutes = dto.DurationMinutes,
                ReleaseDate = dto.ReleaseDate,
                AgeRating = dto.AgeRating,
                Language = dto.Language,
                Genres = dto.Genres,
                Director = dto.Director,
                Cast = dto.Cast,
                PosterUrl = dto.PosterUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.Films.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetFilm), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFilm(int id, FilmDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var entity = await _context.Films.FindAsync(id);
            if (entity == null) return NotFound();

            entity.Title = dto.Title;
            entity.OriginalTitle = dto.OriginalTitle;
            entity.Description = dto.Description;
            entity.DurationMinutes = dto.DurationMinutes;
            entity.ReleaseDate = dto.ReleaseDate;
            entity.AgeRating = dto.AgeRating;
            entity.Language = dto.Language;
            entity.Genres = dto.Genres;
            entity.Director = dto.Director;
            entity.Cast = dto.Cast;
            entity.PosterUrl = dto.PosterUrl;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            var entity = await _context.Films.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Films.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent(); // <-- Return status 204 after successful delete
        }



    }
}