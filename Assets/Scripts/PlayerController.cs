using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private int currentPosition = 0;
    public Sprite[] playerSprites;
    public Sprite missSprite;

    private SpriteRenderer spriteRenderer;
    public bool usePhysicalMovement = false;
    public Transform[] positionMarkers;
    private bool isPaused = false;
    // Teleport
    private bool isBeingTeleported = false;
    private float teleportDelay = 0.1f;

    private GameManager gameManager;
    private float missDuration = 1.0f;
    private float missTimer = 0f;
    private bool isInMissState = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindFirstObjectByType<GameManager>();
        // Initial
        UpdatePlayerVisuals();
    }

    void Update()
    {
        if (!isPaused && !isBeingTeleported)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }
        }

        // Miss Status
        if (isInMissState)
        {
            missTimer -= Time.deltaTime;
            if (missTimer <= 0)
            {
                isInMissState = false;
                UpdatePlayerVisuals();
                StartCoroutine(TeleportToLeftDoor());
            }
        }
    }

    void MoveLeft()
    {
        if (currentPosition > 0)
        {
            currentPosition--;
            UpdatePlayerVisuals();
        }
    }

    void MoveRight()
    {
        if (currentPosition < 6)
        {
            currentPosition++;
            UpdatePlayerVisuals();
            // Check Position
            if (currentPosition == 6 && gameManager != null && gameManager.IsRightDoorOpen())
            {
                gameManager.AddScore();
                StartCoroutine(TeleportToLeftDoor());
            }
        }
    }

    private IEnumerator TeleportToLeftDoor()
    {
        isBeingTeleported = true;
        yield return new WaitForSeconds(teleportDelay);
        currentPosition = 0;
        UpdatePlayerVisuals();

        isBeingTeleported = false;
    }

    void UpdatePlayerVisuals()
    {
        if (isInMissState) return;

        if (spriteRenderer != null && playerSprites.Length > currentPosition)
        {
            spriteRenderer.sprite = playerSprites[currentPosition];
        }
        // Move with sprites change
        if (usePhysicalMovement && positionMarkers != null && positionMarkers.Length > currentPosition)
        {
            transform.position = positionMarkers[currentPosition].position;
        }

        Debug.Log($"Player moved to position {currentPosition}");
    }

    public void SetMissSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = missSprite;

            missTimer = missDuration;
            isInMissState = true;
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

    public bool IsAtPosition(int position)
    {
        return currentPosition == position;
    }

    public int GetCurrentPosition()
    {
        return currentPosition;
    }
}