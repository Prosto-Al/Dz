using Dz_Otus.Core.Entities;
using System;
using System.Collections.Generic;

namespace Dz_Otus.Core.Services
{
    public interface IToDoService
    {
        IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId);
        IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);
        ToDoItem Add(ToDoUser user, string name, int maxTasks, int maxTaskLength);
        void MarkCompleted(Guid id);
        void Delete(Guid id);

        IReadOnlyList<ToDoItem> Find(ToDoUser user, string namePrefix);
    }
}