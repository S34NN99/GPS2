using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggers : MonoBehaviour
{
    public PublicEnumList.TriggerBoxes triggerBoxes;

    [SerializeField] private TutorialManager tutorialManager;

    private void Start()
    {
        //tutorialManager = GetComponent<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            switch (triggerBoxes)
            {
                case PublicEnumList.TriggerBoxes.ExtinguisherEnterRoom:

                    tutorialManager.dialogueUi.ShowDialogue(tutorialManager.dialogueObject, 2);
                    break;
                    
                case PublicEnumList.TriggerBoxes.MedicWalksToVictim:
                    tutorialManager.dialogueUi.ShowDialogue(tutorialManager.dialogueObject, 6);
                    break;
                    
                case PublicEnumList.TriggerBoxes.DemolisherWalksToWall:
                    tutorialManager.dialogueUi.ShowDialogue(tutorialManager.dialogueObject, 10);
                    break;
            }
        }
    }


}
