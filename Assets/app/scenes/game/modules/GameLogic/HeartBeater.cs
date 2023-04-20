using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using app.scenes.settings.developerSettings;
using WSockets;
using static App.Main;

namespace app.scenes.game{
    public class HeartBeater : MonoBehaviour
    {
	    public int heartBeatDelaySec;

		public void Start() {
			InvokeRepeating("heartBeat", 0, heartBeatDelaySec);
        }

        void heartBeat()
        {
	        WS.Send("game:heartBeat");
        }
    }
}