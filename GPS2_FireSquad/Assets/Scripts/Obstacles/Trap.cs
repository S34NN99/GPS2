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

            if (target.myPlayer.characterType != PublicEnumList.CharacterType.Demolisher)
            {
                target.myPlayer.isStunned = true;
                gameManager.CheckIfPlayerStun(target);
                trappedPlayer.Add(target.gameObject);
            }
        }
    }
}
