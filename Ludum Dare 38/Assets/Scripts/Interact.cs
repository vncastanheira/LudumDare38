using UnityEngine;

namespace vnc
{
    [RequireComponent(typeof(FirstPerson))]
    public class Interact : MonoBehaviour
    {
        [Header("References")]
        public LayerMask InteractiveLayer;
        FirstPerson firstPerson;

        [Header("Settings")]
        public float RaycastRange;
        public string Command;

        Interactive _currentSelection;
        bool HasSelection { get { return _currentSelection != null; } }

        void Start()
        {
            firstPerson = GetComponent<FirstPerson>();
        }

        void Update()
        {
            PlayerCanvas.ClearHint();
            if (!firstPerson.InteractiveMode)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, RaycastRange, InteractiveLayer, QueryTriggerInteraction.Collide))
                {
                    _currentSelection = hit.collider.GetComponent<Interactive>();
                    if (_currentSelection != null)
                        PlayerCanvas.ShowHint(_currentSelection.Name);
                }
                else
                {
                    _currentSelection = null;
                }
            }

            // Interact with object
            if (Input.GetButtonDown(Command) && HasSelection)
            {
                _currentSelection.Interact();
                _currentSelection = null;
            }
        }
    }
}
