using System;
using System.Runtime.Serialization;

namespace GeometryApp.API.Dtos
{
    [DataContract]
    public class UserMinimalInfo
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public string UserName { get; set; }
    }
}