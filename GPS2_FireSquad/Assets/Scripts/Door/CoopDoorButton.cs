using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopDoorButton : MonoBehaviour
{
    private Animator ButtonAnimator => this.GetComponent<Animator>();
    private string PressedButton = "ButtonPressed";
    private string ReleasedButton = "ButtonReleased";

    public Animator coopDoor;

    public bool isPressed;

    public void ButtonPressed()
    {
        coopDoor.SetBool("DoorOpen", true);

        //ButtonAnimator.Play(PressedButton);
        //coopDoor.OpenDoorAnimation();
        //coopDoor.isLocked = false;
        //isPressed = true;
    }

    public void ButtonReleased()
    {
        coopDoor.SetBool("DoorOpen", false);
        //ButtonAnimator.Play(ReleasedButton);
        //coopDoor.CloseDoorAnimation();
        //coopDoor.isLocked = true;
        //isPressed = false;
    }
}
