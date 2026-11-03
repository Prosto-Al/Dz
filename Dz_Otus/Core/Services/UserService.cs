// UserService.cs
using Dz_Otus.Core.DataAccess;
using Dz_Otus.Core.Entities;
using System;

namespace Dz_Otus.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
        {
            if (telegramUserId <= 0)
                throw new ArgumentException("Telegram User ID должен быть больше 0.", nameof(telegramUserId));

            if (string.IsNullOrWhiteSpace(telegramUserName))
                throw new ArgumentException("Имя пользователя не может быть пустым.", nameof(telegramUserName));

            var existingUser = _userRepository.GetUserByTelegramUserId(telegramUserId);
            if (existingUser != null)
                throw new InvalidOperationException($"Пользователь с Telegram ID '{telegramUserId}' уже зарегистрирован.");

            var newUser = new ToDoUser(telegramUserId, telegramUserName);
            _userRepository.Add(newUser);

            return newUser;
        }

        public ToDoUser? GetUser(long telegramUserId)
        {
            if (telegramUserId <= 0)
                return null;

            return _userRepository.GetUserByTelegramUserId(telegramUserId);
        }
    }
}