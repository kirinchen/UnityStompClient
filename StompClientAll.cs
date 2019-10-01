using BestHTTP.WebSocket;
using surfm.tool;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityStomp {
    public class StompClientAll : StompClient {

        private string sessionId;
        public WebSocket websocket;
        public static string acceptVersion = "1.1,1.0";
        public static string heartBeat = "10000,10000";
        private Action<string> onErrorCb = (s) => { };
        private Action<string> onCloseCb = (s) => { };
        private CallbackList connectedDone = new CallbackList();
        private Dictionary<string, OnMessager> subscribeIdMap = new Dictionary<string, OnMessager>();
        public int subNo;

        public StompClientAll(string connectString, string sid = null) {
            sessionId = string.IsNullOrEmpty(sid) ? UidUtils.getRandomString(8) : sid;
            connectString += "/" + UidUtils.getRandomNumber(3) + "/" + sessionId + "/websocket";
            websocket = new WebSocket(new Uri(connectString));
            subNo = 0;
        }

        public void SetCookie(string name, string value) {
            //websocket.SetCookie(new WebSocketSharp.Net.Cookie(name, value));
        }

        //Stomp Connect...
        public CallbackList StompConnect() {
            websocket.OnOpen += onOpened;
            websocket.OnMessage += (sender, e) => {
                WsResponse resp = new WsResponse(e);
                if (resp.parse()) {
                    string key = getKeyByDestination(resp.getDestination());
                    if (key != null) {
                         subscribeIdMap[key].onMsg(resp.getMessage());
                    }
                }
            };
            websocket.OnError += (sender, e) => {
                //Debug.Log("Error message : " + e.Message);
                onErrorCb("");
            };

            websocket.OnClosed += (a, b, c) => {
                onCloseCb(c);
            };
            websocket.Open();
            return connectedDone;
        }

        private string getKeyByDestination(string d) {
            foreach (string key in subscribeIdMap.Keys) {
                Regex regex = new Regex(key, RegexOptions.IgnoreCase);
                Match m = regex.Match(d);
                if (m.Success) {
                    return key;
                }
            }
            return null;
        }

        private void onOpened(object sender) {
            var connectString = StompString("CONNECT", new Dictionary<string, string>()
                                            {
                {"accept-version", acceptVersion},
                {"heart-beat", heartBeat}
            });
            websocket.Send(connectString);
            Debug.Log("WebSocket connect...... " + sender);
            connectedDone.done();
        }


        private void vaildOpen() {
            if (!connectedDone.isDone()) throw new Exception("It`s not connected!!!");
        }

        //Subscribe...
        private OnMessager Subscribe(string destination) {
            vaildOpen();
            if (!subscribeIdMap.ContainsKey(destination)) {
                string sid = "sub-" + subNo;
                subscribeIdMap.Add(destination, new OnMessager(sid));
                var subscribeString = StompString("SUBSCRIBE", new Dictionary<string, string>()                     {
                {"id", sid},
                {"destination", destination}
                });
                websocket.Send(subscribeString);
                subNo++;
            }
            return subscribeIdMap[destination];
        }

        public void unSubscribe(string destination) {
            string sid = subscribeIdMap[destination].sid;
            subscribeIdMap.Remove(destination);
            var subscribeString = StompString("UNSUBSCRIBE", new Dictionary<string, string>()                     {
                {"id", sid}
            });
            websocket.Send(subscribeString);
        }

        public void Subscribe(string destination, OnMessage act) {
            OnMessager onMessager = this.Subscribe(destination);
            onMessager.addAction(act);
        }



        //Send Message
        public void SendMessage(string destination, string message) {
            try {
                string jsonMessage = message;
                string contentLength = jsonMessage.Length.ToString();
                jsonMessage = jsonMessage.Replace("\"", "\\\"");

                var sendString = "[\"SEND\\n" +
                    "destination:" + destination + "\\n" +
                        "content-length:" + contentLength + "\\n\\n" +
                        jsonMessage + "\\u0000\"]";

                websocket.Send(sendString);
            } catch (Exception e) {
                Debug.LogWarning(e);
            }
        }

        public void SendMessage(string destination, string message, string subscribeDestination, OnMessage act) {
            SendMessage(destination, message);
            Subscribe(subscribeDestination, act);
        }

        //Close 
        public void CloseWebSocket() {
            try {
                if (websocket.IsOpen) {
                    websocket.Close();
                }
            } catch (Exception e) {
                Debug.Log(e);
            }
        }

        public static string StompString(string method, Dictionary<string, string> content) {
            string result;
            result = "[\"" + method + "\\n";
            foreach (var item in content) {
                result = result + item.Key + ":" + item.Value + "\\n";
            }
            result = result + "\\n\\u0000\"]";
            return result;
        }

        public string getSessionId() {
            return sessionId;
        }

        public void setOnErrorAndClose(Action<string> errorCb, Action<string> cCb) {
            onErrorCb = errorCb;
            onCloseCb = cCb;
        }
    }
}