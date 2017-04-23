using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour {

    public static PlayerCanvas singleton;

    [Header("Refernces")]
    public Text Hint;

	void Start () {
        singleton = this;
	}
	
    public static void ClearHint()
    {
        singleton.Hint.text = string.Empty;
    }

    public static void ShowHint(string label)
    {
        singleton.Hint.text = label;
    }
}
