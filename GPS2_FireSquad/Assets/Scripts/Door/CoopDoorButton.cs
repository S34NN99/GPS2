using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopDoorButton : MonoBehaviour
{
    [SerializeField] private Animator ButtonAnimator;
    private string PressedButton = "ButtonPressed";
    private string ReleasedButton = "ButtonReleased";

    public Door coopDoor;

    public bool isPressed;

    public void ButtonPressed()
    {
        ButtonAnimator.Play(PressedButton);
        coopDoor.OpenDoorAnimation();
        coopDoor.isLocked = false;
        isPressed = true;
    }

    public void ButtonReleased()
    {
        ButtonAnimator.Play(ReleasedButton);
        coopDoor.CloseDoorAnimation();
        coopDoor.isLocked = true;
        isPressed = false;
    }
}
