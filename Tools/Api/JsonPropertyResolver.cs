using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text.Json.Serialization;

namespace FrenchyApps42.Tools.Api
{
    public class JsonPropertyResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = base.CreateProperties(type, memberSerialization);

            foreach (var prop in props)
            {
                PropertyInfo? propInfo = type
                    .GetProperty(prop.UnderlyingName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (propInfo != null)
                {
                    JsonPropertyNameAttribute? jsonPropertyName = propInfo.GetCustomAttribute<JsonPropertyNameAttribute>();

                    if (jsonPropertyName != null)
                    {
                        prop.PropertyName = jsonPropertyName.Name;
                    }
                }
            }

            return props;
        }
    }
}
