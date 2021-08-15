using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject extinguisher;
    [SerializeField] private GameObject medic;
    [SerializeField] private GameObject demolisher;
    [SerializeField] private DialogueObject dialogueObject;

    private DialogueUI dialogueUi;

    //public BoxCollider boxOne, boxTwo, boxThree;

    private void Start()
    {
        dialogueUi = GetComponent<DialogueUI>();
        dialogueObject = GetComponent<DialogueObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == extinguisher)
        {
            Time.timeScale = 0f;
            string dialogue = dialogueObject.Dialogue[2];
        }

        if (other.gameObject == medic)
        {
            Time.timeScale = 0f;
            dialogueUi.ShowDialogue(dialogueObject);
        }

        if (other.gameObject == demolisher)
        {
            Time.timeScale = 0f;
            dialogueUi.ShowDialogue(dialogueObject);
        }
    }
}
