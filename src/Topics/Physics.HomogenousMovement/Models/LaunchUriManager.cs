using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousMovement.Models
{
    static class LaunchUriManager
    {
        public static Uri Serialize<T>(string schema, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            var query = Uri.EscapeDataString(json);
            return new Uri($"{schema}:?{query}");
        }

        public static T Deserialize<T>(Uri uri)
        {
            var dataString = uri.Query.Substring(1);
            var decoded = Uri.UnescapeDataString(dataString);
            return JsonConvert.DeserializeObject<T>(decoded);
        }
    }
}
