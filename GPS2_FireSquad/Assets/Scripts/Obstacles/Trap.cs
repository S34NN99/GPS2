using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, IObjectives
{
    private GameManager gameManager;
    private TaskManager taskManager;
    public List<GameObject> trappedPlayer;


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
            if (target.myPlayer.characterType != PublicEnumList.CharacterType.Demolisher)
            {
                target.Stun(target);
                iPlayer.UniqueAnimation("Trap", true);
                trappedPlayer.Add(target.gameObject);
            }
        }
    }

    public void AddToObjective()
    {
        foreach(Objective obj in taskManager.ActiveObjectives)
        {
            if(obj.objectiveType == Objective.ObjectiveType.BreakTrap)
            {
                obj.currentValue++;
                taskManager.LevelProgression();
                return;
            }
        }
    }
}
