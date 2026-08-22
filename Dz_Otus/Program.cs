using System;
using System.Collections.Generic;

namespace Dz_5_Prosto_al
{
    internal class Program
    {
        // Глобальные переменные состояния
        private static  ToDoUser _currentUser = null;
        private static int _maxTasks = 0;
        private static int _maxTaskLength = 0;
        private static List<ToDoItem> _taskList = new List<ToDoItem>(100);

        private static void Main(string[] args)
        {
            try
            {
                ShowCommands();

                while (true)
                {
                    try
                    {
                        string command = Console.ReadLine();

                        // Обработка команд через switch
                        switch (command)
                        {
                            case "/start":
                                HandleStart();
                                break;

                            case "/addtask":
                                HandleAddTask();
                                break;

                            case "/showtasks":
                                HandleShowTasks();
                                break;

                            case "/showalltasks": // ⬅️ НОВАЯ КОМАНДА
                                HandleShowAllTasks();
                                break;

                            case "/completetask": // ⬅️ НОВАЯ КОМАНДА
                                HandleCompleteTask(command);
                                break;

                            case "/removetask":
                                HandleRemoveTask();
                                break;

                            case "/help":
                                HandleHelp();
                                break;

                            case "/info":
                                HandleInfo();
                                break;

                            case "/echo":
                                HandleEcho(command);
                                break;

                            case "/exit":
                                if (HandleExit())
                                {
                                    return; // Выход из программы
                                }
                                break;

                            default:
                                if (command != null && command.StartsWith("/echo"))
                                {
                                    HandleEcho(command);
                                }
                                else
                                {
                                    Console.WriteLine("Неизвестная команда. Используйте /help для справки.");
                                }
                                break;
                        }

                        Console.Write("Введите команду: ");
                    }
  
                    catch (DuplicateTaskException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.Write("Введите команду: ");
                    }
           
                    catch (TaskLengthLimitException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.Write("Введите команду: ");
                    }

                    catch (TaskCountLimitException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.Write("Введите команду: ");
                    }
             
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                        Console.Write("Введите команду: ");
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла непредвиденная ошибка:");
                        Console.WriteLine($" Тип ошибки: {ex.GetType().FullName}");
                        Console.WriteLine($" Сообщение: {ex.Message}");
                        Console.WriteLine($" Стек вызовов:\n{ex.StackTrace}");
                        Console.Write("Введите команду: ");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" КРИТИЧЕСКАЯ ОШИБКА! Программа будет закрыта:");
                Console.WriteLine($"📌 Тип ошибки: {ex.GetType().FullName}");
                Console.WriteLine($"📝 Сообщение: {ex.Message}");
                Console.WriteLine($"📍 Стек вызовов:\n{ex.StackTrace}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        // ========== МЕТОДЫ ДЛЯ ОТОБРАЖЕНИЯ ==========

        public static void ShowCommands()
        {
            Console.WriteLine("Вам доступны следующие команды:");
            Console.WriteLine("/start, /addtask, /showtasks, /removetask, /help, /info, /echo, /exit");
            Console.Write("Пожалуйста, напишите команду: ");
        }

        public static void ShowTasks(List<string> taskList)
        {
            if (taskList.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
            }
            else
            {
                Console.WriteLine("Ваши задачи:");
                for (int i = 0; i < taskList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {taskList[i]} (Длина: {taskList[i].Length} символов)");
                }
            }
        }

        public static void Help(string userName)
        {
            Console.WriteLine($"Пользователь: {_currentUser.TelegramUserName}");
            Console.WriteLine("/start - запустить программу");
            Console.WriteLine("/addtask - добавить задачу");
            Console.WriteLine("/showtasks - посмотреть задачи");
            Console.WriteLine("/removetask - удалить задачу по номеру");
            Console.WriteLine("/help - показать справку");
            Console.WriteLine("/info - информация о программе");
            Console.WriteLine("/echo [текст] - вернуть введенный текст");
            Console.WriteLine("/exit - выйти из программы");
        }



        public static void ValidateString(string? str, string paramName = "строка")
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException($"{paramName} не может быть пустой или состоять только из пробелов.");
            }
        }

        public static int ParseAndValidateInt(string? str, int min, int max)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException($"Введите число! Допустимый диапазон: от {min} до {max}.");
            }

            if (!int.TryParse(str, out int result))
            {
                throw new ArgumentException($"Введите корректное число! Допустимый диапазон: от {min} до {max}.");
            }

            if (result < min || result > max)
            {
                throw new ArgumentException($"Число должно быть в диапазоне от {min} до {max}. Введено: {result}");
            }

            return result;
        }


        public static void HandleStart()
        {
            if (_currentUser !=null)
            {
                Console.WriteLine($"Вы уже запустили программу, {_currentUser.TelegramUserName}");
                return;
            }

            Console.Write("Введите имя: ");
            string inputName = Console.ReadLine();
            ValidateString(inputName, "Имя");
            _currentUser = new ToDoUser(inputName);
            Console.WriteLine($"Привет, {_currentUser.TelegramUserName}!");
            Console.WriteLine($"Ваш ID: {_currentUser.UserId}");
            Console.WriteLine($"Дата регистрации: {_currentUser.RegisteredAt:dd.MM.yyyy HH:mm}");

            Console.Write("Введите максимально допустимое количество задач (1-100): ");
            _maxTasks = ParseAndValidateInt(Console.ReadLine(), 1, 100);

            Console.Write("Введите максимально допустимую длину задачи (1-100): ");
            _maxTaskLength = ParseAndValidateInt(Console.ReadLine(), 1, 100);

            Console.WriteLine($"Программа запущена!");
            Console.WriteLine($"Максимальное количество задач: {_maxTasks}");
            Console.WriteLine($"Максимальная длина задачи: {_maxTaskLength} символов");
        }

        public static void HandleAddTask()
        {
            if (_currentUser == null )
            {
                Console.WriteLine("Программа не запущена. Используйте /start");
                return;
            }

            if (_taskList.Count >= _maxTasks)
            {
                throw new TaskCountLimitException(_maxTasks);
            }

            Console.Write($"{_currentUser.TelegramUserName}, напишите вашу задачу: ");
            string task = Console.ReadLine();

            ValidateString(task, "Задача");

            if (task.Length > _maxTaskLength)
            {
                throw new TaskLengthLimitException(task.Length, _maxTaskLength);
            }

            if (_taskList.Exists(t => t.Name.Equals(task, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateTaskException(task);
            }

            var newTask = new ToDoItem(_currentUser, task);
            _taskList.Add(newTask);

            Console.WriteLine($"Задача '{task}' добавлена в список.");
            Console.WriteLine($"ID задачи: {newTask.Id}");
            Console.WriteLine($"Всего задач: {_taskList.Count}/{_maxTasks}");
            Console.WriteLine($"Длина задачи: {task.Length}/{_maxTaskLength} символов");
        }

        public static void HandleShowTasks()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Программа не запущена. Используйте /start");
                return;
            }

            var activeTasks = _taskList.Where(t => t.State == ToDoItemState.Active).ToList();

            if (activeTasks.Count == 0)
            {
                Console.WriteLine("Нет активных задач.");
                return;
            }

            Console.WriteLine("Активные задачи:");
            for (int i = 0; i < activeTasks.Count; i++)
            {
                var task = activeTasks[i];
                Console.WriteLine($"{i + 1}. {task.Name} - {task.CreatedAt:dd.MM.yyyy HH:mm} - {task.Id}");
            }
        }

        public static void HandleRemoveTask()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Программа не запущена. Используйте /start");
                return;
            }

            if (_taskList.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            Console.WriteLine("Список задач:");
            for (int i = 0; i < _taskList.Count; i++)
            {
                var task = _taskList[i];
                string stateText = task.State == ToDoItemState.Active ? "Active" : "Completed";
                Console.WriteLine($"{i + 1}. ({stateText}) {task.Name} - {task.Id}");
            }

            Console.Write("Введите номер задачи для удаления: ");
            string taskNumberInput = Console.ReadLine();

            if (int.TryParse(taskNumberInput, out int taskIndex) &&
                taskIndex > 0 &&
                taskIndex <= _taskList.Count)
            {
                var removedTask = _taskList[taskIndex - 1];
                _taskList.RemoveAt(taskIndex - 1);
                Console.WriteLine($"Задача '{removedTask.Name}' удалена из списка.");
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи.");
            }
        }

        public static void HandleHelp()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Программа не запущена. Используйте /start");
                return;
            }

            Console.WriteLine($"Пользователь: {_currentUser.TelegramUserName}");
            Console.WriteLine($"ID пользователя: {_currentUser.UserId}");
            Console.WriteLine("/start - запустить программу");
            Console.WriteLine("/addtask - добавить задачу");
            Console.WriteLine("/showtasks - показать активные задачи");
            Console.WriteLine("/showalltasks - показать все задачи (включая выполненные)");
            Console.WriteLine("/completetask [ID] - отметить задачу как выполненную");
            Console.WriteLine("/removetask - удалить задачу по номеру");
            Console.WriteLine("/help - показать справку");
            Console.WriteLine("/info - информация о программе");
            Console.WriteLine("/echo [текст] - вернуть введенный текст");
            Console.WriteLine("/exit - выйти из программы");
        }

        public static void HandleInfo()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Программа не запущена. Используйте /start");
                return;
            }

            Console.WriteLine($"Пользователь: {_currentUser.TelegramUserName}");
            Console.WriteLine($"ID пользователя: {_currentUser.UserId}");
            Console.WriteLine($"Дата регистрации: {_currentUser.RegisteredAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine("Версия: 0.2.0");
            Console.WriteLine("Дата создания: 01.07.26");
            Console.WriteLine("Дата изменения: 22.07.26");
        }

        public static void HandleShowAllTasks()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Программа не запущена. Используйте /start");
                return;
            }

            if (_taskList.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            Console.WriteLine("Все задачи:");
            for (int i = 0; i < _taskList.Count; i++)
            {
                var task = _taskList[i];
                string stateText = task.State == ToDoItemState.Active ? "Active" : "Completed";
                string stateChangedText = task.StateChangedAt.HasValue
                    ? $"Завершена: {task.StateChangedAt:dd.MM.yyyy HH:mm}"
                    : "Активна";

                Console.WriteLine($"({stateText}) {task.Name} - {task.CreatedAt:dd.MM.yyyy HH:mm} - {task.Id}");
                Console.WriteLine($"  {stateChangedText}");
            }
        }

        public static void HandleCompleteTask(string command)
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Программа не запущена. Используйте /start");
                return;
            }

            // Парсим ID из команды
            string idString = command.Length > "/completetask ".Length
                ? command.Substring("/completetask ".Length).Trim()
                : "";

            if (string.IsNullOrWhiteSpace(idString))
            {
                Console.WriteLine("Введите ID задачи. Пример: /completetask 73c7940a-ca8c-4327-8a15-9119bffd1d5e");
                return;
            }

            if (!Guid.TryParse(idString, out Guid taskId))
            {
                Console.WriteLine("Неверный формат ID. ID должен быть в формате: 73c7940a-ca8c-4327-8a15-9119bffd1d5e");
                return;
            }

            var task = _taskList.FirstOrDefault(t => t.Id == taskId);

            if (task == null)
            {
                Console.WriteLine($"Задача с ID '{taskId}' не найдена.");
                return;
            }

            if (task.State == ToDoItemState.Completed)
            {
                Console.WriteLine($"Задача '{task.Name}' уже выполнена.");
                return;
            }

            task.Complete();
            Console.WriteLine($"✅ Задача '{task.Name}' отмечена как выполненная!");
            Console.WriteLine($"📅 Дата завершения: {task.StateChangedAt:dd.MM.yyyy HH:mm}");
        }
        public static void HandleEcho(string command)
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Программа не запущена. Используйте /start");
                return;
            }

            string echoText = command.Length > 6 ? command.Substring(6) : "";
            ValidateString(echoText, "Текст для эхо");

            Console.WriteLine(echoText);
        }

        public static bool HandleExit()
        {
            if (_currentUser != null)
            {
                Console.WriteLine($"До свидания, {_currentUser.TelegramUserName}!");
            }
            else
            {
                Console.WriteLine("Пока, незнакомец!");
            }
            return true; // Возвращаем true для выхода из программы
        }
    }
}