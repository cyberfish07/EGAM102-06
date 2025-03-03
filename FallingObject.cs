using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    private float speed;
    private int position;
    private bool isPaused = false;
    private GameManager gameManager;
    private PlayerController playerController;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerController = FindFirstObjectByType<PlayerController>();
    }

    public void Initialize(int pos, float fallingSpeed)
    {
        position = pos;
        speed = fallingSpeed;
    }

    void Update()
    {
        if (isPaused) return;

        transform.Translate(Vector3.down * speed * Time.deltaTime);
        // Bottom check
        if (transform.position.y <= -1.5f)
        {
            gameManager.AddScore();
            Destroy(gameObject);
        }
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player check
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                GameManager gameManager = FindAnyObjectByType<GameManager>();
                if (gameManager != null)
                {
                    gameManager.RecordMiss();
                }
                playerController.SetMissSprite();
                Destroy(gameObject);
            }
        }
    }
}