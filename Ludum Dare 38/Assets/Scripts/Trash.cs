using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace vnc
{
    /// <summary>
    /// Remove what you must forget
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class Trash : MonoBehaviour
    {
        UnityEngine.UI.Button btn;
        public UnityEvent OnDelete;

        void Start()
        {
            btn = GetComponent<UnityEngine.UI.Button>();
            btn.onClick.AddListener(Delete);   
        }

        void Delete()
        {
            OnDelete.Invoke();
            Destroy(transform.parent.gameObject);
        }
    }
}
