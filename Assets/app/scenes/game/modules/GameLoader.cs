using System.Linq;
using System.Collections;
using System.Collections.Generic;
using App;
using WSockets;
using UnityEngine;

public class GameLoader {
    private Dictionary<string, WS.MessageHandler> wsHandlers;
    private WS.VoidFunc onLoad;
    private State state;

    public GameLoader(State state, Dictionary<string, WS.MessageHandler> wsHandlers, WS.VoidFunc onLoad) {
        this.wsHandlers = wsHandlers;
        this.onLoad = onLoad;
        this.state = state;
    }

    public void LoadScene() {
        WS.Connect("ws://words-battle.herokuapp.com", () => {
            Debug.Log(" I AM OPEN");
            onLoad();
            RunWSComunication();
        });
         /*WS.Connect("ws://localhost:3000", () => {
             Debug.Log(" I AM OPEN");
             onLoad();
             RunWSComunication();
         });*/
        Debug.Log($"readyState: {WS.readyState}");

    }

    public void ClearScene() {
        WS.Clear(wsHandlers.Select((k) => k.Key).ToList());
        WS.Close();
    }

    void RunWSComunication() {
        Debug.Log(WS.readyState);
        foreach (var handler in wsHandlers) {
            Debug.Log($"ws on {handler.Key}");
            WS.On(handler.Key, handler.Value);
        }
        Debug.Log("connecting... to server");
        object data = new {
            userId = state.auth.UserId,
            animation = state.playerAnimation.name
        };
        WS.Send("lobby", data);
    }
}