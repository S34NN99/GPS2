using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SummaryManager : MonoBehaviour
{
    //Summary
    public Text textBestTimeSUM;
    public Text textTimeLeftSUM;

    public Slider medalSlider;

    public GameObject summaryMenuUI;

    public float bestTime;
    public float timeLeft;

    //Values
    private float medalSliderIncreaseRate = 0.001f;
    private float medalSliderIncreaseValue = 1f;

    //Numbers for testing
    private float bronzeTime = 540f;
    private float silverTime = 420f;
    private float goldTime = 240f;

    public GameObject bronze;
    public GameObject silver;
    public GameObject gold;

    private void Start()
    {
        //Start of game
        SetBestTime();
        UpdateSlider();
    }

    private void Update()
    {
        //In level UI
        SetBestTime();
        //SetTimeLeft();
        
    }

    public void DisplayTime(float timeToDisplay, Text textToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        minutes = Mathf.Round(minutes * 100f) / 100f;
        seconds = Mathf.Round(seconds * 100f) / 100f;

        textToDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetBestTime()
    {
        bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        DisplayTime(bestTime, textBestTimeSUM);
    }

    public void SetTimeLeft()
    {
        timeLeft = PlayerPrefs.GetFloat("TimeLeft", 0f);
        DisplayTime(timeLeft, textTimeLeftSUM);
    }

    public void UpdateBestTime()
    {
        bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        timeLeft = PlayerPrefs.GetFloat("TimeLeft", 0f);

        if (timeLeft < bestTime)
        {
            bestTime = timeLeft;

            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save();
        }
        else
        {
            bestTime = PlayerPrefs.GetFloat("BestTime");
        }
    }

    public void SummaryDisplay()
    {
        summaryMenuUI.SetActive(true);
        Time.timeScale = 0f;

        //Display best time in Summary menu
        SetBestTime();
        //Display time left in Summary menu
        SetTimeLeft();
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 1f;
        //player.gameOver = false;
        SceneManager.LoadScene("Main Menu");
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        //player.gameOver = false;
        SceneManager.LoadScene("SampleScene");
    }

    //Update Timer Slider
    public void UpdateSlider()
    {
        //medalSlider.value = timeLeft;
        if (medalSlider.value < timeLeft - 10)
        {
            medalSlider.value += medalSliderIncreaseValue * 2;
        }
        else
        {
            medalSlider.value += medalSliderIncreaseValue;
        }

        if (medalSlider.value == timeLeft)
        {
            CancelInvoke("UpdateSlider");
        }
    }

    public void DisplayMedals()
    {
        if (timeLeft <= goldTime)
        {
            bronze.SetActive(false);
            silver.SetActive(false);
            gold.SetActive(true);
        }
        else if (timeLeft <= silverTime)
        {
            bronze.SetActive(false);
            silver.SetActive(true);
            gold.SetActive(false);
        }
        else if (timeLeft <= bronzeTime)
        {
            bronze.SetActive(true);
            silver.SetActive(false);
            gold.SetActive(false);
        }
        else
        {
            bronze.SetActive(false);
            silver.SetActive(false);
            gold.SetActive(false);
        }
    }

    //Debug
    private void DebugTime(float timeNumber)
    {
        UpdateBestTime();
        DisplayTime(timeNumber, textBestTimeSUM);
        DisplayTime(timeNumber, textTimeLeftSUM);
        //UpdateSlider();

        //Fill up medal slider bar
        medalSlider.value = 0f;
        InvokeRepeating("UpdateSlider", 0f, medalSliderIncreaseRate);
        
        
        DisplayMedals();
    }

    public void Button1()
    {
        timeLeft = 570f;    //9:30
        PlayerPrefs.SetFloat("TimeLeft", timeLeft);
        PlayerPrefs.Save();
        DebugTime(timeLeft);
    }

    public void Button2()
    {
        timeLeft = 495f;    //8:15
        PlayerPrefs.SetFloat("TimeLeft", timeLeft);
        PlayerPrefs.Save();
        DebugTime(timeLeft);
    }

    public void Button3()
    {
        timeLeft = 332f;    //5:32
        PlayerPrefs.SetFloat("TimeLeft", timeLeft);
        PlayerPrefs.Save();
        DebugTime(timeLeft);
    }

    public void Button4()
    {
        timeLeft = 225f;    //3:45
        PlayerPrefs.SetFloat("TimeLeft", timeLeft);
        PlayerPrefs.Save();
        DebugTime(timeLeft);
    }

    public void ClearAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("BestTime", 600f);
    }

}
