using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vnc
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPerson : MonoBehaviour
    {

        #region Private Fields
        CharacterController p_character;
        Camera p_Camera;

        bool lockedCursor = true;
        #endregion

        #region Public Fields
        public float Speed;
        #endregion

        void Start()
        {
            p_character = GetComponent<CharacterController>();
            p_Camera = Camera.main;
        }

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Look(mouseX, mouseY);
            CursorLock();

            var walk = Input.GetAxis("Vertical") * p_character.transform.TransformDirection(Vector3.forward);
            var strafe = Input.GetAxis("Horizontal") * p_character.transform.TransformDirection(Vector3.right);

            p_character.SimpleMove((walk + strafe) * Speed);
        }

        void Look(float mouseX, float mouseY)
        {
            p_character.transform.rotation *= Quaternion.Euler(0f, mouseX, 0f);
            p_Camera.transform.localRotation *= Quaternion.Euler(-mouseY, 0f, 0f);
            p_Camera.transform.localRotation = ClampRotation(p_Camera.transform.localRotation, -90f, 90f);
        }

        void CursorLock()
        {
            // Unlock cursor from game windwo
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                lockedCursor = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                lockedCursor = true;
            }

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
    }

}

