using Dz_Otus.Core.DataAccess;
using Dz_Otus.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dz_Otus.Infrastructure.DataAccess
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<ToDoUser> _users = new List<ToDoUser>();

        public ToDoUser? GetUser(Guid userId)
        {
            return _users.FirstOrDefault(u => u.UserId == userId);
        }

        public ToDoUser? GetUserByTelegramUserId(long telegramUserId)
        {
            return _users.FirstOrDefault(u => u.TelegramUserId == telegramUserId);
        }

        public void Add(ToDoUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Пользователь не может быть null.");

            // Проверяем, нет ли уже такого пользователя
            if (_users.Any(u => u.UserId == user.UserId))
                throw new InvalidOperationException($"Пользователь с ID '{user.UserId}' уже существует.");

            if (_users.Any(u => u.TelegramUserId == user.TelegramUserId))
                throw new InvalidOperationException($"Пользователь с Telegram ID '{user.TelegramUserId}' уже существует.");

            _users.Add(user);
        }
    }
}