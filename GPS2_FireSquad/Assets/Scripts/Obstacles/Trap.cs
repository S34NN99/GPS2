using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private GameManager gameManager;
    public List<GameObject> trappedPlayer;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement target = collision.gameObject.GetComponent<PlayerMovement>();
            IPlayer iPlayer = target.GetComponent<IPlayer>();
            if (target.myPlayer.characterType != PublicEnumList.CharacterType.Demolisher)
            {
                target.Stun(target);
                iPlayer.UniqueAnimation("Trap", true);
                trappedPlayer.Add(target.gameObject);
            }
        }
    }
}
