using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vnc
{
    public class PlayerCanvas : MonoBehaviour
    {
        public static PlayerCanvas singleton;

        [Header("References")]
        public Text Hint;
        public VerticalLayoutGroup Log;
        public LayoutElement LogEntryPrefab;
        Animator animator;

        void Start()
        {
            singleton = this;
            animator = GetComponent<Animator>();
        }

        #region Hint
        // When the player look at the object, a "hint"
        // appesar so it can identify theobject
        public static void ClearHint()
        {
            singleton.Hint.text = string.Empty;
        }

        public static void ShowHint(string label)
        {
            singleton.Hint.text = label;
        }
        #endregion

        #region Log
        public static void AddLogEntry(string logtext, LogType logType = LogType.Neutral)
        {
            var log = Instantiate(singleton.LogEntryPrefab);
            var textComponent = log.GetComponentInChildren<Text>();
            textComponent.text = logtext;
            switch (logType)
            {
                case LogType.Neutral:
                    textComponent.color = Color.white;
                    break;
                case LogType.Good:
                    textComponent.color = Color.green;
                    break;
                case LogType.Moderate:
                    textComponent.color = Color.yellow;
                    break;
                case LogType.Bad:
                    textComponent.color = Color.red;
                    break;
                default:
                    textComponent.color = Color.white;
                    break;
            }

            log.transform.SetParent(singleton.Log.transform);
            log.transform.SetAsFirstSibling();
        }

        public static void FadeIn(bool isWhite = false)
        {
            if (isWhite)
            {
                singleton.animator.SetTrigger("WhiteFadeIn");
            }
            else
            {
                singleton.animator.SetTrigger("FadeIn");
            }
        }

        public static void FadeOut()
        {
            singleton.animator.SetTrigger("FadeOut");
        }

        #endregion
    }

    public enum LogType
    {
        Neutral,
        Good,
        Moderate,
        Bad
    }
}