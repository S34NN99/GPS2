using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject extinguisher;
    [SerializeField] private GameObject medic;
    [SerializeField] private GameObject demolisher;
    public DialogueObject dialogueObject;

    public DialogueUI dialogueUi;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerMovement playerMovement;

    private int tutorialCheck = 0;

    //public BoxCollider boxOne, boxTwo, boxThree;

    //Need to disable character swap buttons

    private void Start()
    {
        dialogueUi = GetComponent<DialogueUI>();
        dialogueObject = GetComponent<DialogueObject>();
        gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
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

    public void StartGameTutorial()
    {
        StartDialogueAndChecks(1);
    }

    public void StartDialogueAndChecks(int dialogueNumber)
    {
        if (tutorialCheck == dialogueNumber - 1)
        {
            dialogueUi.ShowDialogue(dialogueObject, dialogueNumber);
            tutorialCheck = dialogueNumber;
        }
    }



}
