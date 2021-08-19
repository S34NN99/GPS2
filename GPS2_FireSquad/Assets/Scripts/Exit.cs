using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject exitMenu;

    private void OnCollisionEnter(Collision collision)
    {
        exitMenu.gameObject.SetActive(true);
    }


}
