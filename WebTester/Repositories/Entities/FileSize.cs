namespace WebTester.Repositories.Entities
{
    public class FileSize: IEntity<short>
    {
        public short Id { get; set; }  
        public decimal SizeInMb { get; set; }
        public string Name { get; set; }
    }
}
