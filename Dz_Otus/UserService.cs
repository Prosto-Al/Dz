
using System;
using System.Collections.Generic;

namespace Dz
{
    public class UserService : IUserService
    {

        // Хранилище пользователей (в памяти)
        private readonly Dictionary<long, ToDoUser> _users = new Dictionary<long, ToDoUser>();

        public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
        {
            // Проверяем, что ID не пустой
            if (telegramUserId <= 0)
            {
                throw new ArgumentException("Telegram User ID должен быть больше 0.", nameof(telegramUserId));
            }

            // Проверяем, что имя не пустое
            if (string.IsNullOrWhiteSpace(telegramUserName))
            {
                throw new ArgumentException("Имя пользователя не может быть пустым.", nameof(telegramUserName));
            }

            // Проверяем, не зарегистрирован ли уже пользователь с таким ID
            if (_users.ContainsKey(telegramUserId))
            {
                throw new InvalidOperationException($"Пользователь с Telegram ID '{telegramUserId}' уже зарегистрирован.");
            }

            // Создаём нового пользователя
            var newUser = new ToDoUser(telegramUserId, telegramUserName);
            _users[telegramUserId] = newUser;

            return newUser;
        }

        public ToDoUser? GetUser(long telegramUserId)
        {
            // Проверяем, что ID не пустой
            if (telegramUserId <= 0)
            {
                return null;
            }

            // Пытаемся найти пользователя
            _users.TryGetValue(telegramUserId, out ToDoUser? user);
            return user;
        }
    }
}