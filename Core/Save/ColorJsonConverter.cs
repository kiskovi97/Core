using System;

using Newtonsoft.Json;

using UnityEngine;

namespace Kiskovi.Core
{
    internal class ColorJsonConverter : JsonConverter<Color>
    {
        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var colorValues = serializer.Deserialize<float[]>(reader);
            return new Color(colorValues[0], colorValues[1], colorValues[2], colorValues[3]);
        }

        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, new[] { value.r, value.g, value.b, value.a });
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
    }
}
