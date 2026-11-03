using Otus.ToDoList.ConsoleBot;
using System;

namespace Dz
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            try
            {
               
                // Создаём сервисы
                var userService = new UserService();
                var todoService = new ToDoService();

                // Передаём сервисы в обработчик через конструктор
                var updateHandler = new UpdateHandler(userService, todoService);

                var botClient = new ConsoleBotClient();

                Console.WriteLine("🤖 Бот запущен!");
                Console.WriteLine("Введите команду. Используйте /help для справки.");
                Console.WriteLine("Для выхода используйте Ctrl+C");

                botClient.StartReceiving(updateHandler);

                Console.WriteLine("👋 Бот остановлен. До свидания!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ КРИТИЧЕСКАЯ ОШИБКА: {ex.Message}");
                Console.WriteLine($"📍 {ex.StackTrace}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}