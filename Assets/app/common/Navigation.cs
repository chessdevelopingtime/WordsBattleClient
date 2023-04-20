using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helpers {
    public class Navigation : MonoBehaviour {
        public void goScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }
    }
}
