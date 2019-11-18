# XPike.Drivers.Declarative

[![Build Status](https://dev.azure.com/xpike/xpike/_apis/build/status/xpike.drivers.declarative?branchName=master)](https://dev.azure.com/xpike/xpike/_build/latest?definitionId=11&branchName=master)
![Nuget](https://img.shields.io/nuget/v/XPike.Drivers.Declarative)

An implementation of the Driver pattern for strongly-typed communication with a design-by-contract approach.

## Libraries

#### XPike.Drivers.Declarative

Communication-agnostic interfaces to support the Declarative Driver pattern which
encourages a design-by-contract approach to defining inter-service communication.

#### XPike.Drivers.Http.Declarative

The HTTP implementation of xPike's Declarative Drivers which uses `HttpClient` under the hood.

#### XPike.Drivers.Http.Declarative.AspNetCore

Extension methods for integrating within a .NET Core project.

## Exposed Services

#### `HttpDriverOptions`  

Defines default settings for `HttpDriverBase`:
- ProxyUrl
- DefaultTimeout  
  > This should be specified as a TimeSpan string, eg: `00:00:60` for 60 seconds.
 
#### Attributes

- `[HttpRoute(HttpVerb verb, string routeFormat, HttpFormat format)]`  
    Specifies the Route format to use for an endpoint - supports replacement values, eg: `/user/{UserId}`
- `[StatusCode(HttpStatusCode statusCode)]`  
    Specifies the `HttpStatusCode` that an Enum value corresponds to.

#### Interfaces

- `IRespondWith<T>`  
    Used to indicate a response type (usually singular) for a request Contract.
- `IRespondTo<T>`  
    Used to indicate a request type (usually singular) that can generate a response Contract.
- `IDriver`  
    Base definition of a Driver, agnostic of communication method.
  - `IHttpDriver`  
    Definition of an HTTP Driver.
    - `IDriveHttp`  
        Indicates a specific Request-Response interaction supported by an HTTP Driver implementation.

#### Abstract

- `HttpDriverBase`  
    Abstract base class to simplify building an HTTP Driver implementation.

#### Extensions

- `Enum.GetStatusCode()`  
    Returns the value of a `StatusCodeAttribute` for a given Enum value.
- `IRespondWith<TResponse>.GetHttpRoute()`  
    Returns the value of an `HttpRouteAttribute` for an object of the type specified by `TResponse`.

#### Enums

- `HttpVerb`  
    Enumerates the supported HTTP Verbs for `HttpDriverBase`: `GET`, `POST`, `PUT`, `DELETE`
- `HttpFormat`  
    Enumerates HTTP serialization formats that are or will be supported.  
    > While `gRPC` is listed, it is not currently supported by `HttpDriverBase`.

## Usage

#### Register Dependencies

**`Startup.cs`:**

```csharp
using XPike.Drivers.Http.Declarative.AspNetCore;

public void ConfigureServices(IServiceCollection services)
{
    services.AddXPikeDeclarativeDrivers();
}
```

#### Create an SDK Project

SDK projects should expose everything necessary to communicate with a service.
This includes Request-Response Contracts, Models, Enums, and a communication Driver.

No business logic should be included in the project, but things such as necessary serialization logic may make sense.

While it's generally OK to import other SDK packages, any other references should be avoided.

> Each Response Contract should define its own `Status` enum type.
> 
This should have a structure similar to the following:

- `Contracts`  
    All Request-Response Contracts go in one of the sub-directories here.
  - `Commands`  
    These are Request Contracts which result in an action being taken.
  - `Queries`  
    These are Request Contracts which are implicitly read-only operations.
  - `Messages`  
    These are Request Contracts which act as notifications for listeners on a queue/topic.
- `Driver`  
    An appropriate class for communicating with the service should go here.
- `Enums`  
    Enumerations exposed by the Request-Response Contracts or models.
- `Models`  
    Classes here are POCOs exposed by Request-Response Contracts and should implement `IModel`.

#### Define an Enum

```csharp
using System;
using System.Runtime.Serialization;
using ProtoBuf;

[Serializable]
[DataContract]
[ProtoContract]
public enum AccountType
{
    [EnumMember]
    [ProtoEnum]
    Unknown = 0,

    [EnumMember]
    [ProtoEnum]
    Checking = 1,

    [EnumMember]
    [ProtoEnum]
    Savings = 2
}
```

#### Define a Model

```csharp
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProtoBuf;
using XPike.Contracts;

[Serializable]
[DataContract]
[ProtoContract]
public class Account
    : IModel
{
    [DataMember]
    [ProtoMember(1)]
    public long AccountId { get; set; }

    [DataMember]
    [ProtoMember(2)]
    [JsonConverter(typeof(StringEnumConverter))]
    public AccountType AccountType { get; set; }

    [DataMember]
    [ProtoMember(3)]
    public string AccountNumber { get; set; }
}
```

#### Define Contracts

**`GetAccountQuery.cs`:**

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProtoBuf;
using XPike.Drivers.Declarative;
using XPike.Drivers.Http.Declarative;

[Serializable]
[DataContract]
[ProtoContract]
[HttpRoute(HttpVerb.Get, "account/{AccountId}", HttpFormat.Json)]
public class GetAccountQuery
    : IRespondWith<GetAccountResponse>
{
    // This value will be injected into the URL.
    [DataMember]
    [ProtoMember(1)]
    public long AccountId { get;set; }

    // This value will be passed via QueryString (GET/DELETE) or in the request Body (POST/PUT).
    [DataMember]
    [ProtoMember(2)]
    public bool NoCache { get;set; }
}
```

**`GetAccountResponseStatus.cs`:**

```csharp
using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;
using ProtoBuf;
using XPike.Drivers.Http.Declarative;

[Serializable]
[DataContract]
[ProtoContract]
public enum GetAccountResponseStatus
{
    /// <summary>
    /// Should not be used.
    /// </summary>
    [EnumMember]
    [ProtoEnum]
    [StatusCode(HttpStatusCode.InternalServerError)]
    Unknown = 0,

    [EnumMember]
    [ProtoEnum]
    [StatusCode(HttpStatusCode.OK)]
    Successful = 1,

    [EnumMember]
    [ProtoEnum]
    [StatusCode(HttpStatusCode.NotFound)]
    NotFound = 2,

    [EnumMember]
    [ProtoEnum]
    [StatusCode(HttpStatusCode.InternalServerError)]
    Error = 4
}
```

**`GetAccountResponse.cs`:**

```csharp
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProtoBuf;
using XPike.Contracts;
using XPike.Drivers.Declarative;

[Serializable]
[DataContract]
[ProtoContract]
public class GetAccountResponse
    : IRespondTo<GetAccountQuery>
{
    [DataMember]
    [ProtoMember(1)]
    public Account Account { get; set; }

    [DataMember]
    [ProtoMember(2)]
    [JsonConverter(typeof(StringEnumConverter))]
    public GetAccountResponseStatus Status { get; set; }

    [DataMember]
    [ProtoMember(3)]
    public string ErrorMessage { get; set; }
}
```

#### Define Driver Settings

**`AccountDriverSettings.cs`:**

```csharp
public class AccountDriverSettings
{
    public string BaseUrl { get; set; }
    public string DefaultTimeout { get; set; }
}
```

#### Create a Driver

**`IAccountDriver.cs`:**

```csharp
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XPike.Drivers.Http.Declarative;

public interface IAccountDriver
    : IDriveHttp
{
    Task<GetAccountResponse> GetAccountAsync(GetAccountQuery query,
                                             TimeSpan? timeout = null,
                                             CancellationToken? ct = null,
                                             IDictionary<string, string> headers = null);
}
```

**`AccountDriver.cs`:**

```csharp
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XPike.Drivers.Http.Declarative;
using XPike.Settings;

public class AccountDriver
    : HttpDriverBase,
      IAccountDriver
{
    private readonly ISettings<HttpDriverSettings> _settings;
    private readonly ISettings<AccountDriverSettings> _driverSettings;

    public override HttpHostInfo HostInfo => new HttpHostInfo
                                             {
                                                 BaseUri = new Uri(_driverSettings.Value.BaseUri)
                                             };

    public override HttpDriverSettings Settings => 

    public AccountDriver(ISettings<HttpDriverSettings> settings, HttpClient client, ISettings<AccountDriverSettings> driverSettings)
        : base(client)
    {
        _settings = settings;
        _driverSettings = driverSettings;
    }

    public Task<GetAccountResponse> GetAccountAsync(GetAccountQuery query,
                                                    TimeSpan? timeout = null,
                                                    CancellationToken? ct = null,
                                                    IDictionary<string, string> headers = null) =>
        GetResponseAsync<GetAccountQuery, GetAccountResponse>(query,
                                                              timeout ?? TimeSpan.Parse(_driverOptions.Value.DefaultTimeout),
                                                              ct,
                                                              headers);
}
```

#### Create a Service Collection Extension

**`IServiceCollectionExtensions.cs`:**

```csharp
using XPike.Drivers.Http.Declarative.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAccountSDK(this IServiceCollection services)
    {
        services.AddDriver<IAccountDriver, AccountDriver>();

        return services;
    }
}
```

## Building and Testing

Building from source and running unit tests requires a Windows machine with:

* .Net Core 3.0 SDK
* .Net Framework 4.6.1 Developer Pack

## Issues

Issues are tracked on [GitHub](https://github.com/xpike/xpike-settings/issues). Anyone is welcome to file a bug,
an enhancement request, or ask a general question. We ask that bug reports include:

1. A detailed description of the problem
2. Steps to reproduce
3. Expected results
4. Actual results
5. Version of the package xPike
6. Version of the .Net runtime

## Contributing

See our [contributing guidelines](https://github.com/xpike/documentation/blob/master/docfx_project/articles/contributing.md)
in our documentation for information on how to contribute to xPike.

## License

xPike is licensed under the [MIT License](LICENSE).
