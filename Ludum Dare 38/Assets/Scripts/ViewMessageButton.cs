using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vnc
{
    public class ViewMessageButton : MonoBehaviour {

        public ViewMessageInteractive viewMessage;
        public string Title;
        [Multiline]
        public string Message;


        public void SetMessage()
        {
            viewMessage.Title.text = Title;
            viewMessage.Message.text = Message;
        }
    }
}
