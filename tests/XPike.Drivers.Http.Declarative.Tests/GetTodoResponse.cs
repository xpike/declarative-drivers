using System;
using System.Runtime.Serialization;
using ProtoBuf;
using XPike.Drivers.Declarative;

namespace XPike.Drivers.Http.Declarative.Tests
{
    [Serializable]
    [DataContract]
    [ProtoContract]
    public class GetTodoResponse
        : Todo,
          IRespondTo<GetTodoQuery>
    {
    }
}