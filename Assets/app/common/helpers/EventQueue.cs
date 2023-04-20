using System.Collections.Generic;
using static WSockets.WS;

namespace EventNameSpace {
    public class EventQueue {
        private Dictionary<string, VoidFunc> eventMap = new Dictionary<string, VoidFunc>();
        private List<string> events = new List<string>();

        public void Listen(string eventName, VoidFunc handler) => eventMap.Add(eventName, handler);
        public void Add(string eventName) => events.Add(eventName);
        public void Remove(string eventName) => events.Remove(eventName);

        public void Run() {
            var eventsBox = new List<string>(events);
            eventsBox.ForEach(e => eventMap[e]());
        }

        public void Clear() {
            eventMap.Clear();
            events.Clear();
        }
    }
}