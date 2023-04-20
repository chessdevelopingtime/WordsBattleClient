using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using App;
using TMPro;
using EventNameSpace;
using static WSockets.WS;
using static MatchAnimatorSpace.MatchAnimator;
using app.scenes.game;

public partial class GameScene : MonoBehaviour, GameLogic.IHasGameLogic, IHasMatchAnimator {
    // ? Scene object references
    public GameObject tilePrefab;
    public GameObject preloader;
    public GameObject endGameAnimation;
    public GameObject field;
    public GameObject themeObj;
    public Timer timer;
    public WordsCounterManager wordsCounterManager;
    public Material[] materials;

    // ? State 
    public GameObject TilePrefab { get; set; }
    public GameObject ThemeObj { get; set; }
    public GameObject Field { get; set; }
    public Material[] Materials { get; set; }
    public State State { get; set; }

    public List<GameObject> Tiles { get; set; } = new List<GameObject>();
    public List<TextMeshProUGUI> Letters { get; set; }

    public List<byte> MatchedTileIds { get; set; } = new List<byte>();
    public static char[][] map = null;
    public static string theme = null;
    public static string enemyMatch = null;
    public static string enemyAnimName = null;
    public List<char> MapFlat { get; set; } = new List<char>();

    public static EventQueue events = new EventQueue();

    public static MatchAnimation matchAnimation = MatchAnimation.None;
    public static bool isAnimating = false;

    // ? Modules
    private GameLoader loader;
    private GameLogic logic;
    private MatchAnimator animator;

    void Start() {
        TilePrefab = tilePrefab;
        ThemeObj = themeObj;
        Field = field;
        Materials = materials;
        
        wordsCounterManager.initialize();
        State = Main.state;
        animator = new MatchAnimator(this);
        logic = new GameLogic(this, animator.MatchResultAnimate);

        loader = new GameLoader(State, wsHandlers, () => (Tiles, Letters) = logic.onLoad());
        loader.LoadScene();
        events.Listen("game:map", onMap);
        events.Listen("user:touch", onTouch);
        events.Listen("user:match", onMatch);
        events.Listen("enemy:match", onEnemyMatch);
        events.Listen("game:end", onGameEnd);
        events.Listen("lobby:wait", onLobbyWait);

        events.Add("user:touch");
    }

    void Update() => events.Run();

    private Dictionary<string, MessageHandler> wsHandlers = new Dictionary<string, MessageHandler>(){
        {"game:start",(msg) => {
            var startMsg =  msg.ToObject<StartMsg>();
            map = startMsg.rows;
            theme = startMsg.theme;

            events.Add("game:map");
            
        }},
        {"game:match:success",(msg) =>{
             matchAnimation = MatchAnimation.Success;
             events.Add("user:match");
        }},
        {"game:match:enemy",(msg) => {

            var enemyMatchMsg = msg.ToObject<EnemyMatchMsg>();
            enemyAnimName = enemyMatchMsg.animation;
            enemyMatch = enemyMatchMsg.match;
            Debug.Log($"game:match:enemy: {enemyAnimName}");
            events.Add("enemy:match");
        }},
        {"game:match:failed",(msg) => {
            matchAnimation = MatchAnimation.Failed;
             events.Add("user:match");
        }},
        {"game:end",(msg) => events.Add("game:end")},
        {"lobby:wait", (msg) => events.Add("lobby:wait")}
    };

    private void onTouch() {
        if (Input.touchCount > 0 && !isAnimating) {
            if (Input.touches[0].phase == TouchPhase.Ended) {
                (MatchedTileIds, Tiles) = logic.HandleMatchFinished();
                return;
            }

            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            Physics.Raycast(ray, out hit);
            Tile touchedTile = hit.collider.GetComponent<Tile>();

            if (logic.isValidToMatch(touchedTile)) {
                (touchedTile, MatchedTileIds) = logic.HandleMatch(touchedTile, hit, animator.MatchAnimate);
            }
        }
    }

    private void onMatch() {
        animator.MatchResultAnimate();

        if (matchAnimation == MatchAnimation.Failed) {
            StartCoroutine(animator.matchFailedAnimation(this, logic));
        } else {
            wordsCounterManager.playerIncrease();
            MatchedTileIds.Clear();
        }
        
        matchAnimation = MatchAnimation.None;
        events.Remove("user:match");
    }

    private void onEnemyMatch() {
        List<byte> enemyMatchedIds = enemyMatch.Split('-').Select(byte.Parse).ToList();
        Debug.Log("onEnemyMatch");
        logic.HandleEnemyMatch(animator.MatchEnemyAnimate, enemyMatchedIds);
        
        wordsCounterManager.enemyIncrease();
        events.Remove("enemy:match");
    }

    private void onMap() {
        (MapFlat, Letters) = logic.DisplayField();
        preloader.SetActive(false);
        Field.SetActive(true);
        timer.initialize();

        events.Remove("game:map");
    }

    private void onLobbyWait() {
        preloader.SetActive(true);
        Debug.Log("get lobby:wait");
        events.Remove("lobby:wait");
    }

    private void onGameEnd() {
        State.PlayerMatchedWordsAmount = wordsCounterManager.playerCounter.getWordsAmount();
        State.EnemyMatchedWordsAmount = wordsCounterManager.enemyCounter.getWordsAmount();
        endGameAnimation.SetActive(true);
        loader.ClearScene();
        events.Remove("game:end");
        
        Invoke("loadRewardsScene", 5);
    }

    private void loadRewardsScene()
    {
        SceneManager.LoadScene("Rewards");
    }
}