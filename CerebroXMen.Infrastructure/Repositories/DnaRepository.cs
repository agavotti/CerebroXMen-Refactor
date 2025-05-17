using CerebroXMen.Domain.Entities;
using CerebroXMen.Domain.Repositories;
using CerebroXMen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CerebroXMen.Infrastructure.Repositories;

public class DnaRepository(CerebroDbContext context) : IDnaRepository
{
    private readonly CerebroDbContext _context = context;

    public async Task<DnaSequence?> GetBySequenceAsync(string[] sequence)
    {
        return await _context.DnaSequences
            .FirstOrDefaultAsync(d => d.Sequence.SequenceEqual(sequence));
    }

    public async Task AddAsync(DnaSequence dnaSequence)
    {
        _context.DnaSequences.Add(dnaSequence);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountMutantsAsync()
    {
        return await _context.DnaSequences.CountAsync(d => d.IsMutant);
    }

    public async Task<int> CountHumansAsync()
    {
        return await _context.DnaSequences.CountAsync(d => !d.IsMutant);
    }
}