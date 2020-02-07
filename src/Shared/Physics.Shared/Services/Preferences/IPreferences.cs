using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Services.Preferences
{
    public interface IPreferences
    {
        T GetSetting<T>(string key, T defaultValue = default, PreferenceLocality preferenceLocality = PreferenceLocality.Local);

        T GetSetting<T>(string key, Func<T> defaultValueBuilder, PreferenceLocality preferenceLocality = PreferenceLocality.Local);

        void SetSetting<T>(string key, T value, PreferenceLocality preferenceLocality = PreferenceLocality.Local);
    }
}
