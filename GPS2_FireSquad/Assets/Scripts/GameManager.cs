using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public CameraMovement cameraMovement;

    private GameObject[] playerGroup => GameObject.FindGameObjectsWithTag("Player");

    public GameObject playerObject;
    public Button actionBtn;
    [Header("Prefabs")]
    public GameObject stunPrefab;
    public GameObject holdTimerPrefab;
    public GameObject firePrefab;

    public bool isPressed = false;
    float maxCountDown = 2.0f;
    float currCountDown;
    public Image timer;

    void CheckCharDistance()
    {
        Vector3 selectedPlayerPos = playerObject.transform.position;

        for(int i = 0; i < playerGroup.Length; i++)
        {
            PlayerMovement playermovement = playerGroup[i].GetComponent<PlayerMovement>();
            NavMeshAgent navMeshAgent = playermovement.GetComponent<NavMeshAgent>();
            if (!playermovement.playerSelected)
            {
                IAnimation iAnimation = navMeshAgent.gameObject.GetComponent<IAnimation>();
                Vector3 notSelectedPos = playermovement.transform.position;
                navMeshAgent.enabled = true;
                float distance = Vector3.Distance(notSelectedPos, selectedPlayerPos);

                if(distance > 20)
                {
                    navMeshAgent.isStopped = false;
                    GroupUp(navMeshAgent, selectedPlayerPos);
                    iAnimation.Walking(true);
                }
                else if(distance < 10)
                {
                    navMeshAgent.isStopped = true;
                    iAnimation.Walking(false);
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
    }

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

    //add any new obstacles' tag here 
    public void UseSkill()
    {
        if (!isPressed)
        {
            PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
            Animator animator = playerObject.GetComponent<Animator>(); ;
            IAnimation iAnimation = playerObject.GetComponent<IAnimation>();

            isPressed = !isPressed;

            if (playerMovement.target.tag != "Fire")
            {
                SpawnTimer(playerObject);
            }

            switch (playerMovement.target.tag)
            {
                case "Trap":
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterSecondadrySkill);
                    iAnimation.UsingSecondarySkill(true);
                    break;

                case "Oil Slick":
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterSecondadrySkill);
                    iAnimation.UsingSecondarySkill(true);
                    break;

                case "Player":
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterSecondadrySkill);
                    iAnimation.UsingSecondarySkill(true);
                    break;

                case "Button":
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterCommonSkill[0]);
                    break;

                case "Door":
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterCommonSkill[1]);
                    break;

                /*case "Victim":              FOR VICTIM AND CHECK IF PLAYER IS CARRINY VICTIM OR NOT TO SET THE CORRECT ANIMATION
                    break; */

                default:
                    playerMovement.PlayerSkills(playerMovement, playerMovement.myPlayer.characterMainSkill);
                    iAnimation.UsingMainSkill(true);
                    break;
            }
        }
    }

    #region when player let go of action button
    private void CheckFire(Fire fire)
    {
        if (fire.fireInfo.currentHealth < 3)
        {
            StartCoroutine(fire.Reignite(fire));
        }
    }

    //when player let go of button during extinguishing
    public void LetGoExtinguish()
    {
        isPressed = !isPressed;
        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>(); ;
        Animator animator = playerObject.GetComponent<Animator>(); ;
        IAnimation iAnimation = playerObject.GetComponent<IAnimation>();

        if (playerMovement.myPlayer.characterType == PublicEnumList.CharacterType.Extinguisher)
        {
            Debug.Log("success");
            actionBtn.gameObject.SetActive(false);
            playerMovement.myPlayer.isLookingAtFire = false;
            playerMovement.myPlayer.isExtinguishing = false;
            iAnimation.UsingMainSkill(false);
        }
    }

    //check what is player looking at (only if the player cancel his action halfway)
    public void CheckTarget(PlayerMovement playerMovement, CheckCoroutine checkCoroutine)
    {
        IAnimation iAnimation = playerMovement.GetComponent<IAnimation>();

        //checking for future e.g fire, walls, citizen. Can use switch case
        switch (playerMovement.target.tag)
        {
            case "Fire":
                //DisableTimer();
                //RemoveTimer(playerObject);
                //playerMovement.StopCoroutine(checkCoroutine.currCoroutine);
                //playerMovement.myPlayer.characterCoroutine.isInCoroutine = false;
                //CheckFire(playerMovement.target.GetComponent<Fire>());
                break;

            case "Wall":
                //DisableTimer();
                RemoveTimer(playerObject);
                playerMovement.myPlayer.characterCoroutine.isInCoroutine = false;
                playerMovement.StopCoroutine(checkCoroutine.currCoroutine);
                iAnimation.UsingMainSkill(false);
                break;

            default:
                //DisableTimer();
                RemoveTimer(playerObject);
                playerMovement.myPlayer.characterCoroutine.isInCoroutine = false;
                playerMovement.StopCoroutine(checkCoroutine.currCoroutine);
                break;
        }
    }
    #endregion

    public void CheckIfPlayerStun(PlayerMovement playerMovement) // to instatiate stun icon on top of player
    {
        if(playerMovement.myPlayer.isStunned)
        {
            IAnimation iAnimation = playerMovement.GetComponent<IAnimation>();
            Vector3 abovePlayer = new Vector3(playerMovement.transform.position.x, playerMovement.transform.position.y + 3, playerMovement.transform.position.z);
            GameObject stunIcon = Instantiate(stunPrefab, abovePlayer, Quaternion.identity);
            stunIcon.transform.parent = playerMovement.gameObject.transform;

            iAnimation.Walking(false);
        }
    }



}
