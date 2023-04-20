using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using app.scenes.settings.developerSettings;
using static App.Main;

namespace app.scenes.game{
    public class Timer : MonoBehaviour {
	    public TextMeshProUGUI view;
	    public int durationMin;
		private readonly int refreshTime = 1;
		private float gameStartTime;

		public void initialize() {
	        gameStartTime = Time.realtimeSinceStartup;
	        InvokeRepeating("refresh", 0, refreshTime);
        }

        void refresh()
        {
	        TimeSpan delta = TimeSpan.FromMinutes(durationMin)
		        .Subtract(TimeSpan.FromSeconds(Time.realtimeSinceStartup - gameStartTime));
	        
	        view.SetText(String.Format("{0}:{1:00}", delta.Minutes, delta.Seconds));
        }
    }
}