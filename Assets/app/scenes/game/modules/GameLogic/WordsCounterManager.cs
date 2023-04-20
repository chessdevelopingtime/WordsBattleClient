using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using app.scenes.settings.developerSettings;

namespace app.scenes.game{
    public class WordsCounterManager : MonoBehaviour {
	    public GameObject playerCounterObj;
	    public GameObject enemyCounterObj;
	    public WordsCounter playerCounter { get; set; }
	    public WordsCounter enemyCounter { get; set; }

	    public void initialize()
	    {
		    playerCounterObj.SetActive(true);
		    enemyCounterObj.SetActive(true);
		    playerCounter = playerCounterObj.GetComponent<WordsCounter>();
		    enemyCounter = enemyCounterObj.GetComponent<WordsCounter>();
	    }
	    
	    public void playerIncrease()
	    {
		    playerCounter.increase();
		    playerCounter.refreshView();
        }
	    
	    public void enemyIncrease()
	    {
		    enemyCounter.increase();
		    enemyCounter.refreshView();
	    }
    }
}