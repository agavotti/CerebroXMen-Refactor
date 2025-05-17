using CerebroXMen.Domain.Entities;

namespace CerebroXMen.Domain.Repositories
{
    public interface IDnaRepository
    {
        Task<DnaSequence?> GetBySequenceAsync(string[] sequence);
        Task AddAsync(DnaSequence dnaSequence);
        Task<int> CountMutantsAsync();
        Task<int> CountHumansAsync();
    }
}
