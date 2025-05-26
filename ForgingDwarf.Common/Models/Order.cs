namespace ForgingDwarf.Common.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ItemType { get; set; } // Меч, ворота и т.д.
        public string SteelType { get; set; } // Дамасская, тигельная...
        public string Style { get; set; } // Готика, фэнтези...
        public DateTime OrderDate { get; set; }
        public bool IsCustom { get; set; }
        public string Status { get; set; } // "В очереди", "В работе", "Готово"
    }
}
