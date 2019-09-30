using UnityEngine;
using System.Collections;
using System;
using surfm.tool;

namespace UnityStomp {
    public delegate void OnMessageListener(string msg);

    public interface StompClient {
        void SetCookie(string name, string value);

        CallbackList StompConnect();

        void setOnErrorAndClose(Action<string> errorCb, Action<string> closedCb);

        void Subscribe(string destination, OnMessageListener act);

        void unSubscribe(string destination);

        void SendMessage(string destination, string message);

        void SendMessage(string destination, string message, string subscribeDestination, OnMessageListener act);

        void CloseWebSocket();

        string getSessionId();

    }
}
