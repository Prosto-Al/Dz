using Dz_Otus.Core.Entities;
using Dz_Otus.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dz_Otus.Infrastructure.DataAccess
{
    public class InMemoryToDoRepository : IToDoRepository
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

        public ToDoItem? Get(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void Add(ToDoItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Задача не может быть null.");

            if (_tasks.Any(t => t.Id == item.Id))
                throw new InvalidOperationException($"Задача с ID '{item.Id}' уже существует.");

            _tasks.Add(item);
        }

        public void Update(ToDoItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Задача не может быть null.");

            int index = _tasks.FindIndex(t => t.Id == item.Id);
            if (index == -1)
                throw new ArgumentException($"Задача с ID '{item.Id}' не найдена.");

            _tasks[index] = item;
        }

        public void Delete(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                throw new ArgumentException($"Задача с ID '{id}' не найдена.");

            _tasks.Remove(task);
        }

        public bool ExistsByName(Guid userId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return _tasks.Any(t => t.User.UserId == userId &&
                                   t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public int CountActive(Guid userId)
        {
            return _tasks.Count(t => t.User.UserId == userId && t.State == ToDoItemState.Active);
        }

        // ✅ РЕАЛИЗАЦИЯ МЕТОДА Find
        public IReadOnlyList<ToDoItem> Find(Guid userId, Func<ToDoItem, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate), "Предикат не может быть null.");

            return _tasks
                .Where(t => t.User.UserId == userId && predicate(t))
                .ToList()
                .AsReadOnly();
        }
    }
}