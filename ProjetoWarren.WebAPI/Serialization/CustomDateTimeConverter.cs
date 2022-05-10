using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjetoWarren.WebAPI.Serialization
{
    public class CustomDateTimeConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                var converter = ConverterFactory(typeof(DateTimeConverter), typeToConvert);

                return converter;
            }
            catch (TargetInvocationException targetInvocationEx)
            {
                if (targetInvocationEx.InnerException != null) throw targetInvocationEx.InnerException;

                throw;
            }
        }

        private static JsonConverter ConverterFactory(Type converter, Type typeToConvert)
        {
            var jsonConverter = (JsonConverter)Activator.CreateInstance(
                converter.MakeGenericType(typeToConvert),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: Array.Empty<object>(),
                culture: null
            )!;

            return jsonConverter;
        }
    }
}
