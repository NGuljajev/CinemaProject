using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CinemaBackend.Data;

namespace CinemaProject.UnitTests.ControllerTests
{
    public class GiftCardsControllerTests
    {
        private CinemaDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<CinemaDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new CinemaDbContext(options);
        }

        [Fact]
        public async Task GetGiftCards_ReturnsAll()
        {
            var context = CreateContext(nameof(GetGiftCards_ReturnsAll));
            context.GiftCards.AddRange(new[]
            {
                new GiftCard { Code="ABC123", Amount=50, Balance=50, Currency="USD", IssuedByUser=1, IssuedToEmail="a@test.com", Message="Enjoy", ExpiresAt=DateTime.UtcNow.AddMonths(6), IsActive=true, CreatedAt=DateTime.UtcNow },
                new GiftCard { Code="XYZ789", Amount=100, Balance=80, Currency="EUR", IssuedByUser=2, IssuedToEmail="b@test.com", Message="Happy Birthday", ExpiresAt=DateTime.UtcNow.AddMonths(12), IsActive=true, CreatedAt=DateTime.UtcNow }
            });
            await context.SaveChangesAsync();
            var controller = new GiftCardsController(context);

            var result = await controller.GetGiftCards();

            var ok = Assert.IsType<ActionResult<IEnumerable<GiftCardDto>>>(result);
            var items = Assert.IsType<OkObjectResult>(ok.Result).Value as IEnumerable<GiftCardDto>;
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task GetGiftCard_WhenFound_ReturnsDto()
        {
            var context = CreateContext(nameof(GetGiftCard_WhenFound_ReturnsDto));
            var entity = new GiftCard
            {
                Code = "CARD123",
                Amount = 200,
                Balance = 200,
                Currency = "USD",
                IssuedByUser = 5,
                IssuedToEmail = "someone@example.com",
                Message = "Congrats",
                ExpiresAt = DateTime.UtcNow.AddYears(1),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            context.GiftCards.Add(entity);
            await context.SaveChangesAsync();
            var controller = new GiftCardsController(context);

            var action = await controller.GetGiftCard(entity.Id);

            var ok = Assert.IsType<ActionResult<GiftCardDto>>(action);
            var dto = Assert.IsType<GiftCardDto>(ok.Value);
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal("CARD123", dto.Code);
            Assert.Equal(200, dto.Amount);
        }

        [Fact]
        public async Task GetGiftCard_WhenNotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(GetGiftCard_WhenNotFound_ReturnsNotFound));
            var controller = new GiftCardsController(context);

            var action = await controller.GetGiftCard(999);

            var result = Assert.IsType<ActionResult<GiftCardDto>>(action);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateGiftCard_Valid_AddsEntityAndReturnsCreated()
        {
            var context = CreateContext(nameof(CreateGiftCard_Valid_AddsEntityAndReturnsCreated));
            var controller = new GiftCardsController(context);
            var dto = new CreateGiftCardDto
            {
                Code = "NEW123",
                Amount = 75,
                Balance = 75,
                Currency = "USD",
                IssuedByUser = 3,
                IssuedToEmail = "new@test.com",
                Message = "Welcome",
                ExpiresAt = DateTime.UtcNow.AddMonths(3),
                IsActive = true
            };

            var action = await controller.CreateGiftCard(dto);

            var result = Assert.IsType<CreatedAtActionResult>(action.Result);
            var returned = Assert.IsType<GiftCardDto>(result.Value);
            Assert.NotEqual(0, returned.Id);
            Assert.Equal("NEW123", returned.Code);
            Assert.True((DateTime.UtcNow - returned.CreatedAt).TotalSeconds < 5);

            var saved = await context.GiftCards.FindAsync(returned.Id);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task UpdateGiftCard_NotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(UpdateGiftCard_NotFound_ReturnsNotFound));
            var controller = new GiftCardsController(context);
            var dto = new CreateGiftCardDto
            {
                Code = "UPDATE1",
                Amount = 50,
                Balance = 50,
                Currency = "USD",
                IsActive = true
            };

            var result = await controller.UpdateGiftCard(999, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateGiftCard_ValidUpdate_ReturnsNoContent()
        {
            var context = CreateContext(nameof(UpdateGiftCard_ValidUpdate_ReturnsNoContent));
            var entity = new GiftCard
            {
                Code = "OLD",
                Amount = 30,
                Balance = 30,
                Currency = "USD",
                IssuedByUser = 1,
                IssuedToEmail = "old@test.com",
                Message = "Old Msg",
                ExpiresAt = DateTime.UtcNow.AddMonths(1),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            context.GiftCards.Add(entity);
            await context.SaveChangesAsync();
            var controller = new GiftCardsController(context);

            var dto = new CreateGiftCardDto
            {
                Code = "UPDATED",
                Amount = 60,
                Balance = 60,
                Currency = "EUR",
                IssuedByUser = 2,
                IssuedToEmail = "new@test.com",
                Message = "New Msg",
                ExpiresAt = DateTime.UtcNow.AddMonths(12),
                IsActive = false
            };

            var result = await controller.UpdateGiftCard(entity.Id, dto);

            Assert.IsType<NoContentResult>(result);
            var updated = await context.GiftCards.FindAsync(entity.Id);
            Assert.Equal("UPDATED", updated.Code);
            Assert.Equal(60, updated.Amount);
            Assert.Equal("EUR", updated.Currency);
            Assert.False(updated.IsActive);
        }

        [Fact]
        public async Task DeleteGiftCard_NotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(DeleteGiftCard_NotFound_ReturnsNotFound));
            var controller = new GiftCardsController(context);

            var result = await controller.DeleteGiftCard(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteGiftCard_Exists_RemovesAndReturnsNoContent()
        {
            var context = CreateContext(nameof(DeleteGiftCard_Exists_RemovesAndReturnsNoContent));
            var entity = new GiftCard
            {
                Code = "DEL",
                Amount = 40,
                Balance = 40,
                Currency = "USD",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            context.GiftCards.Add(entity);
            await context.SaveChangesAsync();
            var controller = new GiftCardsController(context);

            var result = await controller.DeleteGiftCard(entity.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(await context.GiftCards.FindAsync(entity.Id));
        }
    }
}
