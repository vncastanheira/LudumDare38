using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vnc
{
    public class EndGameManager : MonoBehaviour {

        public Transform Target;

        public void CinematicEnding()
        {
            StartCoroutine(CinematicEndCoroutine());
        }

        IEnumerator CinematicEndCoroutine()
        {
            PlayerCanvas.FadeIn(isWhite: true);
            FirstPerson.EnterCinematic(Target.position);
            yield return new WaitForSeconds(1);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
