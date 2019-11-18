using System;
using System.Runtime.Serialization;
using ProtoBuf;
using XPike.Drivers.Declarative;

namespace XPike.Drivers.Http.Declarative.Tests
{
    [Serializable]
    [DataContract]
    [ProtoContract]
    [HttpRoute(HttpVerb.Post, "/posts", HttpFormat.Json)]
    public class CreateTodoCommand
        : IRespondWith<CreateTodoResponse>
    {
        [DataMember]
        [ProtoMember(1)]
        public string Title { get; set; }

        [DataMember]
        [ProtoMember(2)]
        public string Body { get; set; }

        [DataMember]
        [ProtoMember(3)]
        public int UserId { get; set; }
    }
}