using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        _animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Bedroom");
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameRoutine());
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    IEnumerator ExitGameRoutine()
    {
        _animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}
