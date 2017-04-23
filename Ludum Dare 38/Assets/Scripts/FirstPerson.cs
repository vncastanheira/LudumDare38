using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vnc
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPerson : MonoBehaviour
    {
        public static FirstPerson singleton;

        #region References
        CharacterController p_character;
        Camera p_Camera;
        #endregion

        #region Settings
        public float Speed;
        public bool InteractiveMode { get { return _interactiveMode; } set { _interactiveMode = value; } }

        bool _interactiveMode = false;
        bool lockedCursor = true;
        #endregion

        #region Unity Methods
        void Start()
        {
            p_character = GetComponent<CharacterController>();
            p_Camera = Camera.main;
            singleton = this;
        }

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            if(!InteractiveMode)
                CursorLock();

            if (lockedCursor)
            {
                Look(mouseX, mouseY);

                var walk = Input.GetAxis("Vertical") * p_character.transform.TransformDirection(Vector3.forward);
                var strafe = Input.GetAxis("Horizontal") * p_character.transform.TransformDirection(Vector3.right);

                p_character.SimpleMove((walk + strafe) * Speed);
            }
        }
        #endregion

        #region Private Methods
        void Look(float mouseX, float mouseY)
        {
            p_character.transform.rotation *= Quaternion.Euler(0f, mouseX, 0f);
            p_Camera.transform.localRotation *= Quaternion.Euler(-mouseY, 0f, 0f);
            p_Camera.transform.localRotation = ClampRotation(p_Camera.transform.localRotation, -90f, 90f);
        }

        void CursorLock()
        {
#if UNITY_EDITOR
            // Unlock cursor from game windwo
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                lockedCursor = !lockedCursor;
            }
#endif

            if (lockedCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotation(Quaternion q, float minAngle, float maxAngle)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, minAngle, maxAngle);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
        #endregion

        #region Static Methods
        public static void OnSetInteractiveMode()
        {
            singleton.InteractiveMode = true;
            singleton.lockedCursor = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public static void OnExitInteractiveMode()
        {
            singleton.InteractiveMode = false;
            singleton.lockedCursor = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
#endregion
    }

}

