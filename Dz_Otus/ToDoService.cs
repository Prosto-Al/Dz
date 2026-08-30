using Dz;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dz
{
    public class ToDoService : IToDoService
    { 

        
        private readonly List<ToDoItem> _tasks = new List<ToDoItem>();

        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            return _tasks
                .Where(t => t.User.UserId == userId)
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            return _tasks
                .Where(t => t.User.UserId == userId && t.State == ToDoItemState.Active)
                .ToList()
                .AsReadOnly();
        }

        public ToDoItem Add(ToDoUser user, string name, int maxTasks, int maxTaskLength)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Пользователь не может быть null.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название задачи не может быть пустым.", nameof(name));

            var userTasks = _tasks.Count(t => t.User.UserId == user.UserId);
            if (userTasks >= maxTasks)
                throw new TaskCountLimitException(maxTasks);

            if (name.Length > maxTaskLength)
                throw new TaskLengthLimitException(name.Length, maxTaskLength);

            if (_tasks.Any(t => t.User.UserId == user.UserId &&
                                t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new DuplicateTaskException(name);

            var newTask = new ToDoItem(user, name);
            _tasks.Add(newTask);

            return newTask;
        }

        public void MarkCompleted(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                throw new ArgumentException($"Задача с ID '{id}' не найдена.");

            if (task.State == ToDoItemState.Completed)
                return;

            task.Complete();
        }

        public void Delete(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                throw new ArgumentException($"Задача с ID '{id}' не найдена.");

            _tasks.Remove(task);
        }
    }
}