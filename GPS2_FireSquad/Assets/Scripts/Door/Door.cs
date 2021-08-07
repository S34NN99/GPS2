using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    public Animator DoorAnimator;

    private string OpenDoor = "DoorOpen";
    private string CloseDoor = "DoorClose";

    public bool isLocked = false;

    public bool isOpen = false;

    public void OpenDoorAnimation()
    {
        if(!isLocked)
        {
            DoorAnimator.Play(OpenDoor);
            isOpen = true;
        }
        else if (isLocked)
        {
            Debug.Log("Door is locked");
        }
    }

    public void CloseDoorAnimation()
    {
        if (!isLocked)
        {
            DoorAnimator.Play(CloseDoor);
            isOpen = false;
        }
    }
}
