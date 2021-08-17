using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlick : MonoBehaviour, IObjectives
{
    private GameManager gameManager;
    private TaskManager taskManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        taskManager = FindObjectOfType<TaskManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement target = collision.gameObject.GetComponent<PlayerMovement>();
            IPlayer iPlayer = target.GetComponent<IPlayer>();
            if (target.myPlayer.characterType != PublicEnumList.CharacterType.Medic)
            {
                target.Stun(target);
                iPlayer.UniqueAnimation("Slip", true);
                AddToObjective();
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/OilSlip");
                Destroy(this.gameObject);
            }
        }
    }

    public void AddToObjective()
    {
        foreach (Objective obj in taskManager.ActiveObjectives)
        {
            if (obj.objectiveType == Objective.ObjectiveType.RemoveOil)
            {
                obj.currentValue++;
                taskManager.LevelProgression();
                return;
            }
        }
    }
}
