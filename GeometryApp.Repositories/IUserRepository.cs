using System;
using System.Threading.Tasks;
using GeometryApp.Repositories.Entities.Users;

namespace GeometryApp.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity> Create(string userName, int accountId, int userId);
        Task<UserEntity> GetByUserName(string userName);
        Task<UserEntity> GetByAccountId(int accountId);
        Task<UserEntity> GetByToken(string token);
        Task CreateAccessToken(Token token);
        Task UpdateName(UserEntity user, string userName);
        Task AddCode(LoginCodeEntity codeEntity);
        Task<LoginCodeEntity> GetSentCode(string userName, Guid key);
    }
}
