using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using vnc;

public class Interactive : MonoBehaviour {

    public string Name;
    public InteractivePanel Panel;
    public UnityEvent OnInteraction;

    public void Interact()
    {
        if (Panel != null)
        {
            Panel.ShowPanel();
        }

        OnInteraction.Invoke();
    }
}
