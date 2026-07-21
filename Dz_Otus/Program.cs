using System;
using System.Text;

namespace Dz_3_Prosto_al
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string userName = null, command = null, task = null, taskNumber = null;
            bool isStarted = false;
            bool isRunning = true;
            List<string> taskList = new List<string>(50);


            Console.WriteLine("Вам доступны следующие команды:");
            Console.WriteLine("/start, /addtask, /showtasks, /removetask, /help, /info, /echo, /exit");
            Console.Write("Пожалуйста, напишите команду: ");

            while (isRunning == true)
            {

                command = Console.ReadLine();


                // Команда /start
                if (command == "/start")
                {
                    if (!isStarted)
                    {
                        Console.Write("Введите имя: ");
                        userName = Console.ReadLine();
                        isStarted = true;
                        Console.WriteLine($"Привет, {userName}! Программа запущена."); ;
                    }
                    else
                    {
                        Console.WriteLine($"Вы уже запустили программу, {userName}");
                    }
                }
                // Команда /addtask
                else if (command == "/addtask")
                {
                    if (!isStarted)
                    {
                        Console.WriteLine("Программа не запущена. Используйте /start");
                    }
                    else
                    {

                        Console.WriteLine($"{userName}, напишите вашу задачу");
                        task = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(task))
                        {
                            taskList.Add(task);
                            Console.WriteLine($"Задача '{task}' добавлена в список.");
                        }
                        else
                        {
                            Console.WriteLine("Вы не ввели задачу.");
                        }

                    }
                }
                // Команда /showtasks
                else if (command == "/showtasks")
                {
                    if (!isStarted)
                    {
                        Console.WriteLine("Программа не запущена. Используйте /start");
                    }
                    else
                    {
                        Console.WriteLine($"Ваши задачи");

                        if (taskList.Count == 0)
                        {
                            Console.WriteLine("Список задач пуст.");
                        }
                        else
                        {
                            for (int i = 0; i < taskList.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {taskList[i]}");
                            }
                        }
                    }
                }
                // Команда /removetask
                else if (command == "/removetask")
                {
                    if (!isStarted)
                    {
                        Console.WriteLine("Программа не запущена. Используйте /start");
                    }
                    else if (taskList.Count == 0)
                    {
                        Console.WriteLine("Список задач пуст.");
                    }
                    else
                    {
                        Console.WriteLine("Список задач:");
                        for (int i = 0; i < taskList.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {taskList[i]}");
                        }

                        Console.Write("Введите номер задачи для удаления: ");
                        string taskNumberInput = Console.ReadLine();

                        if (int.TryParse(taskNumberInput, out int taskIndex) &&
                            taskIndex > 0 &&
                            taskIndex <= taskList.Count)
                        {
                            string removedTask = taskList[taskIndex - 1];
                            taskList.RemoveAt(taskIndex - 1);
                            Console.WriteLine($"Задача '{removedTask}' удалена из списка.");
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: неверный номер задачи.");
                        }
                    }
                }



                // Команда /help
                else if (command == "/help")
                {
                    if (!isStarted)
                    {
                        Console.WriteLine("Программа не запущена. Используйте /start");
                    }
                    else
                    {
                        Console.WriteLine($"Пользователь: {userName}");
                        Console.WriteLine("/start - запустить программу");
                        Console.WriteLine("/addtask - добавить задачу");
                        Console.WriteLine("/showtasks - посмотреть задачи");
                        Console.WriteLine("/removetask - удалить задачу по номеру");
                        Console.WriteLine("/help - показать справку");
                        Console.WriteLine("/info - информация о программе");
                        Console.WriteLine("/echo [текст] - вернуть введенный текст");
                        Console.WriteLine("/exit - выйти из программы");
                    }
                }

                // Команда /info
                else if (command == "/info")
                {
                    if (!isStarted)
                    {
                        Console.WriteLine("Программа не запущена. Используйте /start");
                    }
                    else
                    {
                        Console.WriteLine($"Пользователь: {userName}");
                        Console.WriteLine("Версия: 0.0.4");
                        Console.WriteLine("Дата создания: 01.07.26");
                        Console.WriteLine("Дата изменения: 18.07.26");
                    }
                }

                // Команда /echo
                else if (command.StartsWith("/echo"))
                {
                    if (!isStarted)
                    {
                        Console.WriteLine("Программа не запущена. Используйте /start");

                    }
                    else
                    {
                        Console.WriteLine(command.Substring(6));
                    }
                }

                // Команда /exit
                else if (command == "/exit")
                {
                    if (isStarted)
                    {
                        Console.WriteLine($"До свидания, {userName}!");
                    }
                    else
                    {
                        Console.WriteLine("Пока, незнакомец!");
                    }
                    isRunning = false;
                }


                else
                {
                    Console.WriteLine("Неизвестная команда. Используйте /help для справки.");

                }
                Console.Write("Введите команду:    ");

            }
        }
    }
}