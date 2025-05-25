using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TheForgingDwarf
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Настройка подключения к БД
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseSqlite("filename=../../../MyLocalLibrary.db")
                .Options;

            // Создание и обеспечение существования БД
            using (var db = new ProductContext(options))
            {
                if (!db.Database.Exists())
                {
                    db.Database.Create();
                }
                db.Database.EnsureCreated();

                // 1. Добавление тестовых данных
                if (!db.Users.Any())
                {
                    var user1 = new User { Name = "Гендальф", IsSuperUser = true };
                    var user2 = new User { Name = "Фродо", IsSuperUser = false };
                    db.Users.AddRange(user1, user2);

                    var product1 = new Product
                    {
                        Name = "Меч Араддиг",
                        IsCustom = false,
                        Type = Product.ProdType.Weapon,
                        Price = 999.99m
                    };
                    db.Products.Add(product1);

                    db.SaveChanges();
                }

                // 2. Чтение и вывод данных
                Console.WriteLine("Пользователи:");
                foreach (var user in db.Users.ToList())
                {
                    Console.WriteLine($"{user.Id}: {user.Name}");
                }
            }
        }
    }
}
