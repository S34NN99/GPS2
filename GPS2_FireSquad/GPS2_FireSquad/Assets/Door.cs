using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator DoorAnimator = null;

    private string OpenDoor = "DoorOpen";
    private string CloseDoor = "DoorClose";

    public bool isLocked = false;

    public bool isOpen = false;

    public void OpenDoorAnimation()
    {
        DoorAnimator.Play(OpenDoor);
        isOpen = true;
    }

    public void CloseDoorAnimation()
    {
        DoorAnimator.Play(CloseDoor);
        isOpen = false;
    }
}
