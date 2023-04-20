namespace App {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Helpers;

    public class State : MonoBehaviour {
        // * State
        public Auth auth;
        public int PlayerMatchedWordsAmount { get; set; }
        public int EnemyMatchedWordsAmount { get; set; }
        
        public Animation playerAnimation { get; set; }
        public Animation enemyAnimation { get; set; }
        public State(Animation playerAnimation, Animation enemyAnimation, string userId) {
            this.playerAnimation = playerAnimation;
            this.enemyAnimation = enemyAnimation;

            auth = new Auth(userId);
        }
    }
}
