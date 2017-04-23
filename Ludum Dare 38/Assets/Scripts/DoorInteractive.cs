using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vnc
{
    public class DoorInteractive : Interactive
    {
        string doorName;
        string exitName = "Leave your small world";

        public InteractivePanel LeaveWorldPanel;

        void Start () {
            doorName = Name; 
	    }

        public override void Interact()
        {
            if(GameManager.singleton.SelfEsteem < 4)
            {
                base.Interact();
            }
            else
            {
                LeaveWorldPanel.ShowPanel();
            }
        }

    }
}
