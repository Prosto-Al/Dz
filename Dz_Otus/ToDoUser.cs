using System;

namespace Dz
{
    public class ToDoUser
    {

        public Guid UserId { get; }
        public long TelegramUserId { get; }        // ← НОВОЕ СВОЙСТВО
        public string TelegramUserName { get; }
        public DateTime RegisteredAt { get; }

        public ToDoUser(long telegramUserId, string telegramUserName)
        {
            UserId = Guid.NewGuid();
            TelegramUserId = telegramUserId;        // ← СОХРАНЯЕМ ID
            TelegramUserName = telegramUserName;
            RegisteredAt = DateTime.UtcNow;
        }
    }
}