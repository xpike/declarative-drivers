using System;
using System.Runtime.Serialization;
using ProtoBuf;
using XPike.Contracts;

namespace XPike.Drivers.Http.Declarative.Tests
{
    [Serializable]
    [DataContract]
    [ProtoContract]
    [ProtoInclude(100, typeof(GetTodoResponse))]
    [ProtoInclude(200, typeof(CreateTodoResponse))]
    public class Todo
        : IModel
    {
        [DataMember]
        [ProtoMember(1)]
        public int UserId { get; set; }

        [DataMember]
        [ProtoMember(2)]
        public int Id { get; set; }

        [DataMember]
        [ProtoMember(3)]
        public string Title { get; set; }

        [DataMember]
        [ProtoMember(4)]
        public bool Completed { get; set; }
    }
}