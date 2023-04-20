using app.scenes.settings.developerSettings;
using TMPro;
using UnityEngine;

namespace app.scenes.settings {
    public class DuelPlayerAmountButton : MonoBehaviour {
        public SettingManager settingManager;
        public Setting setting;
        public string buttonTag;
        private TextMeshProUGUI playerAmountView;

        public void changePlayerAmount() {
            string value = settingManager.getSetting(setting).Equals("1") ? "2" : "1";
            settingManager.setSetting(setting, value);
            updateButton(value);
        }

        public void initializeButton() {
            playerAmountView = GameObject.FindGameObjectWithTag(buttonTag)
                .GetComponent<TextMeshProUGUI>();
            Debug.Log(playerAmountView);
            updateButton(settingManager.getSetting(setting));
        }

        private void updateButton(string value) {
            playerAmountView.SetText(value);
        }
    }
}
