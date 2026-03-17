using AlAshmar.Domain.Entities.Common;
using AlAshmar.Infrastructure.Persistence;
using AlAshmar.Infrastructure.Persistence.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace AlAshmar.Tests.Infrastructure;

/// <summary>
/// Integration tests for <see cref="RepositoryBase{TEntity,TKey}"/> using EF Core InMemory provider.
/// Each test class creates its own isolated in-memory database to avoid cross-test state.
/// </summary>
public class RepositoryIntegrationTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly RepositoryBase<Attachment, Guid> _repository;

    public RepositoryIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new RepositoryBase<Attachment, Guid>(
            _context,
            NullLogger<RepositoryBase<Attachment, Guid>>.Instance);
    }

    public void Dispose() => _context.Dispose();

    // ── AddAsync ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task AddAsync_ValidEntity_PersistsEntityAndReturnsSuccess()
    {
        var attachment = CreateAttachment("image/jpeg");

        var result = await _repository.AddAsync(attachment);

        Assert.False(result.IsError);
        var stored = await _context.Set<Attachment>().FindAsync(attachment.Id);
        Assert.NotNull(stored);
        Assert.Equal("image/jpeg", stored!.Type);
    }

    [Fact]
    public async Task AddAsync_NullEntity_ReturnsError()
    {
        var result = await _repository.AddAsync(null!);

        Assert.True(result.IsError);
    }

    // ── GetByIdAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsCorrectEntity()
    {
        var attachment = CreateAttachment("application/pdf");
        await _context.Attachments.AddAsync(attachment);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(attachment.Id);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(attachment.Id, result.Value!.Id);
        Assert.Equal("application/pdf", result.Value.Type);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsError()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Result<T> cannot hold null; FindAsync(null) throws → caught → DatabaseError
        Assert.True(result.IsError);
    }

    // ── GetAllAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_WithFilter_ReturnsFilteredResults()
    {
        await _context.Attachments.AddRangeAsync(
            CreateAttachment("image/jpeg"),
            CreateAttachment("image/jpeg"),
            CreateAttachment("application/pdf"));
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync(a => a.Type == "image/jpeg");

        Assert.False(result.IsError);
        Assert.Equal(2, result.Value!.Count);
        Assert.All(result.Value, a => Assert.Equal("image/jpeg", a.Type));
    }

    [Fact]
    public async Task GetAllAsync_NoFilter_ReturnsAllEntities()
    {
        await _context.Attachments.AddRangeAsync(
            CreateAttachment("image/png"),
            CreateAttachment("text/plain"));
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        Assert.False(result.IsError);
        Assert.Equal(2, result.Value!.Count);
    }

    // ── GetPagedAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPageAndTotalCount()
    {
        for (int i = 0; i < 5; i++)
            await _context.Attachments.AddAsync(CreateAttachment("image/jpeg"));
        await _context.SaveChangesAsync();

        var result = await _repository.GetPagedAsync(page: 1, pageSize: 3);

        Assert.False(result.IsError);
        Assert.Equal(5, result.Value!.TotalItems);
        Assert.Equal(3, result.Value.Items.Count());
        Assert.Equal(1, result.Value.Page);
    }

    [Fact]
    public async Task GetPagedAsync_Page2_ReturnsRemainingItems()
    {
        for (int i = 0; i < 5; i++)
            await _context.Attachments.AddAsync(CreateAttachment("image/jpeg"));
        await _context.SaveChangesAsync();

        var result = await _repository.GetPagedAsync(page: 2, pageSize: 3);

        Assert.False(result.IsError);
        Assert.Equal(5, result.Value!.TotalItems);
        Assert.Equal(2, result.Value.Items.Count());
    }

    // ── UpdateAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_ValidEntity_PersistsChanges()
    {
        var attachment = CreateAttachment("image/jpeg");
        await _context.Attachments.AddAsync(attachment);
        await _context.SaveChangesAsync();

        // Detach so EF doesn't complain about duplicate tracking
        _context.Entry(attachment).State = EntityState.Detached;
        attachment.Type = "image/png";

        var result = await _repository.UpdateAsync(attachment);

        Assert.False(result.IsError);

        _context.Entry(attachment).State = EntityState.Detached;
        var updated = await _context.Attachments.FindAsync(attachment.Id);
        Assert.Equal("image/png", updated!.Type);
    }

    // ── RemoveAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task RemoveAsync_ExistingEntity_DeletesEntityAndReturnsDeleted()
    {
        var attachment = CreateAttachment("image/jpeg");
        await _context.Attachments.AddAsync(attachment);
        await _context.SaveChangesAsync();

        var result = await _repository.RemoveAsync(a => a.Id == attachment.Id);

        Assert.False(result.IsError);
        var deleted = await _context.Attachments.FindAsync(attachment.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task RemoveAsync_NonExistingEntity_ReturnsError()
    {
        var result = await _repository.RemoveAsync(a => a.Id == Guid.NewGuid());

        Assert.True(result.IsError);
    }

    // ── AnyAsync ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task AnyAsync_MatchingEntitiesExist_ReturnsTrue()
    {
        await _context.Attachments.AddAsync(CreateAttachment("video/mp4"));
        await _context.SaveChangesAsync();

        var exists = await _repository.AnyAsync(a => a.Type == "video/mp4");

        Assert.True(exists);
    }

    [Fact]
    public async Task AnyAsync_NoMatchingEntities_ReturnsFalse()
    {
        var exists = await _repository.AnyAsync(a => a.Type == "application/zip");

        Assert.False(exists);
    }

    // ── Helper ────────────────────────────────────────────────────────────────

    private static Attachment CreateAttachment(string mimeType) =>
        new()
        {
            Id = Guid.NewGuid(),
            Path = $"/uploads/{Guid.NewGuid()}.bin",
            Type = mimeType,
            SafeName = $"safe_{Guid.NewGuid():N}.bin",
            OriginalName = "original.bin",
            ExtensionId = null
        };
}
