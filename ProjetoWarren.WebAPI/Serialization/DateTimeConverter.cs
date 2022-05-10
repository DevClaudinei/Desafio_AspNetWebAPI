using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjetoWarren.WebAPI.Serialization
{
    public class DateTimeConverter<DateTime> : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var tokenType = reader.TokenType;
            var dateTimeText = reader.GetString();

            if (tokenType != JsonTokenType.String)
            {
                throw new NotSupportedException($"Value {dateTimeText} not supported.");
            }
            
            if (DateTime.TryParse(dateTimeText, new CultureInfo("pt-BR"), DateTimeStyles.None, out DateTime date)) return date;
            throw new NotSupportedException($"Value {dateTimeText} not supported.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
