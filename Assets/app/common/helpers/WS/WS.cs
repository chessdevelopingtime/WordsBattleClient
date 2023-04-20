using System.Linq;
using WebSocketSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using UnityEngine;
namespace WSockets {

    public partial class WS {
        static private WebSocket ws = null;
        static private Dictionary<string, List<MessageHandler>> socketMap = new Dictionary<string, List<MessageHandler>>();

        public static WebSocketState readyState;

        public static void Connect(string wsServer, VoidFunc cb = null) {
            ws = new WebSocket(wsServer);
            readyState = ws.ReadyState;
            ws.OnOpen += (sender, e) => cb();
            ws.OnMessage += (sender, e) => {
                Message<JObject> msg = JsonConvert.DeserializeObject<Message<JObject>>(e.Data);
                socketMap[msg.type].ForEach((msgHandler) => msgHandler(msg.data));
                Debug.Log($"get msg @type {msg.type}");
            };
            ws.Connect();
        }

        public static void Send(string type, object data = null) {
            ws.Send(JsonConvert.SerializeObject(new Message<object>(type, data)));
        }

        public static void On(string type, MessageHandler msgHandler) {
            if (!socketMap.ContainsKey(type)) socketMap.Add(type, new List<MessageHandler>());
            socketMap[type].Add(msgHandler);
        }

        public static void Clear(List<string> types) {
            types.ForEach((type) => socketMap[type].Clear());
        }

        public static void Close() => ws.Close();
    }
}