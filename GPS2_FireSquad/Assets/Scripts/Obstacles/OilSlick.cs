using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlick : MonoBehaviour
{
    private GameManager gameManager;

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
            if (target.myPlayer.characterType != PublicEnumList.CharacterType.Medic)
            {
                target.Stun(target);
                iPlayer.UniqueAnimation("Slip", true);
                Destroy(this.gameObject);
            }
        }
    }
}
