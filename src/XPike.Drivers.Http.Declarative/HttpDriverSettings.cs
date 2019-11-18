namespace XPike.Drivers.Http.Declarative
{
    /// <summary>
    /// Specifies the base options for Declarative HTTP Drivers.
    /// These will apply to all derived implementations of HttpDriverBase unless overridden in the constructor.
    /// Intended to be sourced via DI through XPike ISettings.
    /// </summary>
    public class HttpDriverSettings
    {
        /// <summary>
        /// The URL of the HTTP Proxy Server to be used.
        /// Leave blank if no Proxy should be used.
        /// </summary>
        public string ProxyUrl { get; set; }

        /// <summary>
        /// The default timeout to apply to outbound connections.
        /// This should be specified as a TimeSpan string, eg: "00:00:60" for 60 seconds.
        /// If not specified, a default timeout of 60 seconds will be applied.
        /// </summary>
        public string DefaultTimeout { get; set; }
    }
}