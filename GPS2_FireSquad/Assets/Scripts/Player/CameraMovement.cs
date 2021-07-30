using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public GameObject playerGroup;
    private GameObject selectedPlayer;

    public float smoothSpeed = 0.125f;
    private float speed = 45f;
    public float speedOffset;
    public Vector3 offset;

    private bool isSwitchingCharacter = false;

    private void Start()
    {
        selectedPlayer = playerGroup.transform.GetChild(0).gameObject;
        selectedPlayer.GetComponent<PlayerMovement>().playerSelected = true;
        FindObjectOfType<GameManager>().playerObject = selectedPlayer;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = selectedPlayer.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = Vector3.MoveTowards(transform.position, smoothedPosition, speed * Time.deltaTime);

        transform.LookAt(selectedPlayer.transform.position);
    }

    public void SwitchPlayer(GameObject player)
    {
        float distance = Vector3.Distance(selectedPlayer.transform.position, player.transform.position);

        isSwitchingCharacter = true;

        speed = distance + speedOffset;

        selectedPlayer.GetComponent<PlayerMovement>().playerSelected = false;
        selectedPlayer = player;
        FindObjectOfType<GameManager>().playerObject = selectedPlayer;
        selectedPlayer.GetComponent<PlayerMovement>().playerSelected = true;
    }
}
