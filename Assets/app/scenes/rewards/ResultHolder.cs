using UnityEngine;
using TMPro;
using static App.Main;

namespace app.scenes.rewards{
    public class ResultHolder : MonoBehaviour {
	    public TextMeshProUGUI playerResultView;
	    public TextMeshProUGUI enemyResultView;

	    public void Start()
	    {
		    playerResultView.SetText(state.PlayerMatchedWordsAmount.ToString());
		    enemyResultView.SetText(state.EnemyMatchedWordsAmount.ToString());
	    }
    }
}