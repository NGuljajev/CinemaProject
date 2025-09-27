using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CinemaBackend.Data;
using CinemaProject.Controllers;
using CinemaProject.Models;
using CinemaProject.Dto;

namespace CinemaProject.UnitTests.ControllerTests
{
    public class FilmControllerTests
    {
        private CinemaDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<CinemaDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new CinemaDbContext(options);
        }

        [Fact]
        public async Task GetFilms_ReturnsAllItems()
        {
            var context = CreateContext(nameof(GetFilms_ReturnsAllItems));
            context.Films.AddRange(new[]
            {
                new Film { Title="Film1", OriginalTitle="Orig1", Description="Desc1", DurationMinutes=120, ReleaseDate=DateTime.UtcNow, AgeRating="PG", Language="EN", Genres="Drama", Director="Dir1", Cast="Actor1", PosterUrl="url1", CreatedAt=DateTime.UtcNow },
                new Film { Title="Film2", OriginalTitle="Orig2", Description="Desc2", DurationMinutes=90, ReleaseDate=DateTime.UtcNow, AgeRating="R", Language="FR", Genres="Comedy", Director="Dir2", Cast="Actor2", PosterUrl="url2", CreatedAt=DateTime.UtcNow }
            });
            await context.SaveChangesAsync();
            var controller = new FilmController(context);

            var result = await controller.GetFilms();

            var ok = Assert.IsType<ActionResult<IEnumerable<FilmDto>>>(result);
            var items = Assert.IsType<OkObjectResult>(ok.Result).Value as IEnumerable<FilmDto>;
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task GetFilm_WhenFound_ReturnsDto()
        {
            var context = CreateContext(nameof(GetFilm_WhenFound_ReturnsDto));
            var entity = new Film
            {
                Title = "Interstellar",
                OriginalTitle = "Interstellar",
                Description = "Sci-Fi epic",
                DurationMinutes = 169,
                ReleaseDate = new DateTime(2014, 11, 7),
                AgeRating = "PG-13",
                Language = "EN",
                Genres = "Sci-Fi",
                Director = "Christopher Nolan",
                Cast = "Matthew McConaughey",
                PosterUrl = "poster.jpg",
                CreatedAt = DateTime.UtcNow
            };
            context.Films.Add(entity);
            await context.SaveChangesAsync();
            var controller = new FilmController(context);

            var action = await controller.GetFilm(entity.Id);

            var ok = Assert.IsType<ActionResult<FilmDto>>(action);
            var dto = Assert.IsType<OkObjectResult>(ok.Result).Value as FilmDto;
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal("Interstellar", dto.Title);
        }

        [Fact]
        public async Task GetFilm_WhenNotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(GetFilm_WhenNotFound_ReturnsNotFound));
            var controller = new FilmController(context);

            var action = await controller.GetFilm(999);

            var result = Assert.IsType<ActionResult<FilmDto>>(action);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateFilm_AddsEntityAndReturnsCreated()
        {
            var context = CreateContext(nameof(CreateFilm_AddsEntityAndReturnsCreated));
            var controller = new FilmController(context);
            var dto = new FilmDto
            {
                Title = "New Film",
                OriginalTitle = "Nouvelle Film",
                Description = "Fresh release",
                DurationMinutes = 110,
                ReleaseDate = DateTime.UtcNow,
                AgeRating = "PG",
                Language = "EN",
                Genres = "Action",
                Director = "Director",
                Cast = "Actor1, Actor2",
                PosterUrl = "poster.png"
            };

            var action = await controller.CreateFilm(dto);

            var result = Assert.IsType<CreatedAtActionResult>(action.Result);
            var returned = result.Value as FilmDto;
            Assert.NotEqual(0, returned.Id);
            Assert.Equal("New Film", returned.Title);
            Assert.True((DateTime.UtcNow - returned.CreatedAt).TotalSeconds < 5);

            var saved = await context.Films.FindAsync(returned.Id);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task UpdateFilm_IdMismatch_ReturnsBadRequest()
        {
            var context = CreateContext(nameof(UpdateFilm_IdMismatch_ReturnsBadRequest));
            var controller = new FilmController(context);
            var dto = new FilmDto { Id = 1, Title = "Mismatch" };

            var result = await controller.UpdateFilm(2, dto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateFilm_NotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(UpdateFilm_NotFound_ReturnsNotFound));
            var controller = new FilmController(context);
            var dto = new FilmDto { Id = 1, Title = "DoesNotExist" };

            var result = await controller.UpdateFilm(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateFilm_ValidUpdate_ReturnsNoContent()
        {
            var context = CreateContext(nameof(UpdateFilm_ValidUpdate_ReturnsNoContent));
            var entity = new Film
            {
                Title = "Old Film",
                OriginalTitle = "Old Orig",
                Description = "Old description",
                DurationMinutes = 100,
                ReleaseDate = DateTime.UtcNow,
                AgeRating = "G",
                Language = "EN",
                Genres = "Drama",
                Director = "Old Director",
                Cast = "Old Cast",
                PosterUrl = "old.jpg",
                CreatedAt = DateTime.UtcNow
            };
            context.Films.Add(entity);
            await context.SaveChangesAsync();
            var controller = new FilmController(context);

            var dto = new FilmDto
            {
                Id = entity.Id,
                Title = "Updated Film",
                OriginalTitle = "Updated Orig",
                Description = "Updated description",
                DurationMinutes = 120,
                ReleaseDate = entity.ReleaseDate,
                AgeRating = "PG",
                Language = "FR",
                Genres = "Comedy",
                Director = "New Director",
                Cast = "New Cast",
                PosterUrl = "new.jpg"
            };

            var result = await controller.UpdateFilm(entity.Id, dto);

            Assert.IsType<NoContentResult>(result);
            var updated = await context.Films.FindAsync(entity.Id);
            Assert.Equal("Updated Film", updated.Title);
            Assert.Equal("Comedy", updated.Genres);
            Assert.Equal("New Director", updated.Director);
        }

        [Fact]
        public async Task DeleteFilm_NotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(DeleteFilm_NotFound_ReturnsNotFound));
            var controller = new FilmController(context);

            var result = await controller.DeleteFilm(123);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteFilm_Exists_RemovesAndReturnsNoContent()
        {
            var context = CreateContext(nameof(DeleteFilm_Exists_RemovesAndReturnsNoContent));
            var entity = new Film
            {
                Title = "DeleteMe",
                OriginalTitle = "To be gone",
                Description = "Testing delete",
                DurationMinutes = 95,
                ReleaseDate = DateTime.UtcNow,
                AgeRating = "R",
                Language = "EN",
                Genres = "Thriller",
                Director = "Someone",
                Cast = "Actor1",
                PosterUrl = "poster.png",
                CreatedAt = DateTime.UtcNow
            };
            context.Films.Add(entity);
            await context.SaveChangesAsync();
            var controller = new FilmController(context);

            var result = await controller.DeleteFilm(entity.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(await context.Films.FindAsync(entity.Id));
        }
    }
}
