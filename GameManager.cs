using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // States
    public enum GameState
    {
        Intro,
        Playing,
        GameOver
    }
    private GameState currentState = GameState.Intro;
    private int missCount = 0;
    public int maxMiss = 3;

    private int score = 0;

    private bool isRightDoorOpen = false;

    public float minDoorStateDuration = 3f;
    public float maxDoorStateDuration = 7f;

    private bool isDoorChanging = false;

    private UIManager uiManager;
    private PlayerController playerController;
    private FallingObjectSpawner objectSpawner;

    void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        playerController = FindFirstObjectByType<PlayerController>();
        objectSpawner = FindFirstObjectByType<FallingObjectSpawner>();

        SetGameState(GameState.Intro);

        StartCoroutine(RandomizeDoorState());
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Intro:
                if (playerController != null) playerController.Pause();
                if (objectSpawner != null) objectSpawner.Pause();
                break;

            case GameState.Playing:
                if (playerController != null) playerController.Resume();
                if (objectSpawner != null) objectSpawner.Resume();
                break;

            case GameState.GameOver:
                if (playerController != null) playerController.Pause();
                if (objectSpawner != null) objectSpawner.Pause();
                break;
        }

        if (uiManager != null)
        {
            uiManager.UpdateButtonState(currentState);
        }
    }

    public void StartGame()
    {
        score = 0;
        missCount = 0;

        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
            uiManager.UpdateMissCount(missCount);
        }

        SetGameState(GameState.Playing);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void RecordMiss()
    {
        missCount++;
        if (uiManager != null)
        {
            uiManager.ShowMiss();
            uiManager.UpdateMissCount(missCount);
        }

        // Check game end
        if (missCount >= maxMiss)
        {
            SetGameState(GameState.GameOver);
        }
    }
    public void AddScore(int scoreValue = 1)
    {
        score += scoreValue;

        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }
    }

    public bool IsRightDoorOpen()
    {
        return isRightDoorOpen;
    }

    private IEnumerator RandomizeDoorState()
    {
        while (true)
        {
            if (currentState == GameState.Playing && !isDoorChanging)
            {
                isDoorChanging = true;

                isRightDoorOpen = !isRightDoorOpen;

                if (uiManager != null)
                {
                    uiManager.UpdateDoorState(isRightDoorOpen);
                }

                float waitTime = Random.Range(minDoorStateDuration, maxDoorStateDuration);

                yield return new WaitForSeconds(waitTime);

                isDoorChanging = false;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }
}