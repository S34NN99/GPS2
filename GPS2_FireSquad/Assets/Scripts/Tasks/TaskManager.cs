using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Objective
{
    public enum ObjectiveType { Rescue, Time, Teammates };

    public string ObjectiveText(ObjectiveType thisObjectiveType, float currentValue, float maxValue)
    {
        string currVal = currentValue.ToString();
        string maxVal = maxValue.ToString();

        if (!objectiveCompleted())
        {
            switch (thisObjectiveType)
            {
                case ObjectiveType.Rescue:
                    return ("Rescue " + maxVal + " victims\t" + "(" + currVal + "/" + maxVal + ")\n").ToString();

                case ObjectiveType.Teammates:
                    return ("Leave no teammates\t" + "(" + currVal + "/" + maxVal + ")\n").ToString();

                case ObjectiveType.Time:
                    return ("Complete within " + maxVal + " seconds\t" + "(" + currVal + "/" + maxVal + ")\n").ToString();

                default:
                    {
                        Debug.LogWarning("No objective found.");
                        return null;
                    }
            }
        }
        else if (objectiveCompleted())
        {
            switch (thisObjectiveType)
            {
                case ObjectiveType.Rescue:
                    return ("All " + maxVal + " victims are saved\n").ToString();

                case ObjectiveType.Teammates:
                    return ("All " + currVal + " of " + maxVal + " teammates are in the safe zone.\n").ToString();

                case ObjectiveType.Time:
                    return ("Complete within " + maxVal + " seconds\t" + "(" + currVal + "/" + maxVal + ")\n").ToString();

                default:
                    {
                        Debug.LogWarning("No objective found.");
                        return null;
                    }
            }
        }
        else
        {
            return null;
        }
        
    }


    public Objective.ObjectiveType objectiveType;
    //public Objective.ObjectiveGoal objectiveGoal;
    public float objectiveValue;    // If time, in seconds
    public float currentValue;
    //public float ObjectiveTimeLeft { get => objectiveValue / 60; set => objectiveTime = objectiveValue / 60; }
    public bool objectiveCompleted()
    {
        return (currentValue >= objectiveValue);
    }
    /*
    public bool loseCondition()
    {
        if (objectiveType == Objective.ObjectiveType.Time)
        {
            return (currentValue <= 0);
        }
        else
        {
            return false;
        }

            
        switch (objectiveType)
        {
            case Objective.ObjectiveType.Time:
                return (currentValue <= 0);     // Lose when time reaches 0

            case Objective.ObjectiveType.Rescue:
                return (currentValue <= 0);     //

            case Objective.ObjectiveType.Teammates:
                return (currentValue <= 0);

            default:
                return false;
        }
    }
    */
}

public class TaskManager : MonoBehaviour
{

    public Objective[] ActiveObjectives;

    public Text textObjectiveList;

    public GameObject taskObjectivesGO;

    [SerializeField] private bool listIsActive;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Timer timer;
    [SerializeField] private SummaryManagerNew summaryManager;

    //[SerializeField] private GameObject[] teammates;

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

        LevelProgression();
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

    #region Win/Lose Conditions

    public void LevelProgression()
    {
        if (timeRanOut() && numberOfConditionsMet() < 0)
        {
            Debug.Log("Level Lost");
            summaryManager.SummaryDisplay();
        }
        else if (timeRanOut() && numberOfConditionsMet() <= 2)
        {
            Debug.Log("Level Won: Time Ran Out");
            summaryManager.SummaryDisplay();
        }
        else if (numberOfConditionsMet() == ActiveObjectives.Length)
        {
            Debug.Log("Level Won: 3 Stars");
            summaryManager.SummaryDisplay();
        }
    }

    public bool timeRanOut()
    {
        foreach (Objective objective in ActiveObjectives)
        {
            if (objective.objectiveType == Objective.ObjectiveType.Time && objective.currentValue <= 0)
            {
                return true;
            }
        }
        return false;
    }

    public int numberOfConditionsMet()
    {
        int completedCount = 0;
        foreach (Objective objective in ActiveObjectives)
        {
            if (objective.objectiveCompleted())
            {
                completedCount += 1;
            }
        }

        return completedCount;
    }

    #endregion Win/Lose Condition
}