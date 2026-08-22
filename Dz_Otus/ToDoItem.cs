using System;
using System.Collections.Generic;
using System.Text;

namespace Dz_5_Prosto_al
{
    public class ToDoItem
    {
        public Guid Id { get; }
        public ToDoUser User { get; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; }
        public ToDoItemState State { get; private set; }
        public DateTime? StateChangedAt { get; private set; }

        public ToDoItem(ToDoUser user, string name)
        {
            Id = Guid.NewGuid();
            User = user;
            Name = name;
            CreatedAt = DateTime.UtcNow;
            State = ToDoItemState.Active;
            StateChangedAt = null;
        }

        /// Отметить задачу как выполненную
        public void Complete()
        {
            if (State == ToDoItemState.Completed)
                return;

            State = ToDoItemState.Completed;
            StateChangedAt = DateTime.UtcNow;
        }

        /// Отметить задачу как активную (отменить выполнение)

    }
}
