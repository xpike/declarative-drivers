using System;
using System.Linq;
using System.Net;

namespace XPike.Drivers.Http.Declarative
{
    /// <summary>
    /// Provides extension methods for working with Enums with Declarative HTTP Drivers.
    ///
    /// Exposes:
    /// - GetStatusCode()
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the value of the StatusCodeAttribute applied to an Enum Member (aka "name", "value", "field").
        /// Optionally allows a default value to be returned if no StatusCodeAttribute is specified for the Enum Member,
        /// and to indicate if an InvalidOperationException should be thrown when no value is found.
        /// </summary>
        /// <typeparam name="TEnum">The type of the Enum Member whose Status Code should be retrieved.</typeparam>
        /// <param name="value">The Enum Member to retrieve a Status Code from.</param>
        /// <param name="defaultValue">An optional HttpStatusCode to be returned if no value is found on the Enum Member.</param>
        /// <param name="throwIfMissing">Optionally indicates, when true, that an InvalidOperationException should be thrown if no value is found on the Enum Member.</param>
        /// <returns></returns>
        public static HttpStatusCode? GetStatusCode<TEnum>(this TEnum value, HttpStatusCode? defaultValue = null, bool throwIfMissing = false)
            where TEnum : Enum
        {
            HttpStatusCode? valueOrDefault = null;

            try
            {
                var enumType = typeof(TEnum);
                var memberInfos = enumType.GetMember(value.ToString());
                var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
                var valueAttributes = enumValueMemberInfo?.GetCustomAttributes(typeof(StatusCodeAttribute), false);
                valueOrDefault = (valueAttributes?.FirstOrDefault() as StatusCodeAttribute)?.StatusCode ?? defaultValue;
            }
            catch(Exception)
            {
                // Intentional no-op
            }

            if (valueOrDefault == null && throwIfMissing)
                throw new InvalidOperationException($"No StatusCode attribute found for Enum Member {typeof(TEnum)}.{value}!");

            return valueOrDefault;
        }
    }
}