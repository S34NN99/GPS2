using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSelectedChar : MonoBehaviour
{
    public List<Button> charButton;


    private void Start()
    {
        ChangeColor(charButton[0]);
    }

    public void ChangeColor(Button thisBtn)
    {
       foreach(Button btn in charButton)
        {
            if(btn == thisBtn)
            {
                btn.gameObject.GetComponent<Image>().color = Color.grey;
            }
            else
            {
                btn.gameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
