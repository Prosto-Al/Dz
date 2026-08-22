using System;
using System.Collections.Generic;
using System.Text;

namespace Dz_5_Prosto_al
{
    public class ToDoUser
    {
        public Guid UserId { get; }
        public string TelegramUserName { get; }
        public DateTime RegisteredAt { get; }
        public List<ToDoItem> Tasks { get; }

        public ToDoUser(string telegramUserName)
        {
            UserId = Guid.NewGuid();
            TelegramUserName = telegramUserName;
            RegisteredAt = DateTime.UtcNow;
            Tasks = new List<ToDoItem>();
        }

    }
}
