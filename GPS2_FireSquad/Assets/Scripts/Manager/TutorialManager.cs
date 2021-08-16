using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject extinguisher;
    [SerializeField] private GameObject medic;
    [SerializeField] private GameObject demolisher;
    [SerializeField] private DialogueObject dialogueObject;

    [SerializeField] private DialogueUI dialogueUi;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private int tutorialCheck = 0;

    //Need to disable character swap buttons

    private void Start()
    {
        StartDialogueAndChecks(0);
    }

    private void Update()
    {
        if (!dialogueUi.dialogueBoxisOpen)
        {
            if (tutorialCheck == 1)
            {
                StartDialogueAndChecks(1);
            }

            #region Checks Starting Actions
            //Is extinguishing
            if (gameManager.playerObject.GetComponent<PlayerMovement>().myPlayer.isExtinguishing == true)
            {
                StartDialogueAndChecks(3);
            }

            //Is carrying
            if (gameManager.playerObject.GetComponent<PlayerMovement>().myPlayer.isCarryingVictim == true)
            {
                StartDialogueAndChecks(7);
            }
            #endregion

            #region Checks Character Swap
            //Swaps to Medic
            //if (gameManager.playerObject.GetComponent<PlayerMovement>().playerSelected == true && 
            if (gameManager.playerObject.GetComponent<PlayerMovement>().myPlayer.characterType == PublicEnumList.CharacterType.Medic)
            {
                StartDialogueAndChecks(5);
            }
            //Swaps to Demolisher
            //if (gameManager.playerObject.GetComponent<PlayerMovement>().playerSelected == true &&
            if (gameManager.playerObject.GetComponent<PlayerMovement>().myPlayer.characterType == PublicEnumList.CharacterType.Demolisher)
            {
                StartDialogueAndChecks(9);
            }
            #endregion

            #region Checks Completed Tasks
            //  "extinguishes them" trigger
            if (GetComponent<TaskManager>().ActiveObjectives[0].objectiveCompleted() == true)
            {
                StartDialogueAndChecks(4);
            }

            //  Carry all civilians
            if (GetComponent<TaskManager>().ActiveObjectives[1].objectiveCompleted() == true)
            {
                StartDialogueAndChecks(8);
            }

            //  Destroyed the one wall
            if (GetComponent<TaskManager>().ActiveObjectives[2].objectiveCompleted() == true)
            {
                StartDialogueAndChecks(11);
            }
            #endregion
        }
    }

    public void TriggerBoxDialogue(int dialogueNumber)
    {
        StartDialogueAndChecks(dialogueNumber);
    }

    public void StartDialogueAndChecks(int dialogueNumber)
    {
        Debug.Log("Dialogue : " + dialogueNumber + "\n TutorialCheck : " + tutorialCheck);
        if (tutorialCheck == dialogueNumber)
        {
            dialogueUi.ShowDialogue(dialogueObject, dialogueNumber);
            tutorialCheck = dialogueNumber + 1;
            Debug.Log("TutorialCheck : " + tutorialCheck);
        }
        else
        {
            Debug.LogWarning("ERROR FOR DIALOGUE" + dialogueNumber);
        }
    }

    

}
