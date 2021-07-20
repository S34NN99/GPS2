using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Objective
{
    public enum ObjectiveType { Rescue, Time, Teammates };
    //public enum ObjectiveGoal { victimsSaved, timeLeftSeconds, teammatesLeft };

    public string ObjectiveText(ObjectiveType thisObjectiveType, float currentValue, float maxValue)
    {
        string currVal = currentValue.ToString();
        string maxVal = maxValue.ToString();

        switch (thisObjectiveType)
        {
            case ObjectiveType.Rescue:
                return ("Rescue " + maxVal + " victims\t" + "(" + currVal + "/" + maxVal + ")\n").ToString();

            case ObjectiveType.Teammates:
                return ("Leave no teammates\t" + "(" + currVal + "/" + maxVal + ")\n").ToString();

            case ObjectiveType.Time:
                    return ("Complete in " + maxVal + " seconds\t" + "(" + currVal + "/" + maxVal + ")\n").ToString();

            default:
                {
                    Debug.LogWarning("No objective found.");
                    return null;
                }
        }
    }


    public Objective.ObjectiveType objectiveType;
    //public Objective.ObjectiveGoal objectiveGoal;
    public float objectiveValue;    // If time, in seconds
    public float currentValue;
    //public float ObjectiveTimeLeft { get => objectiveValue / 60; set => objectiveTime = objectiveValue / 60; }
    public bool objectiveCompleted()
    {
        return (objectiveValue >= currentValue);
    }
}

public class TaskManager : MonoBehaviour
{

    public Objective[] ActiveObjectives;

    public Text textObjectiveList;

    public GameObject taskObjectivesGO;

    [SerializeField] private bool listIsActive;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Timer timer;
    

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        timer = FindObjectOfType<Timer>();
    }

    private void Update()
    {

        foreach (Objective objective in ActiveObjectives)
        {
            Objective.ObjectiveType objType = objective.objectiveType;
            switch(objective.objectiveType)
            {
                //case Objective.ObjectiveType.Rescue:
                //    objective.currentValue = FindObjectsOfType<VictimHealth>().Length;  //To change
                //    break;
                case Objective.ObjectiveType.Time:
                    objective.currentValue = timer.currentTime;
                    break;
                //case Objective.ObjectiveType.Teammates:
                //    objective.currentValue = FindObjectsOfType<PlayerMovement>().Length;    //To change
                //    break;
            }
        }

        //Display task list

        UpdateObjectiveList();
    }

    public void UpdateValue(Objective.ObjectiveType thisObjectiveType, float value)
    {
        foreach (Objective objective in ActiveObjectives)
        {
            if (objective.objectiveType == thisObjectiveType)
            {
                objective.currentValue += value;
            }
        }
    }

    public void UpdateObjectiveList()
    {
        textObjectiveList.text = null;

        foreach (Objective objective in ActiveObjectives)
        {
            textObjectiveList.text += objective.ObjectiveText(objective.objectiveType, objective.currentValue, objective.objectiveValue);
        }
    }

    public void DisplayList()
    {
        if (!listIsActive)
        {
            taskObjectivesGO.SetActive(true);
            listIsActive = !listIsActive;
        }
        else if (listIsActive)
        {
            taskObjectivesGO.SetActive(false);
            listIsActive = !listIsActive;
        }   
    }

}
