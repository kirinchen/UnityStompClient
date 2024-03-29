﻿using UnityEngine;
using System.Collections;
using System;
using UnityStomp;
using BestHTTP.WebSocket;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using surfm.tool;

namespace UnityStomp {
    public class StompClientDebug : StompClient {
        private static readonly string JOINED_MSG = "{\"roomId\":\"QRU6F\",\"gameUid\":\"TankIO2\",\"currentCount\":1,\"maxPlayerCount\":35,\"data\":null,\"information\":{\"gameId\":\"TankIO2\",\"playList\":[\"2aBdTRed\"],\"initCount\":3},\"meId\":\"2aBdTRed\",\"stamp\":\"99999999999\",\"index\":0,\"playedTime\":124024,\"success\":true,\"exceptionName\":null}";
        private static readonly string INTO_ROOM_MSG = "{\"senderId\":\"2aBdTRed\",\"tellerIds\":[],\"sessionId\":\"2aBdTRed\",\"type\":\"NewPlayerJoined\"}";
        private string sessionId;

        public StompClientDebug(string connectString) {
            sessionId = "2aBdTRed";
        }

        public void CloseWebSocket() {
        }

        public string getSessionId() {
            return sessionId;
        }

        public void SendMessage(string destination, string message) {
        }

        public void SendMessage(string destination, string message, string subscribeDestination, OnMessage act) {
        }

        public void SetCookie(string name, string value) {
        }

        public void setOnErrorAndClose(Action<string> errorCb, Action<string> cCb) {
        }

        public InitCallBack StompConnect() {
            return null;
        }

        public void Subscribe(string destination, OnMessage act) {
            if (match(destination, "/app/.+/joinBattle/.+")) {
                act(JOINED_MSG);
            } else if (match(destination, "/message/rooms/.+/broadcast")) {
                act(INTO_ROOM_MSG);
            }
        }

        public void unSubscribe(string destination) {
        }

        private bool match(string path, string reqx) {
            Regex rgx = new Regex(reqx, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(path);
            return matches.Count > 0;
        }

    }
}
