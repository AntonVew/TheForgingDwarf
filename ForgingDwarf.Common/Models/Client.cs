using System.ComponentModel.DataAnnotations;

namespace ForgingDwarf.Common.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя клиента обязательно")]
        [StringLength(100, ErrorMessage = "Имя не может быть длиннее 100 символов")]
        public string? Name { get; set; }

        [Phone(ErrorMessage = "Некорректный формат телефона")]
        [Required(ErrorMessage = "Телефон обязателен")]
        public string? Phone { get; set; }

        public string? Email { get; set; }

        public List<Order> Orders { get; set; } = [];
    }
}