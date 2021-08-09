using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WallMesh
{
    public PublicEnumList.WallHP hp;
    public Material mat;
}


public class Wall : MonoBehaviour, IObjectives
{
    public int health = 3;
    public WallMesh[] mesh;
    private TaskManager taskManager;

    private void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    public void Damage()
    {
        health--;
        UpdateVisual();
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void UpdateVisual()
    {
        switch(health)
        {
            case 2:
                foreach(WallMesh mesh in mesh)
                {
                    if(mesh.hp == PublicEnumList.WallHP.Crack)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material = mesh.mat;
                    }
                }
                break;

            case 1:
                foreach (WallMesh mesh in mesh)
                {
                    if (mesh.hp == PublicEnumList.WallHP.Broken)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material = mesh.mat;
                    }
                }
                break;

            default:
                break;
        }
    }

    public void AddToObjective()
    {
        foreach (Objective obj in taskManager.ActiveObjectives)
        {
            if (obj.objectiveType == Objective.ObjectiveType.BreakWall)
            {
                obj.currentValue++;
                taskManager.LevelProgression();
                return;
            }
        }
    }
}
