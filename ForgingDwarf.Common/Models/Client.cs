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

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль от 6 символов")]
        public string? Password { get; set; }

        public string? Phone { get; set; }
        public bool IsSuperuser { get; set; } = false;
        public string? Email { get; set; }

        public List<Order> Orders { get; set; } = [];


    }
}