using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
