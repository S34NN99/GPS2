using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

[System.Serializable]
public class AbilityHolder
{
    public PublicEnumList.CharacterType character;
    public Sprite image;
}

public class GameManager : MonoBehaviour, IFmod
{
    [Header("LEVEL")]
    public PublicEnumList.LevelNum currentLevel;
    private GameObject[] playerGroup => GameObject.FindGameObjectsWithTag("Player");

    [Space(20)]
    public CameraMovement cameraMovement;
    public GameObject playerObject;
    public Button actionBtn;

    [Header("Prefabs")]
    public GameObject stunPrefab;
    public GameObject holdTimerPrefab;
    public GameObject firePrefab;

    [Header("CHARACTERS")]
    public List<AbilityHolder> characterAbility;
    public bool isPressed = false;
    public bool isGrouping = false;

    float maxCountDown = 2.0f;
    float currCountDown;
    //public Image timer;

    private FMOD.Studio.EventInstance EI;

    void Start()
    {
        //StartAudioFmod(cameraMovement.gameObject, "event:/BGM/bgm");
    }

    #region NAVMESH
    void CheckCharDistance()
    {
        Vector3 selectedPlayerPos = playerObject.transform.position;

        for (int i = 0; i < playerGroup.Length; i++)
        {
            PlayerMovement playermovement = playerGroup[i].GetComponent<PlayerMovement>();
            NavMeshAgent navMeshAgent = playermovement.GetComponent<NavMeshAgent>();
            if (!playermovement.playerSelected && !playermovement.myPlayer.isStunned && !playermovement.myPlayer.characterCoroutine.isInCoroutine)
            {
                IPlayer iPlayer = navMeshAgent.gameObject.GetComponent<IPlayer>();
                Vector3 notSelectedPos = playermovement.transform.position;
                navMeshAgent.enabled = true;
                float distance = Vector3.Distance(notSelectedPos, selectedPlayerPos);

                if (distance > 25)
                {
                    navMeshAgent.isStopped = false;
                    GroupUp(navMeshAgent, selectedPlayerPos);
                    iPlayer.Walking(true);
                    isGrouping = true;
                }
                else if (distance < 13 && isGrouping == true)
                {

                    navMeshAgent.isStopped = true;
                    iPlayer.Walking(false);
                    //playermovement.gameObject.GetComponent<CapsuleCollider>().enabled = true;
                }
            }
            else
            {
                navMeshAgent.enabled = false;
            }
        }
    }

    void GroupUp(NavMeshAgent navMeshAgent, Vector3 destination)
    {
        navMeshAgent.destination = destination;
        //player.GetComponent<CapsuleCollider>().enabled = false;
    }

    #endregion NAVMESH

    #region FMOD
    public void StartAudioFmod(GameObject gameObject, string pathname)
    {
        // EXAMPLE
        /*AE = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Extinguisher/EXT_Extinguishing");
        AE.start();*/
        EI = FMODUnity.RuntimeManager.CreateInstance(pathname);
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(EI, gameObject.transform, gameObject.GetComponent<Rigidbody>());
        EI.start();
        Debug.Log("Playing");
    }

    public void StopAudioFmod(GameObject gameObject)
    {
        EI.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        EI.release();
    }
    #endregion FMOD

    private void Update()
    {
        CheckCharDistance();
    }

    #region skill countdown timer
    public void SpawnTimer(GameObject currPlayer)
    {
        Vector3 playerPos = currPlayer.transform.position;
        GameObject timerPrefab = Instantiate(holdTimerPrefab, new Vector3(playerPos.x, playerPos.y + 4f, playerPos.z), Quaternion.identity);
        timerPrefab.transform.parent = currPlayer.transform;
    }

    public void RemoveTimer(GameObject currPlayer)
    {
        foreach (Transform transform in currPlayer.transform)
        {
            if (transform.tag == "HoldTimer")
            {
                Destroy(transform.gameObject);
                return;
            }
        }
    }

    #endregion
    //TESTING FMOD
    FMOD.Studio.EventInstance AE;

    //add any new obstacles' tag here 
    public void UseSkill()
    {
        if (!isPressed)
        {
            PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
            Animator animator = playerObject.GetComponent<Animator>(); ;
            IPlayer iPlayer = playerObject.GetComponent<IPlayer>();
            isPressed = !isPressed;

            if (playerMovement.target.tag != "Fire")
            {
                SpawnTimer(playerObject);
            }

            switch (playerMovement.target.tag)
            {
                case "Trap":
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterSecondadrySkill);
                    iPlayer.UsingSecondarySkill(true);
                    break;

                case "Oil Slick":
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterSecondadrySkill);
                    iPlayer.UsingMainSkill(true);
                    break;

                case "Player":
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterSecondadrySkill);
                    iPlayer.UsingMainSkill(true);
                    break;

                case "Button":
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterCommonSkill[0]);
                    break;

                //case "Door":
                //    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterCommonSkill[1]);
                //    break;

                case "Fire":
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterMainSkill);
                    iPlayer.UsingMainSkill(true);
                    break;

                case "Victim":
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterMainSkill);
                    if (!playerMovement.myPlayer.isCarryingVictim)
                    {
                        iPlayer.UsingMainSkill(true);
                    }
                    else
                    {
                        iPlayer.UsingMainSkill(false);
                    }
                    break;

                default:
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterMainSkill);
                    iPlayer.UsingMainSkill(true);
                    break;
            }
        }
    }

    #region when player let go of action button

    //when player let go of button during extinguishing
    public void LetGoExtinguish()
    {
        isPressed = !isPressed;
        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
        if (playerMovement.myPlayer.characterType != PublicEnumList.CharacterType.Extinguisher)
        {
            return;
        }

        Animator animator = playerObject.GetComponent<Animator>();
        IPlayer iPlayer = playerObject.GetComponent<IPlayer>();
        IFmod fmod = playerObject.GetComponent<IFmod>();

        actionBtn.gameObject.SetActive(false);
        playerMovement.myPlayer.isLookingAtFire = false;
        playerMovement.myPlayer.isExtinguishing = false;
        iPlayer.UsingMainSkill(false);
        fmod.StopAudioFmod(playerMovement.gameObject);

    }

    //check what is player looking at (only if the player cancel his action halfway)
    public void CheckTarget(PlayerMovement playerMovement, CheckCoroutine checkCoroutine)
    {
        IPlayer iPlayer = playerMovement.GetComponent<IPlayer>();

        Debug.Log("Checking Target");
        RemoveTimer(playerObject);
        playerMovement.myPlayer.characterCoroutine.isInCoroutine = false;
        playerMovement.StopCoroutine(checkCoroutine.currCoroutine);

        switch (playerMovement.myPlayer.characterCoroutine.type)
        {
            case PublicEnumList.CoroutineType.Main:
                iPlayer.UsingMainSkill(false);
                break;

            case PublicEnumList.CoroutineType.Secondary:
                iPlayer.UsingSecondarySkill(false);
                break;

            case PublicEnumList.CoroutineType.CarryingVictim:
                if (!playerMovement.myPlayer.isCarryingVictim)
                {
                    iPlayer.UsingMainSkill(false);
                }
                else
                {
                    iPlayer.UsingMainSkill(true);
                    iPlayer.UniqueAnimation("isCarryingVictim", true);
                }

                break;

            case PublicEnumList.CoroutineType.PressButton:
                //stop press button animaitong
                break;

            default:
                break;
        }



        //checking for future e.g fire, walls, citizen. Can use switch case
        //switch (playerMovement.target.tag)
        //{
        //    case "Wall":
        //        //DisableTimer();
        //        RemoveTimer(playerObject);
        //        playerMovement.myPlayer.characterCoroutine.isInCoroutine = false;
        //        playerMovement.StopCoroutine(checkCoroutine.currCoroutine);
        //        iPlayer.UsingMainSkill(false);
        //        break;

        //    default:
        //        //DisableTimer();
        //        break;
        //}
    }
    #endregion

    public void ChangeAbilityImage(Button actionBtn, PlayerMovement player)
    {
        foreach (AbilityHolder ah in characterAbility)
        {
            if (ah.character == player.myPlayer.characterType)
            {
                actionBtn.gameObject.SetActive(true);
                actionBtn.image.sprite = ah.image;
                return;
            }
        }
    }

  

    public void LevelSelection()
    {
        SceneManager.LoadScene(0);
    }



}
