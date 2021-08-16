using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireInfo
{
    public int maxHealth = 4;
    public int currentHealth;
    public bool isImmunity = false;
    public float immunityDuration = 1.0f;

    public bool spawnOnPlayer = false;
    public FMOD.Studio.EventInstance EI;
    public Coroutine reignite;
}

public class Fire : MonoBehaviour, IFmod
{
    public FireInfo fireInfo;
    public GameObject healthPrefab;

    private bool reigniting = false;
    private GameManager gameManager;

    private float small = 0.3f, medium = 0.5f, large = 1.0f, normal = 1.5f;

    private void Start()
    {
        fireInfo.currentHealth = fireInfo.maxHealth;
        gameManager = FindObjectOfType<GameManager>();
        //StartAudioFmod(this.gameObject, "event:/BGM/bgm");
    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement target = collision.gameObject.GetComponent<PlayerMovement>();
            IPlayer iPlayer = collision.gameObject.GetComponent<IPlayer>();

            if (target.myPlayer.characterType != PublicEnumList.CharacterType.Extinguisher)
            {
                if (!target.myPlayer.isStunned && target.playerSelected)
                {

                    target.Stun(target);
                    //spawned fire on player
                    target.SpawnFire(target, gameManager.firePrefab);
                    iPlayer.UniqueAnimation("Burn", true);
                    iPlayer.Walking(false);
                    Debug.Log("On fire");
                }
                else
                {
                    return;
                }
            }
        }
    }

    #region FMOD 
    public void StartAudioFmod(GameObject gameObject, string pathname)
    {
        // EXAMPLE
        /*AE = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Extinguisher/EXT_Extinguishing");
        AE.start();*/
        FireInfo fireinfo = gameObject.GetComponent<Fire>().fireInfo;
        fireinfo.EI = FMODUnity.RuntimeManager.CreateInstance(pathname);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(fireInfo.EI, this.transform, this.GetComponent<Rigidbody>());
        fireinfo.EI.start();
        Debug.Log("Playing");
    }

    public void StopAudioFmod(GameObject gameObject)
    {
        FireInfo fireinfo = gameObject.GetComponent<Fire>().fireInfo;
        fireinfo.EI.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        fireinfo.EI.release();
    }

    #endregion FMOD

    public void LoseHealth()
    {
        if (!fireInfo.isImmunity)
        {
            fireInfo.currentHealth--;
            fireInfo.isImmunity = true;
            StartCoroutine(FireImmunity());

            if (reigniting)
            {
                StopCoroutine(fireInfo.reignite);
                fireInfo.reignite = StartCoroutine(Reignite(this));
            }
            else
            {
                fireInfo.reignite = StartCoroutine(Reignite(this));
            }

            if (fireInfo.currentHealth <= 0)
            {
                bool temp = fireInfo.spawnOnPlayer ? temp = true : temp = false;
                Death(temp);
            }
                  
            if (transform.Find("HealthIcon(Clone)"))
            {
                GameObject tempHealth = transform.Find("HealthIcon(Clone)").gameObject;
                UpdateHealth(fireInfo, tempHealth);
            }
            else
            {
                Vector3 aboveChar = new Vector3(transform.position.x, 3.5f, transform.position.z);
                GameObject tempHealth = Instantiate(healthPrefab, aboveChar, Quaternion.identity);
                tempHealth.transform.parent = this.transform;
                UpdateHealth(fireInfo, tempHealth);
            }
        }
    }

    public void Death(bool temp)
    {
        if (temp)
        {
            PlayerMovement player = transform.parent.GetComponent<PlayerMovement>();
            IPlayer iPlayer = player.gameObject.GetComponent<IPlayer>();
            player.UnStun(player);
            iPlayer.UniqueAnimation("Burn", false);
        }
        Destroy(this.gameObject);
    }

    void UpdateHealth(FireInfo fireinfo, GameObject healthIcon)
    {
        ParticleSystem firePs = this.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainPs = firePs.main;

        switch (fireinfo.currentHealth)
        {
            case 1:
                healthIcon.GetComponent<Renderer>().material.color = Color.red;
                mainPs.startLifetime = small;
                break;
            
            case 2:
                healthIcon.GetComponent<Renderer>().material.color = Color.yellow;
                mainPs.startLifetime = medium;
                break;  
            
            case 3:
                healthIcon.GetComponent<Renderer>().material.color = Color.green;
                mainPs.startLifetime = large;
                break;

            case 4:
                Destroy(healthIcon);
                break;  

            default:
                Debug.Log("Something is wrong");
                break;
        }    
    }

    IEnumerator FireImmunity()
    {
        yield return new WaitForSeconds(fireInfo.immunityDuration);
        fireInfo.isImmunity = false;
    }

    public IEnumerator Reignite(Fire target)
    {
        reigniting = true;
        ParticleSystem firePS = target.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainPS = firePS.main;

        yield return new WaitForSeconds(4.0f);
        reigniting = false;
        target.fireInfo.currentHealth = target.fireInfo.maxHealth;
        mainPS.startLifetime = normal;
        UpdateHealth(fireInfo, transform.Find("HealthIcon(Clone)").gameObject);
    }


}
