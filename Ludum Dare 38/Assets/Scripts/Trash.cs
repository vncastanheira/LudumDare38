using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vnc
{
    /// <summary>
    /// Remove what you must forget
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class Trash : MonoBehaviour
    {
        UnityEngine.UI.Button btn;
        public GameAction action;

        void Start()
        {
            btn = GetComponent<UnityEngine.UI.Button>();
            btn.onClick.AddListener(Delete);   
        }

        void Delete()
        {
            GameManager.singleton.Action_DeleteMessage(action);
            Destroy(transform.parent.gameObject);
        }
    }
}
