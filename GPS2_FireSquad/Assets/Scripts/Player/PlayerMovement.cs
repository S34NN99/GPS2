using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CheckCoroutine
{
    public Coroutine currCoroutine;
    public bool isInCoroutine;
    public PublicEnumList.CoroutineType type;
}


[System.Serializable]
public class PlayerInfo
{
    public PublicEnumList.CharacterType characterType;
    public PublicEnumList.CharacterSkill characterMainSkill;
    public PublicEnumList.CharacterSkill characterSecondadrySkill;

    [SerializeField]
    public PublicEnumList.CharacterSkill[] characterCommonSkill =
        {
            PublicEnumList.CharacterSkill.Press,
            PublicEnumList.CharacterSkill.InteractDoor
        };

    public float moveSpeed;
    public float detectMaxRadius;
    public bool isExtinguishing = false;
    public bool isLookingAtFire = false;
    public bool isCarryingVictim = false;
    public bool isPressingButton = false;
    public bool isMoving = false;
    public bool isStunned = false;
    public bool isOnObstacle = false;

    public FMOD.Studio.EventInstance EI;
    public CheckCoroutine characterCoroutine;
}

public class PlayerMovement : MonoBehaviour, IPlayer, IFmod, IObjectives
{
    [Header("Player Information")]
    public PlayerInfo myPlayer;

    [Header("UI Element")]
    public Button actionBtn;
    private CharacterController controller;
    public Joystick joystick;

    private Animator animator;

    [Space(30)]
    private GameManager gameManager;
    private TaskManager taskManager;
    public bool playerSelected = false;
    float horizontalMove;
    float verticalMove;


    private void Start()
    {
        //textBtn = actionBtn.gameObject.transform.GetChild(0).GetComponent<Text>();
        controller = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
        actionBtn.gameObject.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
        taskManager = FindObjectOfType<TaskManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerSelected && !myPlayer.isStunned)
        {
            if (!myPlayer.isExtinguishing)
                myPlayer.isMoving = joystick.Horizontal > 0.1 ||
                    joystick.Vertical > 0.1 ||
                    joystick.Horizontal < -0.1 ||
                    joystick.Vertical < -0.1
                    ? myPlayer.isMoving = true : myPlayer.isMoving = false;

            if (myPlayer.characterCoroutine.isInCoroutine)
            {
                if (myPlayer.isMoving)
                {
                    StopAudioFmod(this.gameObject);
                    gameManager.CheckTarget(this, myPlayer.characterCoroutine);
                }
            }

            horizontalMove = joystick.Horizontal;
            verticalMove = joystick.Vertical;

            Vector3 direction = new Vector3(horizontalMove, 0f, verticalMove).normalized;
            if (myPlayer.isExtinguishing)
            {
                CheckFireInRadius(myPlayer);
                if (direction.magnitude >= 0.1f)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                }
                return;
            }

            if (direction.magnitude >= 0.1f)
            {
                Walking(true);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                controller.Move(direction * myPlayer.moveSpeed * Time.deltaTime);
            }
            else
            {
                Walking(false);
            }
        }

        //CheckInteractInRadius(this);
    }

    #region INTERFACES 
    public void Walking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void UsingMainSkill(bool isUsingSkill)
    {
        animator.SetBool("usingMainSkill", isUsingSkill);
    }

    public void UsingSecondarySkill(bool isUsingSkill)
    {
        animator.SetBool("usingSecondarySkill", isUsingSkill);
    }

    public void UniqueAnimation(string skillName, bool isUsingSkill)
    {
        animator.SetBool(skillName, isUsingSkill);
    }

    public void AddToObjective()
    {
        if (taskManager)
        {
            foreach (Objective obj in taskManager.ActiveObjectives)
            {
                if (obj.objectiveType == Objective.ObjectiveType.Stun)
                {
                    obj.objectiveValue++;
                    taskManager.LevelProgression();
                    taskManager.UpdateObjectiveList();
                    return;
                }
            }
        }
    }

    public void Stun(PlayerMovement playerMovement)
    {
        IPlayer iPlayer = playerMovement.GetComponent<IPlayer>();
        AddToObjective();
        Vector3 abovePlayer = new Vector3(playerMovement.transform.position.x, playerMovement.transform.position.y + 3, playerMovement.transform.position.z);
        GameObject stunIcon = Instantiate(gameManager.stunPrefab, abovePlayer, Quaternion.identity);
        stunIcon.transform.parent = playerMovement.gameObject.transform;

        playerMovement.myPlayer.isStunned = true;
        iPlayer.Walking(false);

        int stunCount = 0;
        GameObject parent = this.transform.parent.gameObject;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).GetComponent<PlayerMovement>().myPlayer.isStunned) stunCount++;
        }

        if(stunCount >= 3)
        {
            taskManager.summaryManager.SummaryDisplay();
        }
    }

    public void UnStun(PlayerMovement playerMovement)
    {
        foreach (Transform transform in playerMovement.transform)
        {
            if (transform.tag == "Stun")
            {
                Destroy(transform.gameObject);
                playerMovement.myPlayer.isStunned = false;
                return;
            }
        }
    }

    public void SpawnFire(PlayerMovement playerMovement, GameObject firePrefab)
    {
        GameObject fire = Instantiate(gameManager.firePrefab, playerMovement.transform);
        fire.GetComponent<Fire>().fireInfo.spawnOnPlayer = true;
        fire.transform.position = playerMovement.transform.position;
    }

    public void StartAudioFmod(GameObject gameObject, string pathname)
    {
        // EXAMPLE
        /*AE = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Extinguisher/EXT_Extinguishing");
        AE.start();*/
        PlayerMovement playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerMovement.myPlayer.EI = FMODUnity.RuntimeManager.CreateInstance(pathname);
        playerMovement.myPlayer.EI.start();
    }

    public void StopAudioFmod(GameObject gameObject)
    {
        PlayerMovement playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerMovement.myPlayer.EI.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        playerMovement.myPlayer.EI.release();
    }


    #endregion INTERFACES

    #region Detection
    //////////Fire Detection
    /////////Wall Detection
    public GameObject target;

    private void FixedUpdate()
    {
        CheckTarget(myPlayer);
        //CheckInteractInRadius(myPlayer);
    }

    //check what is the player looking at
    void CheckTarget(PlayerInfo myplayer)
    {
        RaycastHit hit;
        if (playerSelected && !myplayer.isOnObstacle)
        {
            if (!myplayer.isLookingAtFire)
            {
                if(myplayer.isCarryingVictim)
                {
                    gameManager.ChangeAbilityImage(actionBtn, this, false);
                    return;
                }

                Vector3 rayCastPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

                if (Physics.Raycast(rayCastPos, transform.forward, out hit, myPlayer.detectMaxRadius))
                {
                    if (hit.collider.gameObject.CompareTag("Button"))
                    {
                        gameManager.ChangeAbilityImage(actionBtn, this, true);
                        target = hit.collider.gameObject;
                        return;
                    }
                }
                else
                {
                    actionBtn.gameObject.SetActive(false);
                    return;
                }

                switch (myPlayer.characterType)
                {
                    case PublicEnumList.CharacterType.Extinguisher:
                        if (Physics.Raycast(rayCastPos, transform.forward, out hit, myplayer.detectMaxRadius))
                        {
                            if (hit.collider.gameObject.CompareTag("Fire"))
                            {
                                gameManager.ChangeAbilityImage(actionBtn, this, false);
                                target = hit.collider.gameObject;
                            }
                        }
                        else
                        {
                            actionBtn.gameObject.SetActive(false);
                        }
                        break;

                    case PublicEnumList.CharacterType.Demolisher:
                        if (Physics.Raycast(rayCastPos, transform.forward, out hit, myplayer.detectMaxRadius))
                        {
                            if (hit.collider.gameObject.CompareTag("Wall"))
                            {
                                gameManager.ChangeAbilityImage(actionBtn, this, false);
                                target = hit.collider.gameObject;
                            }
                        }
                        else
                        {
                            actionBtn.gameObject.SetActive(false);
                        }
                        break;

                    case PublicEnumList.CharacterType.Medic:
                        if (myplayer.isCarryingVictim && !myplayer.isStunned)
                        {
                            gameManager.ChangeAbilityImage(actionBtn, this, false);
                            return;
                        }

                        if (Physics.Raycast(rayCastPos, transform.forward, out hit, myplayer.detectMaxRadius))
                        {
                            if (hit.collider.gameObject.CompareTag("Victim"))
                            {
                                gameManager.ChangeAbilityImage(actionBtn, this, false);
                                target = hit.collider.gameObject;
                            }

                            if (hit.collider.gameObject.CompareTag("Player"))
                            {
                                PlayerMovement targetPlayer = hit.collider.gameObject.GetComponent<PlayerMovement>();
                                if (targetPlayer.myPlayer.isStunned)
                                {
                                    gameManager.ChangeAbilityImage(actionBtn, this, false);
                                    target = hit.collider.gameObject;
                                }
                            }
                        }
                        else
                        {
                            actionBtn.gameObject.SetActive(false);
                        }

                        break;

                    default:
                        break;
                }
            }
        }
    }

    void CheckFireInRadius(PlayerInfo myPlayer)
    {
        RaycastHit hit;
        Vector3 rayCastPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        if (Physics.Raycast(rayCastPos, transform.forward, out hit, myPlayer.detectMaxRadius) && hit.collider.gameObject.CompareTag("Fire"))
        {
            Fire fire = hit.collider.gameObject.GetComponent<Fire>();
            Walking(false);
            gameManager.ChangeAbilityImage(actionBtn, this, false);
            target = hit.collider.gameObject;
            fire.LoseHealth();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Oil Slick") && myPlayer.characterType == PublicEnumList.CharacterType.Medic)
        {
            target = collision.gameObject;
            myPlayer.isOnObstacle = true;
            gameManager.ChangeAbilityImage(actionBtn, this, false);
        }

        if (collision.gameObject.CompareTag("Trap") && myPlayer.characterType == PublicEnumList.CharacterType.Demolisher) // demolisher secondary skill 
        {
            target = collision.gameObject;
            myPlayer.isOnObstacle = true;
            gameManager.ChangeAbilityImage(actionBtn, this, false);
        }
    }

    private void OnCollisionExit(Collision collision) // only for obstacles on the floor/ cannot be detected by raycast
    {
        if (collision.gameObject.CompareTag("Trap")) // demolisher secondary skill 
        {
            myPlayer.isOnObstacle = false;
            actionBtn.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Oil Slick")) // Medic secondary skill 
        {
            myPlayer.isOnObstacle = false;
            actionBtn.gameObject.SetActive(false);
        }
    }

    #endregion

    #region player skills
    //skills 
    //every skill must have (to remove timer and set coroutine to false)
    //gameManager.RemoveTimer(this.gameObject);
    //SetCoroutine(currPlayer, false);

    public void PlayerSkills(PlayerMovement currPlayer, PublicEnumList.CharacterSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PublicEnumList.CharacterSkill.Extinguish:
                currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(ExtinguishFire(currPlayer, target));
                currPlayer.myPlayer.characterCoroutine.type = PublicEnumList.CoroutineType.Main;
                StartAudioFmod(currPlayer.gameObject, "event:/SFX/Extinguisher/ExtinguishingSFX");
                break;

            case PublicEnumList.CharacterSkill.Break:
                currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(BreakWall(currPlayer, target));
                currPlayer.myPlayer.characterCoroutine.type = PublicEnumList.CoroutineType.Main;
                StartAudioFmod(currPlayer.gameObject, "event:/SFX/Demolisher/BreakWallSFX");
                break;

            case PublicEnumList.CharacterSkill.DemolishTrap:
                currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(BreakTrap(currPlayer, target));
                currPlayer.myPlayer.characterCoroutine.type = PublicEnumList.CoroutineType.Secondary;
                StartAudioFmod(currPlayer.gameObject, "event:/SFX/Demolisher/BreakWallSFX");
                break;

            case PublicEnumList.CharacterSkill.Heal:
                currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(SaveTeamate(currPlayer, target));
                currPlayer.myPlayer.characterCoroutine.type = PublicEnumList.CoroutineType.Main;
                StartAudioFmod(currPlayer.gameObject, "event:/SFX/Medic/RescueSFX");
                break;

            case PublicEnumList.CharacterSkill.Carry:
                currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(CarryVictim(currPlayer, target));
                currPlayer.myPlayer.characterCoroutine.type = PublicEnumList.CoroutineType.CarryingVictim;
                StartAudioFmod(currPlayer.gameObject, "event:/SFX/Medic/RescueSFX");
                break;

            case PublicEnumList.CharacterSkill.Press:
                currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(PressButton(currPlayer, target));
                currPlayer.myPlayer.characterCoroutine.type = PublicEnumList.CoroutineType.PressButton;

                break;

            //case PublicEnumList.CharacterSkill.InteractDoor:
            //    currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(InteractDoor(currPlayer, target));
            //    break;

            default:

                break;
        }
    }

    //enable/disable player coroutine
    public void SetCoroutine(PlayerMovement currPlayer, bool temp)
    {
        currPlayer.myPlayer.characterCoroutine.isInCoroutine = temp;
    }

    //to stop the actions of a character
    public void StopCoroutine(CheckCoroutine checkCoroutine)
    {
        StopCoroutine(checkCoroutine.currCoroutine);
    }

    //extinguish fire
    IEnumerator ExtinguishFire(PlayerMovement currPlayer, GameObject target)
    {
        SetCoroutine(currPlayer, true);
        currPlayer.myPlayer.isExtinguishing = true;
        currPlayer.myPlayer.isLookingAtFire = true;
        SetCoroutine(currPlayer, false);
        yield return null;
    }


    //break wall
    IEnumerator BreakWall(PlayerMovement currPlayer, GameObject target)
    {
        SetCoroutine(currPlayer, true);

        Wall wall = target.GetComponent<Wall>();

        yield return new WaitForSeconds(1.0f);
        wall.Damage(currPlayer);
        //target.GetComponent<Renderer>().material.color = Color.yellow;

        yield return new WaitForSeconds(1.0f);
        wall.Damage(currPlayer);
        //target.GetComponent<Renderer>().material.color = Color.red;

        yield return new WaitForSeconds(1.3f);
        wall.Damage(currPlayer);

        //StopAudioFmod(currPlayer.gameObject);
        //UsingMainSkill(false);
        //gameManager.RemoveTimer(this.gameObject);
        //SetCoroutine(currPlayer, false);
    }


    //break trap and save teammates
    IEnumerator BreakTrap(PlayerMovement currPlayer, GameObject target)
    {
        // have to get Trap's list of trapped player and free them
        Trap trap = target.GetComponent<Trap>();
        SetCoroutine(currPlayer, true);

        yield return new WaitForSeconds(3.3f);
        if (trap.trappedPlayer.Count > 0)
        {
            for (int i = 0; i < trap.trappedPlayer.Count; i++)
            {
                PlayerMovement playerMovement = trap.trappedPlayer[i].GetComponent<PlayerMovement>();
                IPlayer iPlayer = playerMovement.GetComponent<IPlayer>();
                iPlayer.UniqueAnimation("Trap", false);
                UnStun(playerMovement);
            }
        }

        StopAudioFmod(currPlayer.gameObject);
        Destroy(target);
        if (taskManager)
        {
            trap.AddToObjective();
        }
        UsingSecondarySkill(false);
        myPlayer.isOnObstacle = false;
        //actionBtn.gameObject.SetActive(false);
        gameManager.RemoveTimer(this.gameObject);
        SetCoroutine(currPlayer, false);
    }

    //save teamate from oil slick
    IEnumerator SaveTeamate(PlayerMovement currPlayer, GameObject target)
    {
        string tag = target.tag;
        SetCoroutine(currPlayer, true);
        OilSlick oil = target.GetComponent<OilSlick>();

        yield return new WaitForSeconds(3.0f);
        if (tag == "Player")
        {
            UnStun(target.GetComponent<PlayerMovement>());
            target.GetComponent<IPlayer>().UniqueAnimation("Slip", false);
        }
        else
        {
            oil.AddToObjective();
            Destroy(target);
        }

        myPlayer.isOnObstacle = false;
        //actionBtn.gameObject.SetActive(false);
        gameManager.RemoveTimer(this.gameObject);
        UsingMainSkill(false);
        SetCoroutine(currPlayer, false);
    }

    //carry victim
    IEnumerator CarryVictim(PlayerMovement currPlayer, GameObject target)
    {
        string tag = target.tag;
        SetCoroutine(currPlayer, true);
        Transform carryingPosition = currPlayer.transform.GetChild(2);
        Transform placePosition = currPlayer.transform.GetChild(3);

        if (tag == "Victim" && currPlayer.myPlayer.isCarryingVictim == false)
        {
            yield return new WaitForSeconds(3.0f);
            target.GetComponent<Rigidbody>().useGravity = false;
            target.GetComponent<BoxCollider>().enabled = false;

            target.transform.parent.position = carryingPosition.position;
            target.transform.parent.rotation = carryingPosition.rotation;
            target.transform.parent.parent = carryingPosition.transform;
            currPlayer.myPlayer.isCarryingVictim = true;

            UniqueAnimation("isCarryingVictim", true);
            target.GetComponent<VictimHealth>().CheckCarrying(true);
        }
        else if (currPlayer.myPlayer.isCarryingVictim)
        {
            UniqueAnimation("isCarryingVictim", false);
            yield return new WaitForSeconds(3.0f);
            target.GetComponent<Rigidbody>().useGravity = true;
            target.GetComponent<BoxCollider>().enabled = true;

            target.transform.parent.position = placePosition.position;
            target.transform.parent.rotation = placePosition.rotation;
            target.transform.parent.parent = null;

            currPlayer.myPlayer.isCarryingVictim = false;

            target.GetComponent<VictimHealth>().CheckCarrying(false);
        }

        StopAudioFmod(currPlayer.gameObject);
        gameManager.RemoveTimer(this.gameObject);
        SetCoroutine(currPlayer, false);
    }

    IEnumerator PressButton(PlayerMovement currPlayer, GameObject target)
    {
        SetCoroutine(currPlayer, true);
        CoopDoorButton btn = target.GetComponent<CoopDoorButton>();
        btn.ButtonPressed();
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PressButtonSFX");
        yield return new WaitForSeconds(3.0f);

        gameManager.RemoveTimer(this.gameObject);
    }
    #endregion

}
