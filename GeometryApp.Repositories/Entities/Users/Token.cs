using System;

namespace GeometryApp.Repositories.Entities.Users
{
    public class Token
    {
        public string RawToken;
        public DateTime Expired;
        public Guid UserId;

        public override string ToString()
        {
            return RawToken;
        }
    }
}