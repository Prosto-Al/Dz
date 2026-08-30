using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dz
{
    public class UpdateHandler : IUpdateHandler
    {

        private readonly IUserService _userService;
        private readonly IToDoService _todoService;

        private ToDoUser _currentUser = null;
        private int _maxTasks = 5;
        private int _maxTaskLength = 20;

        public UpdateHandler(IUserService userService, IToDoService todoService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _todoService = todoService ?? throw new ArgumentNullException(nameof(todoService));
        }

        public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
        {
            try
            {
                string command = update.Message?.Text?.Trim();

                if (string.IsNullOrEmpty(command))
                {
                    botClient.SendMessage(update.Message.Chat, "Введите команду. Используйте /help для справки.");
                    return;
                }

                var chat = update.Message.Chat;

                // Получаем или регистрируем пользователя
                var user = _userService.GetUser(chat.Id);
                if (user == null)
                {
                    string userName = update.Message.From?.Username ?? "User";
                    user = _userService.RegisterUser(chat.Id, userName);

                    botClient.SendMessage(chat, $"👋 Привет, {user.TelegramUserName}!");
                    botClient.SendMessage(chat, $"📌 Ваш ID: {user.UserId}");
                    botClient.SendMessage(chat, $"📌 Telegram ID: {user.TelegramUserId}");
                    botClient.SendMessage(chat, "💡 Используйте /start для настройки программы.");
                }

                _currentUser = user;

                // Обработка команд
                switch (command)
                {
                    case "/start":
                        HandleStart(botClient, chat);
                        break;

                    case "/addtask":
                        if (!IsUserRegistered(botClient, chat)) return;
                        HandleAddTask(botClient, chat, command);
                        break;

                    case "/showtasks":
                        if (!IsUserRegistered(botClient, chat)) return;
                        HandleShowTasks(botClient, chat);
                        break;

                    case "/showalltasks":
                        if (!IsUserRegistered(botClient, chat)) return;
                        HandleShowAllTasks(botClient, chat);
                        break;

                    case "/completetask":
                        if (!IsUserRegistered(botClient, chat)) return;
                        HandleCompleteTask(botClient, chat, command);
                        break;

                    case "/removetask":
                        if (!IsUserRegistered(botClient, chat)) return;
                        HandleRemoveTask(botClient, chat, command);
                        break;

                    case "/help":
                        HandleHelp(botClient, chat);
                        break;

                    case "/info":
                        HandleInfo(botClient, chat);
                        break;

                    case "/exit":
                        HandleExit(botClient, chat);
                        break;

                    default:
                        if (command.StartsWith("/addtask "))
                        {
                            if (!IsUserRegistered(botClient, chat)) return;
                            HandleAddTask(botClient, chat, command);
                        }
                        else if (command.StartsWith("/removetask "))
                        {
                            if (!IsUserRegistered(botClient, chat)) return;
                            HandleRemoveTask(botClient, chat, command);
                        }
                        else if (command.StartsWith("/completetask "))
                        {
                            if (!IsUserRegistered(botClient, chat)) return;
                            HandleCompleteTask(botClient, chat, command);
                        }
                        else
                        {
                            botClient.SendMessage(chat, "❌ Неизвестная команда. Используйте /help для справки.");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                // ✅ ОДИН catch ДЛЯ ВСЕХ ИСКЛЮЧЕНИЙ
                botClient.SendMessage(update.Message.Chat, $"❌ Ошибка: {ex.Message}");
                Console.WriteLine($"📌 Тип ошибки: {ex.GetType().FullName}");
                Console.WriteLine($"📝 Сообщение: {ex.Message}");
                Console.WriteLine($"📍 Стек вызовов:\n{ex.StackTrace}");
            }
        }

        // ========== ВСПОМОГАТЕЛЬНЫЙ МЕТОД ==========

        private bool IsUserRegistered(ITelegramBotClient botClient, Chat chat)
        {
            if (_currentUser == null)
            {
                botClient.SendMessage(chat, "❌ Пользователь не зарегистрирован. Используйте /start для регистрации.");
                return false;
            }

            var user = _userService.GetUser(chat.Id);
            if (user == null)
            {
                botClient.SendMessage(chat, "❌ Пользователь не найден. Пожалуйста, зарегистрируйтесь заново через /start.");
                return false;
            }

            return true;
        }

        // ========== ОБРАБОТЧИКИ КОМАНД ==========

        private void HandleStart(ITelegramBotClient botClient, Chat chat)
        {
            if (_currentUser == null)
            {
                botClient.SendMessage(chat, "❌ Ошибка: пользователь не зарегистрирован.");
                return;
            }

            _maxTasks = 5;
            _maxTaskLength = 20;

            botClient.SendMessage(chat, $"✅ Привет, {_currentUser.TelegramUserName}!");
            botClient.SendMessage(chat, $"📌 Ваш ID: {_currentUser.UserId}");
            botClient.SendMessage(chat, $"📌 Telegram ID: {_currentUser.TelegramUserId}");
            botClient.SendMessage(chat, $"📊 Максимальное количество задач: {_maxTasks}");
            botClient.SendMessage(chat, $"📏 Максимальная длина задачи: {_maxTaskLength} символов");
            botClient.SendMessage(chat, "💡 Программа запущена! Используйте /help для справки.");
        }

        private void HandleAddTask(ITelegramBotClient botClient, Chat chat, string command)
        {
            if (_currentUser == null)
            {
                botClient.SendMessage(chat, "❌ Программа не запущена. Используйте /start");
                return;
            }

            string taskName = command.Length > "/addtask ".Length
                ? command.Substring("/addtask ".Length).Trim()
                : "";

            if (string.IsNullOrWhiteSpace(taskName))
            {
                botClient.SendMessage(chat, "❌ Введите название задачи. Пример: /addtask Купить молоко");
                return;
            }

            // ✅ ВСЯ ЛОГИКА В ToDoService
            var newTask = _todoService.Add(_currentUser, taskName, _maxTasks, _maxTaskLength);

            botClient.SendMessage(chat, $"✅ Задача '{taskName}' добавлена в список.");
            botClient.SendMessage(chat, $"🆔 ID задачи: {newTask.Id}");
            botClient.SendMessage(chat, $"📊 Всего задач: {_todoService.GetAllByUserId(_currentUser.UserId).Count}/{_maxTasks}");
        }

        private void HandleShowTasks(ITelegramBotClient botClient, Chat chat)
        {
            if (_currentUser == null)
            {
                botClient.SendMessage(chat, "❌ Программа не запущена. Используйте /start");
                return;
            }

            var activeTasks = _todoService.GetActiveByUserId(_currentUser.UserId);

            if (activeTasks.Count == 0)
            {
                botClient.SendMessage(chat, "📭 Нет активных задач.");
                return;
            }

            botClient.SendMessage(chat, $"📋 Активные задачи ({activeTasks.Count}):");
            for (int i = 0; i < activeTasks.Count; i++)
            {
                var task = activeTasks[i];
                botClient.SendMessage(chat, $"{i + 1}. {task.Name} - {task.Id}");
            }
        }

        private void HandleShowAllTasks(ITelegramBotClient botClient, Chat chat)
        {
            if (_currentUser == null)
            {
                botClient.SendMessage(chat, "❌ Программа не запущена. Используйте /start");
                return;
            }

            var allTasks = _todoService.GetAllByUserId(_currentUser.UserId);

            if (allTasks.Count == 0)
            {
                botClient.SendMessage(chat, "📭 Список задач пуст.");
                return;
            }

            botClient.SendMessage(chat, $"📋 Все задачи ({allTasks.Count}):");
            for (int i = 0; i < allTasks.Count; i++)
            {
                var task = allTasks[i];
                string stateText = task.State == ToDoItemState.Active ? "🟢 Active" : "🔴 Completed";
                botClient.SendMessage(chat, $"{i + 1}. {stateText} - {task.Name} - {task.Id}");
            }
        }

        private void HandleCompleteTask(ITelegramBotClient botClient, Chat chat, string command)
        {
            if (_currentUser == null)
            {
                botClient.SendMessage(chat, "❌ Программа не запущена. Используйте /start");
                return;
            }

            string idString = command.Length > "/completetask ".Length
                ? command.Substring("/completetask ".Length).Trim()
                : "";

            if (string.IsNullOrWhiteSpace(idString))
            {
                botClient.SendMessage(chat, "❌ Введите ID задачи. Пример: /completetask 73c7940a-ca8c-4327-8a15-9119bffd1d5e");
                return;
            }

            if (!Guid.TryParse(idString, out Guid taskId))
            {
                botClient.SendMessage(chat, "❌ Неверный формат ID.");
                return;
            }

            // ✅ ИСПОЛЬЗУЕМ IToDoService.MarkCompleted
            _todoService.MarkCompleted(taskId);

            botClient.SendMessage(chat, $"✅ Задача отмечена как выполненная!");
        }

        private void HandleRemoveTask(ITelegramBotClient botClient, Chat chat, string command)
        {
            if (_currentUser == null)
            {
                botClient.SendMessage(chat, "❌ Программа не запущена. Используйте /start");
                return;
            }

            string idString = command.Length > "/removetask ".Length
                ? command.Substring("/removetask ".Length).Trim()
                : "";

            if (string.IsNullOrWhiteSpace(idString))
            {
                botClient.SendMessage(chat, "❌ Введите ID задачи. Пример: /removetask 73c7940a-ca8c-4327-8a15-9119bffd1d5e");
                return;
            }

            if (!Guid.TryParse(idString, out Guid taskId))
            {
                botClient.SendMessage(chat, "❌ Неверный формат ID.");
                return;
            }

            // Проверяем, что задача принадлежит пользователю
            var allTasks = _todoService.GetAllByUserId(_currentUser.UserId);
            if (!allTasks.Any(t => t.Id == taskId))
            {
                botClient.SendMessage(chat, $"❌ Задача с ID '{taskId}' не найдена у этого пользователя.");
                return;
            }

            _todoService.Delete(taskId);
            botClient.SendMessage(chat, $"✅ Задача удалена!");
        }

        private void HandleHelp(ITelegramBotClient botClient, Chat chat)
        {
            string userName = _currentUser?.TelegramUserName ?? "незнакомец";

            botClient.SendMessage(chat, $"📚 Справка для {userName}:");
            botClient.SendMessage(chat, "/start - запустить программу");
            botClient.SendMessage(chat, "/addtask [название] - добавить задачу");
            botClient.SendMessage(chat, "/showtasks - показать активные задачи");
            botClient.SendMessage(chat, "/showalltasks - показать все задачи");
            botClient.SendMessage(chat, "/completetask [ID] - отметить задачу как выполненную");
            botClient.SendMessage(chat, "/removetask [ID] - удалить задачу по ID");
            botClient.SendMessage(chat, "/help - показать справку");
            botClient.SendMessage(chat, "/info - информация о программе");
            botClient.SendMessage(chat, "/exit - выйти из программы");
        }

        private void HandleInfo(ITelegramBotClient botClient, Chat chat)
        {
            botClient.SendMessage(chat, "📊 Информация о программе:");
            botClient.SendMessage(chat, "📦 Версия: 0.4.0");
            botClient.SendMessage(chat, "📅 Дата создания: 01.07.26");
            botClient.SendMessage(chat, "📅 Дата изменения: 22.07.26");

            if (_currentUser != null)
            {
                var allTasks = _todoService.GetAllByUserId(_currentUser.UserId);
                var activeTasks = _todoService.GetActiveByUserId(_currentUser.UserId);
                botClient.SendMessage(chat, $"👤 Пользователь: {_currentUser.TelegramUserName}");
                botClient.SendMessage(chat, $"📋 Всего задач: {allTasks.Count}/{_maxTasks}");
                botClient.SendMessage(chat, $"🟢 Активных задач: {activeTasks.Count}");
            }
        }

        private void HandleExit(ITelegramBotClient botClient, Chat chat)
        {
            if (_currentUser != null)
            {
                botClient.SendMessage(chat, $"👋 До свидания, {_currentUser.TelegramUserName}!");
                _currentUser = null;
            }
            else
            {
                botClient.SendMessage(chat, "👋 Пока, незнакомец!");
            }
        }
    }
}