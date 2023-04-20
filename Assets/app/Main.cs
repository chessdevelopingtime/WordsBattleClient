using GameAnalyticsSDK;

namespace App {
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using app.scenes.settings;
    using app.scenes.settings.developerSettings;

    public class Main : MonoBehaviour {
        public SettingManager settingManager;
        public Animation playerAnimation;
        public Animation enemyAnimation;
        
        public static State state { get; set; }
        
        private void Start() {
            GameAnalytics.Initialize();

            Setting st = new Setting("Animation", "Blue");
            Setting userId = new Setting("userId", "123");
            string userAnimationName = settingManager.getSetting(st);
            Debug.Log($"userAnimationName: {userAnimationName}");

            state = new State(playerAnimation, enemyAnimation, settingManager.getSetting(userId));
            
            Debug.Log(state.auth.UserId);
        }
    }
}
