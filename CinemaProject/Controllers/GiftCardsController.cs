using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaBackend.Data;

[ApiController]
[Route("api/[controller]")]
public class GiftCardsController : ControllerBase
{
    private readonly CinemaDbContext _context;

    public GiftCardsController(CinemaDbContext context)
    {
        _context = context;
    }

    // GET: api/giftcards
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GiftCardDto>>> GetGiftCards()
    {
        var cards = await _context.GiftCards
            .Select(g => new GiftCardDto
            {
                Id = g.Id,
                Code = g.Code,
                Amount = g.Amount,
                Balance = g.Balance,
                Currency = g.Currency,
                IssuedByUser = g.IssuedByUser,
                IssuedToEmail = g.IssuedToEmail,
                Message = g.Message,
                ExpiresAt = g.ExpiresAt,
                IsActive = g.IsActive,
                CreatedAt = g.CreatedAt
            })
            .ToListAsync();

        return Ok(cards);
    }

    // GET: api/giftcards/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GiftCardDto>> GetGiftCard(long id)
    {
        var g = await _context.GiftCards.FindAsync(id);

        if (g == null)
            return NotFound();

        return new GiftCardDto
        {
            Id = g.Id,
            Code = g.Code,
            Amount = g.Amount,
            Balance = g.Balance,
            Currency = g.Currency,
            IssuedByUser = g.IssuedByUser,
            IssuedToEmail = g.IssuedToEmail,
            Message = g.Message,
            ExpiresAt = g.ExpiresAt,
            IsActive = g.IsActive,
            CreatedAt = g.CreatedAt
        };
    }

    // POST: api/giftcards
    [HttpPost]
    public async Task<ActionResult<GiftCardDto>> CreateGiftCard(CreateGiftCardDto dto)
    {
        var g = new GiftCard
        {
            Code = dto.Code,
            Amount = dto.Amount,
            Balance = dto.Balance,
            Currency = dto.Currency,
            IssuedByUser = dto.IssuedByUser,
            IssuedToEmail = dto.IssuedToEmail,
            Message = dto.Message,
            ExpiresAt = dto.ExpiresAt,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.GiftCards.Add(g);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGiftCard), new { id = g.Id }, new GiftCardDto
        {
            Id = g.Id,
            Code = g.Code,
            Amount = g.Amount,
            Balance = g.Balance,
            Currency = g.Currency,
            IssuedByUser = g.IssuedByUser,
            IssuedToEmail = g.IssuedToEmail,
            Message = g.Message,
            ExpiresAt = g.ExpiresAt,
            IsActive = g.IsActive,
            CreatedAt = g.CreatedAt
        });
    }

    // PUT: api/giftcards/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGiftCard(long id, CreateGiftCardDto dto)
    {
        var g = await _context.GiftCards.FindAsync(id);

        if (g == null)
            return NotFound();

        g.Code = dto.Code;
        g.Amount = dto.Amount;
        g.Balance = dto.Balance;
        g.Currency = dto.Currency;
        g.IssuedByUser = dto.IssuedByUser;
        g.IssuedToEmail = dto.IssuedToEmail;
        g.Message = dto.Message;
        g.ExpiresAt = dto.ExpiresAt;
        g.IsActive = dto.IsActive;

        _context.Entry(g).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/giftcards/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGiftCard(long id)
    {
        var g = await _context.GiftCards.FindAsync(id);
        if (g == null)
            return NotFound();

        _context.GiftCards.Remove(g);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
