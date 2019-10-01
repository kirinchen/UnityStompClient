using System.Collections.Generic;
using UnityEngine;
namespace UnityStomp {
    public class OnMessager  {
        private List<OnMessage> onMessages = new List<OnMessage>();
        public string sid { get; private set; }

        internal OnMessager(string s) {
            sid = s;
        }

        public void onMsg(string s) {
            onMessages.ForEach(om=> om(s));
        }

        internal void addAction(OnMessage  om) {
            onMessages.Add(om);
        }

    }
}
