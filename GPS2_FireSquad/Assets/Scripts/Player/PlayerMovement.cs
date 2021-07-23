using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CheckCoroutine
{
    public Coroutine currCoroutine;
    public bool isInCoroutine;
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

    public CheckCoroutine characterCoroutine;
}

public class PlayerMovement : MonoBehaviour, IAnimation, IPlayer
{
    [Header("Player Information")]
    public PlayerInfo myPlayer;

    [Header("UI Element")]
    public Button actionBtn;
    private Text textBtn;
    private CharacterController controller;
    public Joystick joystick;

    public Animator animator;

    [Space(30)]
    private GameManager gameManager;
    public bool playerSelected = false;
    float horizontalMove;
    float verticalMove;

    private void Start()
    {
        textBtn = actionBtn.gameObject.transform.GetChild(0).GetComponent<Text>();
        controller = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
        actionBtn.gameObject.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSelected && !myPlayer.isStunned)
        {
            if(!myPlayer.isExtinguishing)
            myPlayer.isMoving = joystick.Horizontal > 0.1 || 
                joystick.Vertical > 0.1  ||
                joystick.Horizontal < -0.1 ||
                joystick.Vertical < -0.1
                ? myPlayer.isMoving = true : myPlayer.isMoving = false;

            if (myPlayer.characterCoroutine.isInCoroutine)
            {
                if (myPlayer.isMoving)
                {
                    gameManager.CheckTarget(this, myPlayer.characterCoroutine);
                    //StopCoroutine(myPlayer.characterCoroutine.currCoroutine);
                }
            }

            horizontalMove = joystick.Horizontal;
            verticalMove = joystick.Vertical;

            Vector3 direction = new Vector3(horizontalMove, 0f, verticalMove).normalized;

            if (direction.magnitude >= 0.1f)
            {
                if (!myPlayer.isExtinguishing && !myPlayer.isCarryingVictim)
                {
                    Walking(true);
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                    controller.Move(direction * myPlayer.moveSpeed * Time.deltaTime);

                }
                else if(myPlayer.isCarryingVictim)
                {
                    Walking(false);
                    animator.SetBool("isCarryingVictim", true);
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                    controller.Move(direction * myPlayer.moveSpeed * Time.deltaTime);
                }
                else
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                    CheckFireInRadius(myPlayer);
                }
            }
            else
            {
                if (myPlayer.characterType == PublicEnumList.CharacterType.Medic)
                {
                    animator.SetBool("isCarryingVictim", false);
                }
                Walking(false);
            }
        }
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

    public void UsingUniqueSkill(string skillName, bool isUsingSkill)
    {
        animator.SetBool(skillName, isUsingSkill);
    }

    public void Stun(PlayerMovement playerMovement)
    {
        IAnimation iAnimation = playerMovement.GetComponent<IAnimation>();
        Vector3 abovePlayer = new Vector3(playerMovement.transform.position.x, playerMovement.transform.position.y + 3, playerMovement.transform.position.z);
        GameObject stunIcon = Instantiate(gameManager.stunPrefab, abovePlayer, Quaternion.identity);
        stunIcon.transform.parent = playerMovement.gameObject.transform;

        playerMovement.myPlayer.isStunned = true;
        iAnimation.Walking(false);
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

    }

    public void RemoveFire(PlayerMovement playerMovement)
    {
        foreach (Transform transform in playerMovement.transform)
        {
            if (transform.tag == "Fire")
            {
                Destroy(transform.gameObject);
                return;
            }
        }
    }

    #endregion


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
                Vector3 rayCastPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                switch (myPlayer.characterType)
                {
                    case PublicEnumList.CharacterType.Extinguisher:
                        if (Physics.Raycast(rayCastPos, transform.forward, out hit, myplayer.detectMaxRadius))
                        {
                            if (hit.collider.gameObject.CompareTag("Fire"))
                            {
                                actionBtn.gameObject.SetActive(true);
                                textBtn.text = myplayer.characterMainSkill.ToString();
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
                                actionBtn.gameObject.SetActive(true);
                                textBtn.text = myplayer.characterMainSkill.ToString();
                                target = hit.collider.gameObject;
                            }
                        }
                        else
                        {
                            actionBtn.gameObject.SetActive(false);
                        }
                        break;

                    case PublicEnumList.CharacterType.Medic:
                        if (Physics.Raycast(rayCastPos, transform.forward, out hit, myplayer.detectMaxRadius))
                        {
                            if(hit.collider.gameObject.CompareTag("Victim"))
                            {
                                actionBtn.gameObject.SetActive(true);
                                textBtn.text = myplayer.characterMainSkill.ToString();
                                target = hit.collider.gameObject;
                            }

                            if (hit.collider.gameObject.CompareTag("Player"))
                            {
                                PlayerMovement targetPlayer = hit.collider.gameObject.GetComponent<PlayerMovement>();
                                if (targetPlayer.myPlayer.isStunned)
                                {
                                    actionBtn.gameObject.SetActive(true);
                                    textBtn.text = myplayer.characterSecondadrySkill.ToString();
                                    target = hit.collider.gameObject;
                                }
                            }
                        }
                        else
                        {
                            if (myplayer.isCarryingVictim)
                            {
                                actionBtn.gameObject.SetActive(true);
                            }
                            else
                            {
                                actionBtn.gameObject.SetActive(false);
                            }
                        }
                       
                        break;

                    default:
                        Debug.Log("none found");
                        break;
                }
            }
        }
    }

    void CheckFireInRadius(PlayerInfo myPlayer)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, myPlayer.detectMaxRadius) && hit.collider.gameObject.CompareTag("Fire"))
        {
            Fire fire = hit.collider.gameObject.GetComponent<Fire>();
            
            actionBtn.gameObject.SetActive(true);
            textBtn.text = myPlayer.characterMainSkill.ToString();
            target = hit.collider.gameObject;

            fire.fireInfo.isDamaged = true;
        }
    }

    private void CheckInteractInRadius(PlayerInfo myPlayer)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, myPlayer.detectMaxRadius) && hit.collider.gameObject.CompareTag("Interact"))
        {
            actionBtn.gameObject.SetActive(true);

            if (hit.collider.gameObject.CompareTag("Button"))
            {
                textBtn.text = myPlayer.characterCommonSkill[0].ToString();
            }
            else if (hit.collider.gameObject.GetComponent<Door>().isLocked == false)
            {
                textBtn.text = myPlayer.characterCommonSkill[1].ToString();
            }
            else if (hit.collider.gameObject.GetComponent<Door>().isLocked == true)
            {
                textBtn.text = "Locked";
            }

            target = hit.collider.gameObject;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Oil Slick") && myPlayer.characterType == PublicEnumList.CharacterType.Medic)
        {
            Debug.Log("Dected oil");
            target = collision.gameObject;
            myPlayer.isOnObstacle = true;
            actionBtn.gameObject.SetActive(true);
            textBtn.text = myPlayer.characterSecondadrySkill.ToString();
        }

        if (collision.gameObject.CompareTag("Trap") && myPlayer.characterType == PublicEnumList.CharacterType.Demolisher) // demolisher secondary skill 
        {
            Debug.Log("Dected Trap");
            target = collision.gameObject;
            myPlayer.isOnObstacle = true;
            actionBtn.gameObject.SetActive(true);
            textBtn.text = myPlayer.characterSecondadrySkill.ToString();
        }
    }

    private void OnCollisionExit(Collision collision) // only for obstacles on the floor/ cannot be detected by raycast
    {
        if (collision.gameObject.CompareTag("Trap")) // demolisher secondary skill 
        {
            Debug.Log("exit Trap");
            myPlayer.isOnObstacle = false;
            actionBtn.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Oil Slick")) // demolisher secondary skill 
        {
            Debug.Log("exit oil");
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
        Debug.Log("Used " + playerSkill.ToString());
            switch (playerSkill)
            {
                case PublicEnumList.CharacterSkill.Extinguish:
                    currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(ExtinguishFire(currPlayer, target));
                    break;

                case PublicEnumList.CharacterSkill.Break:
                currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(BreakWall(currPlayer, target));
                    break;

                case PublicEnumList.CharacterSkill.DemolishTrap:
                    currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(BreakTrap(currPlayer, target));
                    break;

                case PublicEnumList.CharacterSkill.Heal:
                    currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(SaveTeamate(currPlayer, target));
                        break;

                case PublicEnumList.CharacterSkill.Carry:
                    currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(CarryVictim(currPlayer, target));
                    break;

                case PublicEnumList.CharacterSkill.Press:
                    currPlayer.myPlayer.characterCoroutine.currCoroutine = StartCoroutine(PressButton(currPlayer, target));
                    break;

            default:
                    Debug.Log("Could not find skill");
                    break;
            }
    }

    //void RemoveFire(GameObject target)
    //{
    //    foreach (Transform transform in target.transform)
    //    {
    //        if (transform.tag == "Fire")
    //        {
    //            Destroy(transform.gameObject);
    //            return;
    //        }
    //    }
    //}

    //enable/disable player coroutine
    void SetCoroutine(PlayerMovement currPlayer, bool temp)
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
        yield return new WaitForSeconds(0f);
        //gameManager.RemoveTimer(this.gameObject);
        //animator.SetBool("usingMainSkill", false);
    }

    public GameObject wall2Hp;
    public GameObject wall1Hp;
    Mesh wallMesh;
    Mesh wall2HpMesh;
    Mesh wall1HpMesh;

    //break wall
    IEnumerator BreakWall(PlayerMovement currPlayer, GameObject target)
    {
        SetCoroutine(currPlayer, true);

        //change sprites here
        //wallMesh = target.GetComponent<MeshFilter>().sharedMesh;
        //wall2HpMesh = wall2Hp.GetComponent<MeshFilter>().sharedMesh;
        //wall1HpMesh = wall1Hp.GetComponent<MeshFilter>().sharedMesh;

        yield return new WaitForSeconds(1.0f);
        target.GetComponent<Renderer>().material.color = Color.yellow;
        //target.GetComponent<MeshFilter>().sharedMesh = wall2HpMesh;

        yield return new WaitForSeconds(1.0f);
        target.GetComponent<Renderer>().material.color = Color.red;
        //target.GetComponent<MeshFilter>().sharedMesh = wall1HpMesh;

        yield return new WaitForSeconds(1.0f);
        Destroy(target);

        UsingMainSkill(false);
        gameManager.RemoveTimer(this.gameObject);
        SetCoroutine(currPlayer, false);
    }


    //break trap and save teammates
    IEnumerator BreakTrap(PlayerMovement currPlayer, GameObject target)
    {
        // have to get Trap's list of trapped player and free them
        Trap trap = target.GetComponent<Trap>();


        yield return new WaitForSeconds(3.0f);
        if (trap.trappedPlayer.Count > 0)
        {
            for (int i = 0; i < trap.trappedPlayer.Count; i++)
            {
                PlayerMovement playerMovement = trap.trappedPlayer[i].GetComponent<PlayerMovement>();
                UnStun(playerMovement);
            }
        }
        Destroy(target);
        gameManager.RemoveTimer(this.gameObject);
        SetCoroutine(currPlayer, false);
    }

    //save teamate from oil slick
    IEnumerator SaveTeamate(PlayerMovement currPlayer, GameObject target)
    {
        string tag = target.tag;

        yield return new WaitForSeconds(3.0f);
        if (tag == "Player")
        {
            UnStun(target.GetComponent<PlayerMovement>());
        }
        else
        {
            Debug.Log("Target is oil");
            Destroy(target);
        }

        gameManager.RemoveTimer(this.gameObject);
        SetCoroutine(currPlayer, false);
    }

    //carry victim
    IEnumerator CarryVictim(PlayerMovement currPlayer, GameObject target)
    {
        string tag = target.tag;

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

            target.GetComponent<VictimHealth>().CheckCarrying(true);
            Debug.Log("Victim is picked up");
        }
        else if(currPlayer.myPlayer.isCarryingVictim)
        {
            yield return new WaitForSeconds(3.0f);
            target.GetComponent<Rigidbody>().useGravity = true;
            target.GetComponent<BoxCollider>().enabled = true;

            target.transform.parent.position = placePosition.position;
            target.transform.parent.rotation = placePosition.rotation;
            target.transform.parent.parent = null;

            currPlayer.myPlayer.isCarryingVictim = false;

            target.GetComponent<VictimHealth>().CheckCarrying(false);
            Debug.Log("Victim is dropped");
        }

        UsingMainSkill(false);
        gameManager.RemoveTimer(this.gameObject);
        SetCoroutine(currPlayer, false);
    }

    IEnumerator PressButton(PlayerMovement currPlayer, GameObject target)
    {
        string tag = target.tag;

        yield return new WaitForSeconds(3.0f);

        if (tag == "Button" && currPlayer.myPlayer.isPressingButton == false)
        {
            Debug.Log("Target is Button");
            //target;
            currPlayer.myPlayer.isPressingButton = true;
        }

        gameManager.RemoveTimer(this.gameObject);
        SetCoroutine(currPlayer, false);
    }

    #endregion

}
