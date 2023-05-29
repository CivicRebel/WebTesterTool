using WebTester.Repositories.Base;
using WebTester.Repositories.Entities;
using WebTester.Repositories.Interfaces;

namespace WebTester.Repositories
{
    public class FileSizeRepository : BaseRepository<FileSize, short>, IFileSizeRepository
    {
        public FileSizeRepository(IEnumerable<FileSize> entities): base(entities)
        {
        }
    }
}
