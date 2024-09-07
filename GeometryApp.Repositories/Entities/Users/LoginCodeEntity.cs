using System;

namespace GeometryApp.Repositories.Entities.Users
{
    public class LoginCodeEntity
    {
        public string UserName;
        public Guid Key;
        public string Code;
        public DateTime SentTime;
    }
}