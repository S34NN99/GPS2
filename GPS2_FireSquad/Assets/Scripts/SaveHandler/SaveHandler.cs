using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Levels
{
    public PublicEnumList.LevelNum levelNum;
    public int objectivesCompleted;
    public float bestTime;
    public float timeLeft;

    public Levels()
    {
        levelNum = PublicEnumList.LevelNum.Level_1;
        objectivesCompleted = 0;
        bestTime = 0;
        timeLeft = 0;
    }
}

[System.Serializable]
public class PlayerData
{
    public bool isInitialized;

    public float Master;
    public float BGM;
    public float SFX;
    public List<Levels> level = new List<Levels>();


    public PlayerData()
    {
        isInitialized = true;
        Master = 1;
        BGM = 0.5f;
        SFX = 0.5f;

        level = new List<Levels>();
    }
}

public class SaveHandler : MonoBehaviour
{
    public PlayerData myPlayerData;
    public static SaveHandler sH;

    // Start is called before the first frame update
    private void Awake()
    {
        MakeThisTheOnlyGameManager();
        ReadFromJson();
    }

    void MakeThisTheOnlyGameManager()
    {
        if (sH == null)
        {
            DontDestroyOnLoad(gameObject);
            sH = this;
        }
        else
        {
            if (sH != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetPlayerData(PlayerData newdata)
    {
        if (myPlayerData.isInitialized == false)
        {
            myPlayerData = new PlayerData();
        }
        else
        {
            myPlayerData = newdata;
        }
    }

    //change dataPath to persistenDataPath when building
    public PlayerData ReadFromJson()
    {
        //string myDir = Application.dataPath + "/PlayerSaves/playerData.json";
        string myDir = Path.Combine(Application.persistentDataPath, "PlayerSave", "playerData.json");
        if (File.Exists(myDir))
        {
            string myJson = File.ReadAllText(myDir);
            if (myJson.Length <= 0)
            {
                SaveToJSON();
                Debug.Log("no data");
            }
            myPlayerData = JsonUtility.FromJson<PlayerData>(myJson);
            return myPlayerData;
        }
        else
        {
            Debug.Log("no file found");
            SaveToJSON();


            return null;
        }
    }

    public void SaveToJSON()
    {
        //string filePath = Path.Combine(Application.dataPath, "PlayerSaves", "playerData.json");
        string filePath = Path.Combine(Application.persistentDataPath, "PlayerSave", "playerData.json");
        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
            Debug.Log("Creating Path");
            //Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "PlayerSave"));
        }

        SetPlayerData(myPlayerData);
        string myJson = JsonUtility.ToJson(myPlayerData);
        //string myDir = Path.Combine(Application.dataPath, "PlayerSaves", "playerData.json");
        string myDir = Path.Combine(Application.persistentDataPath, "PlayerSaves", "playerData.json");
        File.WriteAllText(myDir, myJson);
        ReadFromJson(); //reads stored data
    }
}
