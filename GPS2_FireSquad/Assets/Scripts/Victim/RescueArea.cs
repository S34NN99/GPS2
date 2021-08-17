using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueArea : MonoBehaviour
{

    [SerializeField] private TaskManager taskManager;

    private void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Victim"))
        {
            GameObject health = collision.gameObject;
            health.GetComponent<VictimHealth>().CancelInvoke();

            taskManager.UpdateValue(Objective.ObjectiveType.Rescue, 1f);
            taskManager.LevelProgression();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {

            taskManager.UpdateValue(Objective.ObjectiveType.Teammates, 1f);
            taskManager.LevelProgression();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Victim"))
        {
            GameObject health = collision.gameObject;

            taskManager.UpdateValue(Objective.ObjectiveType.Rescue, -1f);
            taskManager.LevelProgression();
        }
        else if(collision.gameObject.CompareTag("Player"))
        {

            taskManager.UpdateValue(Objective.ObjectiveType.Teammates, -1f);
            taskManager.LevelProgression();
        }
    }
}
