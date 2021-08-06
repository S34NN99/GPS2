using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public float startTime;
    public float currentTime = 0f;

    public Slider slider;
    public Text countdownText;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = startTime;
        currentTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        slider.value = currentTime;

        //countdownText.text = currentTime.ToString("0");

        if(currentTime <= 0)
        {
            //countdownText.color = Color.red;
            currentTime = 0;
        }
    }
}
