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

    [SerializeField] private GameManager gameManager;
    //[SerializeField] private SaveHandler saveHandler;

    private void Start()
    {
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
        DisplayTime(bestTime, textBestTimeSUM);
    }

    public void SetTimeLeft()
    {
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

        //saveHandler.ReadFromJson();

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

    public void GetScoreFromJson()
    {
        //check if this level is in save file

        if (SaveHandler.sH)
        {
            foreach (Levels lvl in SaveHandler.sH.myPlayerData.level)
            {
                if (lvl.levelNum == gameManager.currentLevel)
                {
                    lvl.timeLeft = timer.currentTime;
                    lvl.objectivesCompleted = taskManager.numberOfConditionsMet();

                    if (timer.currentTime < bestTime)
                    {
                        lvl.bestTime = timer.currentTime;
                    }
                    SaveHandler.sH.SaveToJSON();

                    bestTime = lvl.bestTime;
                    timeLeft = lvl.timeLeft;
                    Debug.Log("Existing level found");
                    return;
                }
            }
        }

        //if it doesnt exist
        Levels newLevel = new Levels();
        newLevel.levelNum = gameManager.currentLevel;
        newLevel.timeLeft = timer.currentTime;
        newLevel.bestTime = timer.currentTime;
        newLevel.objectivesCompleted = taskManager.numberOfConditionsMet();
        SaveHandler.sH.myPlayerData.level.Add(newLevel);
        SaveHandler.sH.SaveToJSON();

        bestTime = newLevel.bestTime;
        timeLeft = newLevel.timeLeft;
    }

    public void SummaryDisplay()
    {
        summaryMenuUI.SetActive(true);
        gameManager.StopAudioFmod(gameManager.cameraMovement.gameObject);
        Time.timeScale = 0f;

        //PlayerPrefs.SetFloat("TimeLeft", timer.currentTime);
        GetScoreFromJson();


        //UpdateBestTime();
        ////Display best time in Summary menu
        SetBestTime();
        ////Display time left in Summary menu
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

    public void NextScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
