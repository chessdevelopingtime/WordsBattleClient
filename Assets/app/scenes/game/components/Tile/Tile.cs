using UnityEngine;

using static GameScene;

public class Tile : MonoBehaviour {
    public Cell cell;
    public byte id;

    public bool isMatched = false;

    public Tile ShallowCopy() {
        return (Tile)this.MemberwiseClone();
    }
}
