using System;
using System.Runtime.Serialization;

namespace GeometryApp.Repositories.Entities.Users
{
    public class UserEntity
    {
        [DataMember(Name = "id")] public Guid Id;
        [DataMember(Name = "username")] public string UserName;
        [DataMember(Name = "accountid")] public int AccountId;
        [DataMember(Name = "userid")] public int UserId;
    }
}