using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingMenu;
    public GameObject levelSelectMenu;
    int gameLevels;
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

    public void AdjustVolume()
    {

    }

    public void AdjustBrightness()
    {

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

    public void SelectLevel1()
    {
        gameLevels = 0;
        Debug.Log("Select Level 1");
    }

    public void SelectLevel2()
    {
        gameLevels = 1;
        Debug.Log("Select Level 2");
    }

    public void SelectLevel3()
    {
        gameLevels = 2;
        Debug.Log("Select Level 3");
    }

    public void SelectLevel4()
    {
        gameLevels = 3;
        Debug.Log("Select Level 4");
    }

    public void SelectLevel5()
    {
        gameLevels = 4;
        Debug.Log("Select Level 5");
    }

    public void SelectLevel6()
    {
        gameLevels = 5;
        Debug.Log("Select Level 6");
    }

    public void SelectLevel7()
    {
        gameLevels = 6;
        Debug.Log("Select Level 7");
    }

    public void PlayLevel()
    {
        int tempSelectLevel = gameLevels;
        switch (tempSelectLevel)
        {
            case 0:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Debug.Log("Play Level 1");
                break;

            case 1:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
                Debug.Log("Play Level 2");
                break;

            case 2:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
                Debug.Log("Play Level 3");
                break;

            case 3:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
                Debug.Log("Play Level 4");
                break;

            case 4:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5);
                Debug.Log("Play Level 5");
                break;

            case 5:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 6);
                Debug.Log("Play Level 6");
                break;

            case 6:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 7);
                Debug.Log("Play Level 7");
                break;
        }
    }
}
