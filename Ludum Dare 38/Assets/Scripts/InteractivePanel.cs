using UnityEngine;

namespace vnc
{
    public class InteractivePanel : MonoBehaviour
    {
        [Tooltip("If Main Panel, exiting it returns to first person mode")]
        public bool isMainPanel;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void ShowPanel()
        {
            if (isMainPanel)
                FirstPerson.OnSetInteractiveMode();
            gameObject.SetActive(true);
        }

        public void ExitPanel()
        {
            if (isMainPanel)
                FirstPerson.OnExitInteractiveMode();
            gameObject.SetActive(false);
        }
    }
}
