using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helpers {
    public class AnalyticEventSender : MonoBehaviour {
        public void sendFirstPlayButonEvent() {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "sendFirstPlayButonEvent");
        }
        
        public void sendSecondPlayButonEvent() {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "sendSecondPlayButonEvent");
        }
    }
}
