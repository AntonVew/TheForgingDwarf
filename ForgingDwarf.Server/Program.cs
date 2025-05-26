using ForgingDwarf.Server.Data;
using ForgingDwarf.Server.Repositories;

static void Main()
{
    // Инициализация БД
    using (var db = new AppDbContext())
    {
        db.Database.EnsureCreated();
    }

    var orderRepo = new OrderRepository();
    // Здесь будет TCP/HTTP-сервер
}