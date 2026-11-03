using Dz_Otus.Core.Entities;
using Dz_Otus.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using Dz_Otus.Core.Exceptionn;

namespace Dz_Otus.Core.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _todoRepository;

        public ToDoService(IToDoRepository todoRepository)
        {
            _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
        }

        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            return _todoRepository.GetAllByUserId(userId);
        }

        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            return _todoRepository.GetActiveByUserId(userId);
        }

        public ToDoItem Add(ToDoUser user, string name, int maxTasks, int maxTaskLength)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Пользователь не может быть null.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название задачи не может быть пустым.", nameof(name));

            int activeCount = _todoRepository.CountActive(user.UserId);
            if (activeCount >= maxTasks)
                throw new TaskCountLimitException(maxTasks);

            if (name.Length > maxTaskLength)
                throw new TaskLengthLimitException(name.Length, maxTaskLength);

            if (_todoRepository.ExistsByName(user.UserId, name))
                throw new DuplicateTaskException(name);

            var newTask = new ToDoItem(user, name);
            _todoRepository.Add(newTask);

            return newTask;
        }

        public void MarkCompleted(Guid id)
        {
            var task = _todoRepository.Get(id);
            if (task == null)
                throw new ArgumentException($"Задача с ID '{id}' не найдена.");

            if (task.State == ToDoItemState.Completed)
                return;

            task.Complete();
            _todoRepository.Update(task);
        }

        public void Delete(Guid id)
        {
            var task = _todoRepository.Get(id);
            if (task == null)
                throw new ArgumentException($"Задача с ID '{id}' не найдена.");

            _todoRepository.Delete(id);
        }

        // ✅ РЕАЛИЗАЦИЯ МЕТОДА Find
        public IReadOnlyList<ToDoItem> Find(ToDoUser user, string namePrefix)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Пользователь не может быть null.");

            if (string.IsNullOrWhiteSpace(namePrefix))
                return Array.Empty<ToDoItem>();

            // ✅ ИСПОЛЬЗУЕМ IToDoRepository.Find С ЛЯМБДА-ПРЕДИКАТОМ
            return _todoRepository.Find(user.UserId,
                task => task.Name.StartsWith(namePrefix, StringComparison.OrdinalIgnoreCase));
        }
    }
}