using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{

    public GameObject rightDoor;

    public Sprite doorOpenSprite;
    public Sprite doorCloseSprite;

    private SpriteRenderer rightDoorRenderer;

    void Start()
    {
        rightDoorRenderer = rightDoor.GetComponent<SpriteRenderer>();
        UpdateDoorState(false);
    }

    public void UpdateDoorState(bool isRightDoorOpen)
    {
        if (rightDoorRenderer != null)
        {
            rightDoorRenderer.sprite = isRightDoorOpen ? doorOpenSprite : doorCloseSprite;
        }
    }

    public bool IsRightDoorOpen()
    {
        if (rightDoorRenderer != null)
        {
            return rightDoorRenderer.sprite == doorOpenSprite;
        }
        return false;
    }
}