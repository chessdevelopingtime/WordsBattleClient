using app.scenes.settings.developerSettings;
using UnityEngine;

namespace app.scenes.settings
{
    public class SettingManager : MonoBehaviour
    {
        public string getSetting(Setting settings)
        {
            if (!PlayerPrefs.HasKey(settings.Name))
            {
                setSetting(settings, settings.DefValue);
                return settings.DefValue;
            }

            return PlayerPrefs.GetString(settings.Name);
        }

        public void setSetting(Setting key, string value)
        {
            PlayerPrefs.SetString(key.Name, value);
            PlayerPrefs.Save();
        }
    }
}