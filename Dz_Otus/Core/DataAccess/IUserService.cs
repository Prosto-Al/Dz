using Dz_Otus.Core.Entities;
using System;

namespace Dz_Otus.Core.DataAccess
{
    public interface IUserService
    {
        ToDoUser RegisterUser(long telegramUserId, string telegramUserName);
        ToDoUser? GetUser(long telegramUserId);
    }
}