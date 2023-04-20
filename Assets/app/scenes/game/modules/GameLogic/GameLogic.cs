using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using EventNameSpace;
using WSockets;
using static GameScene;
using MatchAnimatorSpace;

public partial class GameLogic {
    private readonly IHasGameLogic data;
    private readonly MatchResultAnimate animateMatch;

    public GameLogic(IHasGameLogic scene, MatchResultAnimate animateMatch) {
        data = scene;
        this.animateMatch = animateMatch;
    }

    public (List<GameObject>, List<TextMeshProUGUI>) onLoad() {
        data.Field = GameObject.FindGameObjectWithTag("Field");
        data.Tiles = new List<GameObject>();
        data.MatchedTileIds = new List<byte>();
        data.MapFlat = new List<char>();

        GameScene.isAnimating = false;
        GameScene.events = new EventQueue();

        data.Field.SetActive(false);
        return createField();
    }

    public (List<char>, List<TextMeshProUGUI>) DisplayField() {
        List<char> newMapFlat = new List<char>(data.MapFlat);
        List<TextMeshProUGUI> newLetters = new List<TextMeshProUGUI>(data.Letters);

        data.ThemeObj.GetComponent<TextMeshPro>().SetText(GameScene.theme);

        byte i = 0;
        GameScene.map.SelectMany(row => row).ToList()
            .ForEach((letter => {
                newLetters[i++].SetText(letter.ToString());
                newMapFlat.Add(letter);
            }));

        return (newMapFlat, newLetters);
    }

    public (List<byte>, List<GameObject>) HandleMatchFinished() {
        List<byte> newTileIds = new List<byte>(data.MatchedTileIds);
        List<char> newMapFlat = new List<char>(data.MapFlat);
        List<GameObject> newTiles = new List<GameObject>(data.Tiles);

        string[] tileIds = newTileIds.Select(id => id.ToString()).ToArray();
        string matchedWord = string.Join("-", tileIds);
        if (newTileIds.Count > 1) {
            WS.Send("game:match", matchedWord);
        } else {
            newTiles = animateMatch();
            (newTileIds, newTiles) = MatchClear();
        }

        return (newTileIds, newTiles);
    }

    public (List<byte>, List<GameObject>) MatchClear() {
        List<byte> newTileIds = new List<byte>(data.MatchedTileIds);
        List<GameObject> newTiles = new List<GameObject>(data.Tiles);

        newTileIds.ForEach((i) => {
            Tile tile = newTiles[i].GetComponent<Tile>();
            if (tile.isMatched) {
                newTiles[i].GetComponent<MeshRenderer>().sharedMaterial = data.Materials[(int)MatchAnimatorSpace.MatchAnimator.MatchAnimation.None];
                newTiles[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
                tile.isMatched = false;
            }
        });
        newTileIds.Clear();

        return (newTileIds, newTiles);
    }

    public (Tile, List<byte>) HandleMatch(
        Tile tile,
        RaycastHit hit,
        MatchAnimate matchAnimate
    ) {
        // ? copied tile not change isMatched
        // Tile newTile = tile.ShallowCopy();

        List<byte> newTiles = new List<byte>(data.MatchedTileIds);

        matchAnimate(hit);
        tile.isMatched = true;
        newTiles.Add(tile.id);
        return (tile, newTiles);
    }

    public List<GameObject> HandleEnemyMatch(MatchEnemyAnimate matchEnemyAnimate, List<byte> enemyMatchedTileIds) {
        List<byte> newTileIds = new List<byte>(data.MatchedTileIds);
        List<GameObject> newTiles = new List<GameObject>(data.Tiles);
        // ? check if current userMatch has same tiles with enemy match
        if (data.MatchedTileIds.Count != 0) {
            bool isCrossMatch = data.Tiles.Exists(tile => tile.GetComponent<Tile>().isMatched);
            // ? clean a user match if it crossed with an enemy match
            if (isCrossMatch) {
                Debug.Log("cross Match");
                (data.MatchedTileIds, data.Tiles) = MatchClear();
            }

        }

        matchEnemyAnimate(enemyMatchedTileIds);
        enemyMatchedTileIds.ForEach(id => {
            Tile tile = newTiles[id].GetComponent<Tile>();
            tile.isMatched = true;
        });
        return newTiles;
    }

    public bool isValidToMatch(Tile tile) {
        if (tile == null || tile.isMatched) return false;
        if (data.MatchedTileIds.Count == 0) return true;

        var prevTile = data.Tiles[data.MatchedTileIds[data.MatchedTileIds.Count - 1]].GetComponent<Tile>();

        if (tile.cell.row == prevTile.cell.row) {
            if (tile.cell.col == prevTile.cell.col + 1 || tile.cell.col == prevTile.cell.col - 1)
                return true;
        }
        if (tile.cell.col == prevTile.cell.col) {
            if (tile.cell.row == prevTile.cell.row + 1 || tile.cell.row == prevTile.cell.row - 1)
                return true;
        }
        return false;
    }

    public (List<GameObject>, List<TextMeshProUGUI>) createField() {

        //TODO refactor difficult to understand

        List<TextMeshProUGUI> newLetters = new List<TextMeshProUGUI>();
        List<GameObject> newTiles = new List<GameObject>();

        byte size = 8;
        sbyte space = 24;

        byte id = 0;
        sbyte y = 84;
        for (byte row = 0; row < size; row++, y -= space) {
            sbyte x = -84;
            for (byte col = 0; col < size; col++, x += space) {
                var newTile = GameObject.Instantiate(data.TilePrefab, new Vector3(x, y, -10f), Quaternion.identity);
                newTile.transform.SetParent(data.Field.transform);
                newTile.GetComponent<Tile>().cell = new Cell(row, col);
                newTile.GetComponent<Tile>().id = id++;
                newLetters.Add(newTile.GetComponentInChildren<TextMeshProUGUI>());
                newTiles.Add(newTile);
            }
        }

        return (newTiles, newLetters);
    }
}
