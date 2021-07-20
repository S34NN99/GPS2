using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictimHealth : MonoBehaviour
{
    private MeshFilter thisMesh;
    
    private float maxHP = 100;        //max HP of victim, for start of level and update use
    [SerializeField]
    private float currentHP;    //current HP of victim

    public float healthChangeRate = 1f;
    private float healthChangeValue = 1f;

    public HealthBar healthBar;

    private void Start()
    {
        currentHP = maxHP;
        healthBar.SetMaxHealth(maxHP);

        thisMesh = this.GetComponent<MeshFilter>();
        InvokeRepeating("HealthDecrease", 0f, healthChangeRate);
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

    private void HealthDecrease()
    {
        currentHP -= healthChangeValue;
        healthBar.SetHealth(currentHP);
    }
}
