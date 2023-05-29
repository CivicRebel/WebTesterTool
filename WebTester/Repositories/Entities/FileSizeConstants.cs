namespace WebTester.Repositories.Entities
{
    public static class SizeLists
    {
        public static readonly IEnumerable<FileSize> FileSizes = new[]
        {
            new FileSize { Id = 0, SizeInMb = 1, Name = "1MB" },
            new FileSize { Id = 1, SizeInMb = 10, Name = "10MB"},
            new FileSize { Id = 2, SizeInMb = 50, Name = "50MB" },
            new FileSize { Id = 3, SizeInMb = 100, Name = "100MB" },
            new FileSize { Id = 4, SizeInMb = 300, Name = "300MB" }
        };
       
        public static readonly IEnumerable<ChunkSize> ChunkSizes = new[]
        {
            new ChunkSize { Id = 0, SizeInKb = 4, Name = "4KB" },
            new ChunkSize { Id = 1, SizeInKb = 8, Name = "8KB"},
            new ChunkSize { Id = 2, SizeInKb = 16, Name = "16KB" },
            new ChunkSize { Id = 3, SizeInKb = 32, Name = "32KB" },
            new ChunkSize { Id = 4, SizeInKb = 64, Name = "64KB" }
        };
    }
}
