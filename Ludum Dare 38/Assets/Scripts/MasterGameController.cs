using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vnc
{
    // MONOLITHIC GAME CONTROLLER MASTER OF ALL SCENES
    public class MasterGameController : MonoBehaviour
    {
        public Image Panel;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Panel.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseResumeGame();
            }

    #if UNITY_EDITOR
            Cheats();
    #endif
        }

        public void PauseResumeGame()
        {
            Panel.gameObject.SetActive(!Panel.gameObject.activeInHierarchy);
            FirstPerson.singleton.lockedCursor = !FirstPerson.singleton.lockedCursor;
            GameManager.Singleton.isPaused = !GameManager.Singleton.isPaused;
        }

        public void ExitGame()
        {
            Application.Quit();
        }


    #if UNITY_EDITOR
        void Cheats()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                GameManager.Singleton.SelfEsteem = 5;
            }
        }
    #endif
    }

}
