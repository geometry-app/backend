using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using GeometryApp.Repositories;
using GeometryApp.Repositories.Entities.Users;

namespace GeometryApp.API
{
    public static class AuthHelper
    {
        public static async Task<UserEntity> GetUser(this IUserRepository repository, HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-App-Token", out var values))
                return null;
            var value = values.FirstOrDefault();
            if (value == null)
                return null;
            return await repository.GetByToken(value);
        }

        public static Token GenerateToken(Guid userId)
        {
            return new Token()
            {
                Expired = DateTime.Now.AddYears(1),
                RawToken = GenerateToken(),
                UserId = userId
            };
        }

        private static string GenerateToken()
        {
            return (Guid.NewGuid().ToString() + Guid.NewGuid()).Replace("-", "");
        }
    }
}