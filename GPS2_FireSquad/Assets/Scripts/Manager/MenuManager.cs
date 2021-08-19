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

public class MenuManager : MonoBehaviour, IFmod
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
    public GameObject gameScreenMenu;
    public bool isMainMenu = false;
    private FMOD.Studio.EventInstance EI;


    #region FMOD
    public void StartAudioFmod(GameObject gameObject, string pathname)
    {
        // EXAMPLE
        /*AE = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Extinguisher/EXT_Extinguishing");
        AE.start();*/
        EI = FMODUnity.RuntimeManager.CreateInstance(pathname);
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(EI, gameObject.transform, gameObject.GetComponent<Rigidbody>());
        EI.start();
    }

    public void StopAudioFmod(GameObject gameObject)
    {
        EI.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        EI.release();
    }
    #endregion FMOD

    private void Start()
    {
        if (isMainMenu)
        {
            StartAudioFmod(this.gameObject, "event:/BGM/MainMenuBGM");
        }
    }

    //MAIN MENU
    public void PressButtonSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SelectUIButtonSFX");
    }


    public void SwitchLevelSelection()
    {
        mainMenu.SetActive(false);

        SetStarImage();
        StopAudioFmod(this.gameObject);
        StartAudioFmod(this.gameObject, "event:/BGM/LevelSelectBGM");
        levelSelectMenu.SetActive(true);

    }

    public void Setting()
    {
        mainMenu.SetActive(false);

        settingMenu.SetActive(true);
    }

    public void QuitGame()
    {
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
        StopAudioFmod(this.gameObject);
        StartAudioFmod(this.gameObject, "event:/BGM/MainMenuBGM");
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
        StopAudioFmod(this.gameObject);
        SceneManager.LoadScene(levelName);
    }

    //PAUSE MENU
    public void Pause()
    {
        gameScreenMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Confirm()
    {
        if (SaveHandler.sH)
        {
            SaveHandler.sH.SaveToJSON();
        }

        pauseMenu.SetActive(false);
        gameScreenMenu.SetActive(true);
    }

    public void BackExit(GameObject temp)
    {
        temp.SetActive(false);
    }

    public void LeaveLevel()
    {
        SceneManager.LoadScene("Main Menu");
        GameManager gameManager = this.gameObject.GetComponent<GameManager>();
        gameManager.StopAudioFmod(gameManager.cameraMovement.gameObject);
        //mainMenu.SetActive(false);
        //levelSelectMenu.SetActive(true);
    }

    void SetStarImage()
    {
        SaveHandler.sH.ReadFromJson();
        for (int i = 1; i < levelimage.Count; i++)
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
