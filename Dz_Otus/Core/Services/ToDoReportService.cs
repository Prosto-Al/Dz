using Dz_Otus.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dz_Otus.Core.Services
{
    public class ToDoReportService : IToDoReportService
    {
        private readonly IToDoRepository _todoRepository;

        public ToDoReportService(IToDoRepository todoRepository)
        {
            _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
        }

        public (int total, int completed, int active, DateTime generatedAt) GetUserStats(Guid userId)
        {
            // ✅ Получаем все задачи пользователя
            var allTasks = _todoRepository.GetAllByUserId(userId);

            // Считаем статистику
            int total = allTasks.Count;
            int completed = allTasks.Count(t => t.State == ToDoItemState.Completed);
            int active = allTasks.Count(t => t.State == ToDoItemState.Active);

            // Дата генерации отчёта
            DateTime generatedAt = DateTime.UtcNow;

            // ✅ Возвращаем кортеж
            return (total, completed, active, generatedAt);
        }
    }
}