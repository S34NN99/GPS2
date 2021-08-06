using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SummaryManagerNew : MonoBehaviour
{
    //Summary
    public Text textBestTimeSUM;
    public Text textTimeLeftSUM;

    public GameObject summaryMenuUI;

    public float bestTime;
    public float timeLeft;

    public GameObject[] HydrantStars;

    [SerializeField]
    private Timer timer;
    
    [SerializeField]
    private TaskManager taskManager;

    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        taskManager = FindObjectOfType<TaskManager>();

        //Start of game
        SetBestTime();

    }

    private void Update()
    {
        //In level UI
        //SetBestTime();
        //SetTimeLeft();

    }

    public void DisplayTime(float timeToDisplay, Text textToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

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

    public void DisplayFireHydrants()
    {
        //If Hydrants are individual/separate
        /*
        for (int i = 0; i < taskManager.numberOfConditionsMet(); i++)
        {
            HydrantStars[i].SetActive(true);
        }
        */

        HydrantStars[taskManager.numberOfConditionsMet()].SetActive(true);
    }

    public void SummaryDisplay()
    {
        summaryMenuUI.SetActive(true);
        Time.timeScale = 0f;

        PlayerPrefs.SetFloat("TimeLeft", timer.currentTime);

        UpdateBestTime();
        //Display best time in Summary menu
        SetBestTime();
        //Display time left in Summary menu
        SetTimeLeft();

        DisplayFireHydrants();


    }

    #region SceneChangeButtons
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
    #endregion SceneChangeButtons

    #region Debug Buttons
    private void DebugTime(float timeNumber)
    {
        UpdateBestTime();
        DisplayTime(timeNumber, textBestTimeSUM);
        DisplayTime(timeNumber, textTimeLeftSUM);
        //UpdateSlider();
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
    #endregion Debug Buttons
}
