using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace WSockets {
    public partial class WS {
        private class Message<T> {
            public string type;
            public T data;

            public Message(string type, T data) {
                this.type = type;
                this.data = data;
            }
        }

        public delegate void MessageHandler(JObject msg);
        public delegate void VoidFunc();


        private class SocketMap : Dictionary<string, List<MessageHandler>> {
            public new List<MessageHandler> this[string type] {
                get { return base[type]; }
                set { base[type] = value; }
            }
        }
    }
}