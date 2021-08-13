using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class LevelImage
{
    public PublicEnumList.LevelNum levelNum;
    public List<Sprite> levelImage;
}

public class MenuManager : MonoBehaviour
{
    public List<LevelImage> levelimage;
    public GameObject buttonGroup;

    [Header("MAIN MENU")]
    public GameObject mainMenu;
    public GameObject settingMenu;
    public GameObject levelSelectMenu;
    int gameLevels;

    //public static bool isPause = false;
    public GameObject pauseMenu;
    public GameObject gameMenu;

    //MAIN MENU
    public void SwitchLevelSelection()
    {
        mainMenu.SetActive(false);

       levelSelectMenu.SetActive(true);
        SetStarImage();
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
        SaveHandler.sH.SaveToJSON();

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

    void SetStarImage()
    {
        SaveHandler.sH.ReadFromJson();
        for (int i = 0; i < levelimage.Count; i++)
        {
            int stars = 0;
            for (int j = 0; j < SaveHandler.sH.myPlayerData.level.Count; j++)
            {
                if (levelimage[i].levelNum == SaveHandler.sH.myPlayerData.level[j].levelNum)
                {
                    stars = SaveHandler.sH.myPlayerData.level[j].objectivesCompleted;
                }
            }
            buttonGroup.transform.GetChild(i).GetComponent<Image>().sprite = levelimage[i].levelImage[stars];
        }
    }
}
