using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using T240P01.Controllers;

using CinemaBackend.Data;

using CinemaProject.Models;

using CinemaProject.Dto;

using Xunit;
using CinemaProject.Controllers;


namespace CinemaProject.UnitTests.ControllerTests

{

    public class CinemaControllerTests

    {

        private CinemaDbContext CreateContext(string dbName)

        {

            var options = new DbContextOptionsBuilder<CinemaDbContext>()

                .UseInMemoryDatabase(databaseName: dbName)

                .Options;

            return new CinemaDbContext(options);

        }

        [Fact]

        public async Task GetCinemas_ReturnsAllItems()

        {

            // Arrange

            var context = CreateContext(nameof(GetCinemas_ReturnsAllItems));

            context.Cinemas.AddRange(new[]

            {

                new Cinema { Name="A", Address="X", City="C1", Phone="P1", Email="E1", Website="W1", Timezone="TZ1", CreatedAt=DateTime.UtcNow },

                new Cinema { Name="B", Address="Y", City="C2", Phone="P2", Email="E2", Website="W2", Timezone="TZ2", CreatedAt=DateTime.UtcNow }

            });

            await context.SaveChangesAsync();

            var controller = new CinemaController(context);

            // Act

            var result = await controller.GetCinemas();

            // Assert

            var ok = Assert.IsType<ActionResult<IEnumerable<CinemaDto>>>(result);

            var items = Assert.IsType<OkObjectResult>(ok.Result).Value as IEnumerable<CinemaDto>;

            Assert.Equal(2, items.Count());

        }

        [Fact]

        public async Task GetCinema_WhenFound_ReturnsDto()

        {

            // Arrange

            var context = CreateContext(nameof(GetCinema_WhenFound_ReturnsDto));

            var entity = new Cinema

            {

                Name = "Test",

                Address = "Addr",

                City = "City",

                Phone = "123",

                Email = "t@t.com",

                Website = "site",

                Timezone = "TZ",

                CreatedAt = DateTime.UtcNow

            };

            context.Cinemas.Add(entity);

            await context.SaveChangesAsync();

            var controller = new CinemaController(context);

            // Act

            var action = await controller.GetCinema(entity.Id);

            // Assert

            var ok = Assert.IsType<ActionResult<CinemaDto>>(action);

            var dto = Assert.IsType<OkObjectResult>(ok.Result).Value as CinemaDto;

            Assert.Equal(entity.Id, dto.Id);

            Assert.Equal("Test", dto.Name);

        }

        [Fact]

        public async Task GetCinema_WhenNotFound_ReturnsNotFound()

        {

            // Arrange

            var context = CreateContext(nameof(GetCinema_WhenNotFound_ReturnsNotFound));

            var controller = new CinemaController(context);

            // Act

            var action = await controller.GetCinema(999);

            // Assert

            var result = Assert.IsType<ActionResult<CinemaDto>>(action);

            Assert.IsType<NotFoundResult>(result.Result);

        }

        [Fact]

        public async Task CreateCinema_AddsEntityAndReturnsCreated()

        {

            // Arrange

            var context = CreateContext(nameof(CreateCinema_AddsEntityAndReturnsCreated));

            var controller = new CinemaController(context);

            var dto = new CinemaDto

            {

                Name = "New",

                Address = "A",

                City = "C",

                Phone = "P",

                Email = "E",

                Website = "W",

                Timezone = "TZ"

            };

            // Act

            var action = await controller.CreateCinema(dto);

            // Assert

            var result = Assert.IsType<CreatedAtActionResult>(action.Result);

            var returned = result.Value as CinemaDto;

            Assert.NotEqual(0, returned.Id);

            Assert.Equal("New", returned.Name);

            Assert.True((DateTime.UtcNow - returned.CreatedAt).TotalSeconds < 5);

            Assert.True((DateTime.UtcNow - returned.UpdatedAt).TotalSeconds < 5);

            // Verify persisted

            var saved = await context.Cinemas.FindAsync(returned.Id);

            Assert.NotNull(saved);

        }

        [Fact]

        public async Task UpdateCinema_IdMismatch_ReturnsBadRequest()

        {

            // Arrange

            var context = CreateContext(nameof(UpdateCinema_IdMismatch_ReturnsBadRequest));

            var controller = new CinemaController(context);

            var dto = new CinemaDto { Id = 1 };

            // Act

            var result = await controller.UpdateCinema(2, dto);

            // Assert

            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]

        public async Task UpdateCinema_NotFound_ReturnsNotFound()

        {

            // Arrange

            var context = CreateContext(nameof(UpdateCinema_NotFound_ReturnsNotFound));

            var controller = new CinemaController(context);

            var dto = new CinemaDto { Id = 1 };

            // Act

            var result = await controller.UpdateCinema(1, dto);

            // Assert

            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]

        public async Task UpdateCinema_ValidUpdate_ReturnsNoContent()

        {

            // Arrange

            var context = CreateContext(nameof(UpdateCinema_ValidUpdate_ReturnsNoContent));

            var entity = new Cinema

            {

                Name = "Old",

                Address = "X",

                City = "Y",

                Phone = "P",

                Email = "E",

                Website = "W",

                Timezone = "TZ",

                CreatedAt = DateTime.UtcNow

            };

            context.Cinemas.Add(entity);

            await context.SaveChangesAsync();

            var controller = new CinemaController(context);

            var dto = new CinemaDto

            {

                Id = entity.Id,

                Name = "Updated",

                Address = "X2",

                City = "Y2",

                Phone = "P2",

                Email = "E2",

                Website = "W2",

                Timezone = "TZ2"

            };

            // Act

            var result = await controller.UpdateCinema(entity.Id, dto);

            // Assert

            Assert.IsType<NoContentResult>(result);

            var updated = await context.Cinemas.FindAsync(entity.Id);

            Assert.Equal("Updated", updated.Name);

            Assert.Equal("Y2", updated.City);

        }

        [Fact]

        public async Task DeleteCinema_NotFound_ReturnsNotFound()

        {

            // Arrange

            var context = CreateContext(nameof(DeleteCinema_NotFound_ReturnsNotFound));

            var controller = new CinemaController(context);

            // Act

            var result = await controller.DeleteCinema(1);

            // Assert

            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]

        public async Task DeleteCinema_Exists_ReturnsNoContentAndRemoves()

        {

            // Arrange

            var context = CreateContext(nameof(DeleteCinema_Exists_ReturnsNoContentAndRemoves));

            var entity = new Cinema

            {

                Name = "ToDelete",

                Address = "X",

                City = "Y",

                Phone = "P",

                Email = "E",

                Website = "W",

                Timezone = "TZ",

                CreatedAt = DateTime.UtcNow

            };

            context.Cinemas.Add(entity);

            await context.SaveChangesAsync();

            var controller = new CinemaController(context);

            // Act

            var result = await controller.DeleteCinema(entity.Id);

            // Assert

            Assert.IsType<NoContentResult>(result);

            Assert.Null(await context.Cinemas.FindAsync(entity.Id));

        }

    }

}

