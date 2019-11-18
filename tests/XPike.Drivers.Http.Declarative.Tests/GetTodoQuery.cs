using System;
using System.Runtime.Serialization;
using ProtoBuf;
using XPike.Drivers.Declarative;

namespace XPike.Drivers.Http.Declarative.Tests
{
    [Serializable]
    [DataContract]
    [ProtoContract]
    [HttpRoute(HttpVerb.Get, "/todos/{Id}", HttpFormat.Json)]
    public class GetTodoQuery
        : IRespondWith<GetTodoResponse>
    {
        [DataMember]
        [ProtoMember(1)]
        public int Id { get; set; }
    }
}