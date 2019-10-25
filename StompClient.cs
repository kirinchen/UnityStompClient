using UnityEngine;
using System.Collections;
using System;
using surfm.tool;

namespace UnityStomp {
    public delegate void OnMessage(string msg);

    public interface StompClient {
        void SetCookie(string name, string value);

        InitCallBack StompConnect();

        void setOnErrorAndClose(Action<string> errorCb, Action<string> closedCb);

        void Subscribe(string destination, OnMessage act);

        void unSubscribe(string destination);

        void SendMessage(string destination, string message);

        void SendMessage(string destination, string message, string subscribeDestination, OnMessage act);

        void CloseWebSocket();

        string getSessionId();

    }
}
