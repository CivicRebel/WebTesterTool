namespace WebTester.Repositories.Entities
{
    public class ChunkSize: IEntity<short>
    {
        public short Id { get; set; }  
        public decimal SizeInKb { get; set; }
        public string Name { get; set; }
    }
}
