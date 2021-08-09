using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingMenu;
    public GameObject levelSelectMenu;
    int gameLevels;

    //public static bool isPause = false;
    public GameObject pauseMenu;
    public GameObject gameMenu;

    public AudioMixer music;

    //MAIN MENU
    public void SwitchLevelSelection()
    {
        mainMenu.SetActive(false);

       levelSelectMenu.SetActive(true);
    }

    public void Setting()
    {
        mainMenu.SetActive(false);

        settingMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    //SETTINGS
    public void AdjustVolume(float sliderValue)
    {
        music.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void AdjustBrightness(float rgbValue)
    {
        RenderSettings.ambientLight = new Color(rgbValue, rgbValue, rgbValue, 1);
    }

    public void Back()
    {
        mainMenu.SetActive(true);

        settingMenu.SetActive(false);

    }

    public void LevelBack()
    {
        mainMenu.SetActive(true);

        levelSelectMenu.SetActive(false);

    }

    public void SaveSetting()
    {
        mainMenu.SetActive(true);

        settingMenu.SetActive(false);
    }


    //LEVEL SELECTION
    string levelName;

    public void SelectLevel(string level)
    {
        levelName = level;
    }

    public void PlayLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    //PAUSE MENU
    public void Pause()
    {
        gameMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Confirm()
    {
        gameMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void LeaveLevel()
    {
        SceneManager.LoadScene("Main Menu");
        //mainMenu.SetActive(false);
        //levelSelectMenu.SetActive(true);
    }
}
