namespace XPike.Drivers.Http.Declarative
{
    public enum HttpFormat
    {
        Unknown = 0,

        FormEncoded = 1,

        Json = 2,

        Bson = 3,

        // ReSharper disable once InconsistentNaming
        XML = 4,

        // ReSharper disable once InconsistentNaming
        gRPC = 5
    }
}