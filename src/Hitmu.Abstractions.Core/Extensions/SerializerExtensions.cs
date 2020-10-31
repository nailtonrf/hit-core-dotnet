using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;

namespace Hitmu.Abstractions.Core.Extensions
{
    public static class SerializerExtensions
    {
        private static readonly JsonSerializerSettings JsonSerializerOptions;

        static SerializerExtensions()
        {
            JsonSerializerOptions = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public static string Serialize(this object source)
        {
            return JsonConvert.SerializeObject(source, DefaultSerializerOptions());
        }

        public static T ParseToFromJson<T>(this byte[] source)
        {
            var sourceMessage = Encoding.UTF8.GetString(source);
            return JsonConvert.DeserializeObject<T>(sourceMessage);
        }

        public static T ParseToFromJson<T>(this string source)
        {
            return JsonConvert.DeserializeObject<T>(source);
        }

        public static object? ParseToTypeFromJson(this string source, Type targetType)
        {
            return JsonConvert.DeserializeObject(source, targetType, DefaultSerializerOptions());
        }

        public static JsonSerializerSettings DefaultSerializerOptions()
        {
            return JsonSerializerOptions;
        }
    }
}