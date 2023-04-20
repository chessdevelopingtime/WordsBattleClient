using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using app.scenes.settings.developerSettings;
using static App.Main;

namespace app.scenes.game{
    public class WordsCounter : MonoBehaviour {
	    private int wordsAmount;
	    public TextMeshProUGUI view;

	    public int getWordsAmount()
	    {
		    return wordsAmount;
	    }

	    public void increase()
	    {
		    wordsAmount++;
	    }

	    public void refreshView()
	    {
		    view.SetText(wordsAmount.ToString());
	    }
    }
}