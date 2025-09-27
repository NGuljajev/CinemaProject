using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaBackend.Data;

[ApiController]
[Route("api/[controller]")]
public class GiftCardTransactionsController : ControllerBase
{
    private readonly CinemaDbContext _context;

    public GiftCardTransactionsController(CinemaDbContext context)
    {
        _context = context;
    }

    // GET: api/giftcardtransactions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GiftCardTransactionDto>>> GetTransactions()
    {
        var transactions = await _context.GiftCardTransactions
            .Select(t => new GiftCardTransactionDto
            {
                Id = t.Id,
                GiftCardId = t.GiftCardId,
                Type = t.Type,
                Amount = t.Amount,
                PerformedBy = t.PerformedBy,
                PerformedAt = t.PerformedAt,
                RelatedOrderId = t.RelatedOrderId,
                Note = t.Note
            })
            .ToListAsync();

        return Ok(transactions);
    }

    // GET: api/giftcardtransactions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GiftCardTransactionDto>> GetTransaction(long id)
    {
        var t = await _context.GiftCardTransactions.FindAsync(id);

        if (t == null)
            return NotFound();

        return new GiftCardTransactionDto
        {
            Id = t.Id,
            GiftCardId = t.GiftCardId,
            Type = t.Type,
            Amount = t.Amount,
            PerformedBy = t.PerformedBy,
            PerformedAt = t.PerformedAt,
            RelatedOrderId = t.RelatedOrderId,
            Note = t.Note
        };
    }

    // POST: api/giftcardtransactions
    [HttpPost]
    public async Task<ActionResult<GiftCardTransactionDto>> CreateTransaction(CreateGiftCardTransactionDto dto)
    {
        var t = new GiftCardTransaction
        {
            GiftCardId = dto.GiftCardId,
            Type = dto.Type,
            Amount = dto.Amount,
            PerformedBy = dto.PerformedBy,
            PerformedAt = dto.PerformedAt ?? DateTime.UtcNow,
            RelatedOrderId = dto.RelatedOrderId,
            Note = dto.Note
        };

        _context.GiftCardTransactions.Add(t);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTransaction), new { id = t.Id }, new GiftCardTransactionDto
        {
            Id = t.Id,
            GiftCardId = t.GiftCardId,
            Type = t.Type,
            Amount = t.Amount,
            PerformedBy = t.PerformedBy,
            PerformedAt = t.PerformedAt,
            RelatedOrderId = t.RelatedOrderId,
            Note = t.Note
        });
    }

    // PUT: api/giftcardtransactions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(long id, CreateGiftCardTransactionDto dto)
    {
        var t = await _context.GiftCardTransactions.FindAsync(id);

        if (t == null)
            return NotFound();

        t.GiftCardId = dto.GiftCardId;
        t.Type = dto.Type;
        t.Amount = dto.Amount;
        t.PerformedBy = dto.PerformedBy;
        t.PerformedAt = dto.PerformedAt ?? DateTime.UtcNow;
        t.RelatedOrderId = dto.RelatedOrderId;
        t.Note = dto.Note;

        _context.Entry(t).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/giftcardtransactions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(long id)
    {
        var t = await _context.GiftCardTransactions.FindAsync(id);
        if (t == null)
            return NotFound();

        _context.GiftCardTransactions.Remove(t);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
