namespace ParsePdfApp.Models
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
    }
    public class File
    {
        public string FileName { get; set; }
        public string Data { get; set; }

    }

    public class PurchaseWithFile : Purchase
    {
        public File File { get; set; }
    }

}
