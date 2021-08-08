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
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.CompareTag("Victim"))
        {
            Debug.Log("Victim saved");
            GameObject health = collision.gameObject;
            health.GetComponent<VictimHealth>().CancelInvoke();

            taskManager.UpdateValue(Objective.ObjectiveType.Rescue, 1f);
            taskManager.LevelProgression();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Teammate" + collision.gameObject.name + " entered safe area");

            taskManager.UpdateValue(Objective.ObjectiveType.Teammates, 1f);
            taskManager.LevelProgression();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Victim"))
        {
            Debug.Log("Victim is moved out of safe area");
            GameObject health = collision.gameObject;
            health.GetComponent<VictimHealth>().InvokeRepeating("HealthDecrease", 0f, health.GetComponent<VictimHealth>().healthChangeRate);

            taskManager.UpdateValue(Objective.ObjectiveType.Rescue, -1f);
            taskManager.LevelProgression();
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Teammate" + collision.gameObject.name + "exits safe area");

            taskManager.UpdateValue(Objective.ObjectiveType.Teammates, -1f);
            taskManager.LevelProgression();
        }
    }
}
