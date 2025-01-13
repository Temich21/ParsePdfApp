using ParsePdfApp.Models;

namespace ParsePdfApp.ServicesВ
{
    public class PurchaseService
    {
        private readonly List<Purchase> _purchases;

        public PurchaseService()
        {
            // Инициализируем список пользователей здесь, чтобы он не требовал регистрации в DI контейнере
            _purchases = new List<Purchase>
            {
            new PurchaseWithFile
                 {
                        Id = Guid.NewGuid(),
                        Name = "Weekly Groceries",
                        Date = "Mon Jan 06 2025 11:26:57 GMT+0100 (Central European Standard Time)",
                        File = new Models.File
                        {
                            FileName = "receipt.pdf",
                            Data = ""
                        }
                 },
            };
        }

        public List<Purchase> getPurchaseList() { return _purchases; }
    }
}