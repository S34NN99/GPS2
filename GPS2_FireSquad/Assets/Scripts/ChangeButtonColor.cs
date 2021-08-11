using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonColor : MonoBehaviour
{
    public List<Button> charBtn;

    private void Start()
    {
        ChangeColor(charBtn[0]);
    }


    public void ChangeColor(Button btn)
    {
        foreach(Button button in charBtn)
        {
            if(button == btn)
            {
                button.gameObject.GetComponent<Image>().color = Color.grey;
            }
            else
            {
                button.gameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
