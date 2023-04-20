using TMPro;
using System.Collections.Generic;
using UnityEngine;

namespace App {
    public class Auth {
        public string UserId { get; set; } = null;

        public Auth(string userId) {
            UserId = userId;
        }
    }
}
