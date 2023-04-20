using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using app.scenes.settings.developerSettings;
using static App.Main;
using Animation = App.Animation;

namespace app.scenes.settings {
    public class SettingsScene : MonoBehaviour {
        public DuelPlayerAmountButton duelPlayerAmountButton;
        public GameObject animationNameObj;
        public SettingManager settingManager;

        private TextMeshProUGUI animName { get; set; }

        public string[] animationOrder = new string[] { "Blue", "Orange" };
        public int curAnimInd;

        void Start() {
            duelPlayerAmountButton.initializeButton();
            animName = animationNameObj.GetComponent<TextMeshProUGUI>();
            Setting st = new Setting("Animation", "Blue");

            string colorName = settingManager.getSetting(st);
            List<string> listAnimationOrder = new List<string>(animationOrder);

            int animInx = listAnimationOrder.FindIndex(color => color == colorName);
            curAnimInd = animInx;

            animName.SetText(state.playerAnimation.name);
        }

        public void ChangeAnimation()
        {
            Animation temp = state.playerAnimation;
            state.playerAnimation = state.enemyAnimation;
            state.enemyAnimation = temp;
        }

    }

}