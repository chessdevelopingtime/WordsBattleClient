using System.Collections.Generic;
using App;
using TMPro;
using UnityEngine;

namespace MatchAnimatorSpace {
    public partial class MatchAnimator {
        public enum MatchAnimation {
            None,
            Hover,
            Success,
            Failed,
        }

        public interface IHasMatchAnimator {
            State State{ get; set; }
            List<byte> MatchedTileIds { get; set; }
            List<GameObject> Tiles { get; set; }
            Material[] Materials { get; set; }
        }
    }
}
