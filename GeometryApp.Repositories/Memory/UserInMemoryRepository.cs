using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeometryApp.Repositories.Entities.Users;

namespace GeometryApp.Repositories.Memory
{
    public class UserInMemoryRepository
    {
        private Dictionary<Guid, UserEntity> users = new();
        private Dictionary<string, Guid> tokens = new();
        
        public UserEntity Create(string userName, int accountId, int userId)
        {
            var id = Guid.NewGuid();
            users.Add(id, new UserEntity()
            {
                Id = id,
                UserId = userId,
                UserName = userName,
                AccountId = accountId
            });
            return users[id];
        }

        public string GenerateAccessToken(Guid id)
        {
            var token = Guid.NewGuid().ToString();
            tokens.Add(token, id);
            return token;
        }

        public bool TryGetUser(string userName, out UserEntity user)
        {
            user = users.Values.FirstOrDefault(x => x.UserName == userName);
            return user != null;
        }

        public Task<UserEntity> Get(string token)
        {
            if (tokens.TryGetValue(token, out var user))
                return Task.FromResult(users[user]);
            return Task.FromResult<UserEntity>(null);
        }
    }
}
