using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictimHealth : MonoBehaviour
{
    private MeshFilter thisMesh;

    private void Start()
    {
        //currentHP = maxHP;
        //healthBar.SetMaxHealth(maxHP);

        thisMesh = this.GetComponent<MeshFilter>();
        //InvokeRepeating("HealthDecrease", 0f, healthChangeRate);
    }

    public void CheckCarrying(bool isCarrying)
    {
        if(isCarrying)
        {
            thisMesh.sharedMesh = Resources.Load<Mesh>("Civilian(carried)");
        }
        else
        {
            thisMesh.sharedMesh = Resources.Load<Mesh>("Civilian(collapsed)");
        }
    }
}
