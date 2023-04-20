using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static MatchAnimatorSpace.MatchAnimator;

public class MatchAnimator {
    private readonly IHasMatchAnimator data;

    public MatchAnimator(IHasMatchAnimator scene) {
        data = scene;
    }

    public List<GameObject> MatchResultAnimate() {
        List<GameObject> newTiles = new List<GameObject>(data.Tiles);

        data.MatchedTileIds.ForEach((id) => {
            newTiles[id].GetComponent<MeshRenderer>().sharedMaterial = data.Materials[(int)GameScene.matchAnimation];
            newTiles[id].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = GameScene.matchAnimation == MatchAnimation.None ? Color.black : Color.white;
        });

        return newTiles;
    }

    public void MatchAnimate(RaycastHit hit) {
        Renderer renderer = hit.collider.GetComponent<Renderer>();
        renderer.sharedMaterial = data.State.playerAnimation.onSuccess;
    }

    // ! dirty mutable func
    public void MatchEnemyAnimate(List<byte> enemyTileIds) {
        enemyTileIds.ForEach((id) => {
            data.Tiles[id].GetComponent<MeshRenderer>().sharedMaterial = data.State.enemyAnimation.onSuccess;
            data.Tiles[id].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        });
    }

    public IEnumerator matchFailedAnimation(GameScene scene, GameLogic logic) {
        GameScene.isAnimating = true;
        yield return new WaitForSeconds(1);
        scene.Tiles = MatchResultAnimate();
        (scene.MatchedTileIds, scene.Tiles) = logic.MatchClear();
        GameScene.isAnimating = false;
    }
}
