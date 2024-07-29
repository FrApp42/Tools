using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text.Json.Serialization;

namespace FrApp42.Web.API.Helpers
{
    internal sealed class JsonPropAttrResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);

            foreach (JsonProperty prop in props)
            {
                if (prop.UnderlyingName == null)
                    continue;

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
