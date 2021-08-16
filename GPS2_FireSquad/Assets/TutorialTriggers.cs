using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggers : MonoBehaviour
{
    public PublicEnumList.TriggerBoxes triggerBoxes;

    [SerializeField] private TutorialManager tutorialManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            switch (triggerBoxes)
            {
                case PublicEnumList.TriggerBoxes.ExtinguisherEnterRoom:
                    tutorialManager.TriggerBoxDialogue(2);
                    break;
                    
                case PublicEnumList.TriggerBoxes.MedicWalksToVictim:
                    tutorialManager.TriggerBoxDialogue(6);
                    break;
                    
                case PublicEnumList.TriggerBoxes.DemolisherWalksToWall:
                    tutorialManager.TriggerBoxDialogue(10);
                    break;
            }
        }
    }


}
