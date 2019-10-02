using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityStomp;
namespace UnityStomp {
    public class Test : MonoBehaviour {

        private StompClient stompClient;
        public InputField showMsgText, sendMsgText;



        void Awake() {
            Application.runInBackground = true;
        }

        public void connect() {
            stompClient = new StompClientAll("ws://127.0.0.1:8080/gs-guide-websocket");
            stompClient.StompConnect().add(() => {
                Debug.Log("Connected!!!");
                stompClient.Subscribe("/topic/greetings", onMsg);
            });
        }

        private void onMsg(string s) {
            showMsgText.text += s + "\n\r";
        }

        public class SendData {
            public string name;
        }

        public void send() {
            SendData sd = new SendData() { name = sendMsgText.text };
            string jStr = JsonConvert.SerializeObject(sd);
            stompClient.SendMessage("/app/hello", jStr);
        }

    }
}
