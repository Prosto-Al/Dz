using System;
using Otus.ToDoList.ConsoleBot;
using Dz_Otus.Infrastructure.DataAccess;
using Dz_Otus.Core.Services;            
using Dz_Otus.TelegramBot;              

namespace Dz_Otus
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            try
            {
                // ✅ Создаём репозитории
                var userRepository = new InMemoryUserRepository();
                var todoRepository = new InMemoryToDoRepository();

                // ✅ Создаём сервисы
                var userService = new UserService(userRepository);
                var todoService = new ToDoService(todoRepository);
                var reportService = new ToDoReportService(todoRepository);  // ✅ НОВЫЙ СЕРВИС

                // ✅ Передаём все три сервиса в UpdateHandler
                var updateHandler = new UpdateHandler(userService, todoService, reportService);

                var botClient = new ConsoleBotClient();

                Console.WriteLine("Бот запущен!");
                Console.WriteLine("Введите команду. Используйте /help для справки.");
                Console.WriteLine("Для выхода используйте Ctrl+C");

                botClient.StartReceiving(updateHandler);

                Console.WriteLine("👋 Бот остановлен. До свидания!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ПУ-ПУ-ПУ КРИТИЧЕСКАЯ ОШИБКА: {ex.Message}");
                Console.WriteLine($" {ex.StackTrace}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}