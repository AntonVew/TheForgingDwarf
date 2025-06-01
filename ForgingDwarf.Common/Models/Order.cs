using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForgingDwarf.Common.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Клиент не указан")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Тип изделия обязателен")]
        public ItemType ItemType { get; set; }

        [Required(ErrorMessage = "Тип стали обязателен")]
        public SteelType SteelType { get; set; }

        [Required(ErrorMessage = "Стиль обязателен")]
        public Style Style { get; set; }

        [Required(ErrorMessage = "Дата заказа обязательна")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Укажите, является ли заказ индивидуальным")]
        public bool IsCustom { get; set; }

        [Required(ErrorMessage = "Статус заказа обязателен")]
        public OrderStatus Status { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public double Price { get; set; }

        [JsonIgnore]
        public Client? Client { get; set; }
    }

    //Енамы для полей класса

        public enum OrderStatus
        {
            InQueue,
            InProgress,
            Completed
        }

        public enum ItemType
        {
            Weapon,
            Gate,
            Sculpture,
            ArmorMedieval,
            ArmorRenaissance
        }

        public enum SteelType
        {
            Damascus,
            Mosaic,
            Crucible
        }

        public enum Style
        {
            Gothic,
            Fantasy,
            Historical
        }
}