using Dz_Otus.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dz_Otus.Core.DataAccess
{
    public interface IUserRepository
    {    
        ToDoUser? GetUser(Guid userId);
      
        ToDoUser? GetUserByTelegramUserId(long telegramUserId);

        void Add(ToDoUser user);
    }
}
