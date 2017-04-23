using UnityEngine;
using UnityEngine.Events;
using vnc;

public class Interactive : MonoBehaviour {

    public string Name;
    public InteractivePanel Panel;
    public UnityEvent OnInteraction;

    [Tooltip("You cannot ineract with energy demanding tasks")]
    public bool RequireSleep;

    public virtual void Interact()
    {
        if (RequireSleep)
        {
            if(GameManager.singleton.Energy <= 0)
            {
                PlayerCanvas.AddLogEntry("Too tired...");
                return;
            }
        }

        if (Panel != null)
        {
            Panel.ShowPanel();
        }

        OnInteraction.Invoke();
    }
}
