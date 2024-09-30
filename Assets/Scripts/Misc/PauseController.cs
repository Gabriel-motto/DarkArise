using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;

            if (currentGameState == GameState.Running)
            {   
                canvas.gameObject.SetActive(true);
            }
            else
            {
                canvas.gameObject.SetActive(false);
            }

            GameState newGameState = currentGameState == GameState.Running ? GameState.Paused : GameState.Running;

            GameStateManager.Instance.SetState(newGameState);
        }
    }

    public void OnContinueBtnClick()
    {
        canvas.gameObject.SetActive(false);

        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.Running ? GameState.Paused : GameState.Running;

        GameStateManager.Instance.SetState(newGameState);
    }
}
