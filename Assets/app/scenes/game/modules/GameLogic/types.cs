using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class GameLogic {
    public delegate void MatchAnimate(RaycastHit hit);
    public delegate void MatchEnemyAnimate(List<byte> enemyTileIds);

    public delegate List<GameObject> MatchResultAnimate();

    public interface IHasGameLogic {
        List<char> MapFlat { get; set; }
        List<TextMeshProUGUI> Letters { get; set; }
        List<byte> MatchedTileIds { get; set; }
        List<GameObject> Tiles { get; set; }
        Material[] Materials { get; set; }

        GameObject TilePrefab { get; set; }
        GameObject ThemeObj { get; set; }
        GameObject Field { get; set; }
    }
}