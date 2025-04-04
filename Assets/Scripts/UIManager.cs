using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject missTextObject;
    public GameObject[] missIcons;
    public Button gameControlButton;
    private DoorSystem doorSystem;
    private GameManager gameManager;

    void Start()
    {
        doorSystem = FindAnyObjectByType<DoorSystem>();
        gameManager = FindAnyObjectByType<GameManager>();
        missTextObject.SetActive(false);
        foreach (GameObject icon in missIcons)
        {
            icon.SetActive(false);
        }
        // On click
        if (gameControlButton != null)
        {
            gameControlButton.onClick.AddListener(OnGameControlButtonClick);
        }
        UpdateButtonState(gameManager.GetCurrentState());
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
    // Miss text
    public void ShowMiss()
    {
        missTextObject.SetActive(true);
    }


    // Miss icon
    public void UpdateMissCount(int count)
    {
        for (int i = 0; i < missIcons.Length; i++)
        {
            missIcons[i].SetActive(i < count);
        }
    }
    public void UpdateDoorState(bool isRightDoorOpen)
    {
        if (doorSystem != null)
        {
            doorSystem.UpdateDoorState(isRightDoorOpen);
        }
    }

    public void UpdateButtonState(GameManager.GameState gameState)
    {
        if (gameControlButton != null)
        {
            switch (gameState)
            {
                case GameManager.GameState.Intro:
                    gameControlButton.gameObject.SetActive(true);
                    break;

                case GameManager.GameState.Playing:
                    gameControlButton.gameObject.SetActive(false);
                    break;

                case GameManager.GameState.GameOver:
                    gameControlButton.gameObject.SetActive(true);
                    break;
            }
        }
    }

    void OnGameControlButtonClick()
    {
        if (gameManager != null)
        {
            GameManager.GameState currentState = gameManager.GetCurrentState();

            if (currentState == GameManager.GameState.Intro)
            {
                gameManager.StartGame();
            }
            else if (currentState == GameManager.GameState.GameOver)
            {
                gameManager.RestartGame();
            }
        }
    }
}