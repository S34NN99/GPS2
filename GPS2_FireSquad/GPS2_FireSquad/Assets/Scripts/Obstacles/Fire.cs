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
    public bool isDamaged = false;

    [Space(20)]
    public List<GameObject> trappedPlayer;
}

public class Fire : MonoBehaviour
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
    }

    private void Update()
    {
        if(fireInfo.currentHealth <= 0)
        {
            Death();
        }

        if(fireInfo.isDamaged)
        {
            if (!fireInfo.isImmunity)
            {
                Debug.Log(fireInfo.currentHealth);
                LoseHealth();
                StartCoroutine(FireImmunity());
            }
        }
        else
        {
            if(fireInfo.currentHealth < fireInfo.maxHealth)
            {
                if (!reigniting)
                {
                    StartCoroutine(Reignite(this));
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement target = collision.gameObject.GetComponent<PlayerMovement>();
            IAnimation iAnimation = collision.gameObject.GetComponent<IAnimation>();

            if (target.myPlayer.characterType != PublicEnumList.CharacterType.Extinguisher)
            {
                if (!target.myPlayer.isStunned)
                {

                    target.Stun(target);
                    //spawned fire on player
                    SpawnFireOnPlayer(target.transform);
                    fireInfo.trappedPlayer.Add(target.gameObject);

                    iAnimation.Walking(false);
                }
                else
                {
                    return;
                }
            }
        }
    }

    private void SpawnFireOnPlayer(Transform player)
    {
        GameObject fire = Instantiate(gameManager.firePrefab, player);
        fire.transform.position = player.transform.position;

    }

    public void LoseHealth()
    {
        fireInfo.currentHealth--;
        fireInfo.isImmunity = true;

        if(transform.Find("HealthIcon(Clone)"))
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

    public void Death()
    {
        if (fireInfo.trappedPlayer.Count > 0)
        {
            for (int i = 0; i < fireInfo.trappedPlayer.Count; i++)
            {
                PlayerMovement playerMovement = fireInfo.trappedPlayer[i].GetComponent<PlayerMovement>();
                playerMovement.UnStun(playerMovement);
                foreach(Transform transform in playerMovement.transform)
                {
                    if(transform.CompareTag("Fire"))
                    {
                        Debug.Log("Fire found");
                        Destroy(transform.gameObject);
                        break;
                    }
                }
            }
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
        fireInfo.isDamaged = false;
        fireInfo.isImmunity = false;
    }

    public IEnumerator Reignite(Fire target)
    {
        reigniting = true;
        ParticleSystem firePS = target.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainPS = firePS.main;

        yield return new WaitForSeconds(5.0f);
        reigniting = false;
        target.fireInfo.currentHealth = target.fireInfo.maxHealth;
        mainPS.startLifetime = normal;
        UpdateHealth(fireInfo, transform.Find("HealthIcon(Clone)").gameObject);
    }


}
