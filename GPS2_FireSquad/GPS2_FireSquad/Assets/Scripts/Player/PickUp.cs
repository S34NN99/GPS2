using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform carryingPosition;
    public Transform placePosition;

    [SerializeField]
    private GameObject interactableObject;


    public bool isCarryingVictim;
    public bool detectedVictim;

    private void OnTriggerStay(Collider other)
    {
        if (!isCarryingVictim)
        {
            if (other.tag == "Victim")
            {
                interactableObject = other.gameObject;
                detectedVictim = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isCarryingVictim)
        {
            interactableObject = null;
            detectedVictim = false;
        }
    }

    public void VictimInteraction()
    {
        if (!isCarryingVictim && detectedVictim)
        {
            interactableObject.GetComponent<Rigidbody>().useGravity = false;
            interactableObject.GetComponent<CapsuleCollider>().enabled = false;

            interactableObject.transform.parent.position = carryingPosition.position;
            interactableObject.transform.parent.rotation = carryingPosition.rotation;
            interactableObject.transform.parent.rotation = carryingPosition.rotation;
            interactableObject.transform.parent.parent = GameObject.Find("CarryingPosition").transform;
            isCarryingVictim = true;
        }
        else if (isCarryingVictim)
        {
            interactableObject.GetComponent<Rigidbody>().useGravity = true;
            interactableObject.GetComponent<CapsuleCollider>().enabled = true;

            interactableObject.transform.parent.position = placePosition.position;
            interactableObject.transform.parent.rotation = placePosition.rotation;

            interactableObject.transform.parent.parent = null;
            interactableObject = null;

            isCarryingVictim = false;
            detectedVictim = false;
        }
    }
}
