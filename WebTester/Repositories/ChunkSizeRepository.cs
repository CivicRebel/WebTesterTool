using WebTester.Repositories.Base;
using WebTester.Repositories.Entities;
using WebTester.Repositories.Interfaces;

namespace WebTester.Repositories
{
    public class ChunkSizeRepository: BaseRepository<ChunkSize, short>, IChunkSizeRepository
    {
        public ChunkSizeRepository(IEnumerable<ChunkSize> entities) : base(entities)
        {
        }
    }
}
