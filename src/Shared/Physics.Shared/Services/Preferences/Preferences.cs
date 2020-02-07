using Newtonsoft.Json;
using System;
using Windows.Storage;

namespace Physics.Shared.Services.Preferences
{
    public class Preferences : IPreferences
    {
        public T GetSetting<T>(string key, T defaultValue = default, PreferenceLocality preferenceLocality = PreferenceLocality.Local)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var container = GetContainer(preferenceLocality);
            if (!TryReadFromContainer(key, container, out T result))
            {
                result = defaultValue;
            }
            return result;
        }

        public T GetSetting<T>(string key, Func<T> defaultValueBuilder, PreferenceLocality preferenceLocality = PreferenceLocality.Local)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (defaultValueBuilder == null) throw new ArgumentNullException(nameof(defaultValueBuilder));

            var container = GetContainer(preferenceLocality);
            if (!TryReadFromContainer(key, container, out T result))
            {
                result = defaultValueBuilder();
            }
            return result;
        }

        private static bool TryReadFromContainer<T>(string key, ApplicationDataContainer container, out T deserializeObject)
        {
            if (container.Values.TryGetValue(key, out var value) && value != null)
            {
                deserializeObject = JsonConvert.DeserializeObject<T>(value.ToString());
                return true;
            }
            deserializeObject = default;
            return false;
        }

        public void SetSetting<T>(string key, T value, PreferenceLocality preferenceLocality = PreferenceLocality.Local)
        {
            var container = GetContainer(preferenceLocality);
            var serialized = JsonConvert.SerializeObject(value);
            container.Values[key] = serialized;
        }

        private ApplicationDataContainer GetContainer(PreferenceLocality locality) =>
            locality switch
            {
                PreferenceLocality.Local => ApplicationData.Current.LocalSettings,
                PreferenceLocality.Roaming => ApplicationData.Current.RoamingSettings,
                _ => throw new ArgumentOutOfRangeException(nameof(locality), locality, null)
            };
    }
}
