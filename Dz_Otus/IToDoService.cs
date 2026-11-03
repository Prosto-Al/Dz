using System;
using System.Collections.Generic;
using System.Text;

namespace Dz
{
    public interface IToDoService

    {

        IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId);
        IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);
        ToDoItem Add(ToDoUser user, string name, int maxTasks, int maxTaskLength);
        void MarkCompleted(Guid id); 
        void Delete(Guid id);
    }
}
