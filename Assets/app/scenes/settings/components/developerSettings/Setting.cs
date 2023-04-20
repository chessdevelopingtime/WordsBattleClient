using System.Collections;
using UnityEngine;

namespace app.scenes.settings.developerSettings {
    public class Setting : MonoBehaviour {
        public string Name { get; }
        public string DefValue { get; }

        public Setting(string n, string v) {
            Name = n;
            DefValue = v;
        }
    }
}