using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameScene {
    class StartMsg {
        public string theme;
        public char[][] rows;
    }

    class EnemyMatchMsg {
        public string animation;
        public string match;
    }

    public class Cell {
        public byte row;
        public byte col;

        public Cell(byte row, byte col) {
            this.row = row;
            this.col = col;
        }
    }
}

