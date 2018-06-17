using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class InsantiatePlayer : NetworkBehaviour
{
    public GameObject player;
    public Transform playerPrefab;
    private float lastPosition;
    // Use this for initialization

    void Start()
    {
        //InvokeRepeating("SpawnCactus", 1f, 3f);
        if (isLocalPlayer)
        {
            lastPosition = player.transform.position[2];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            float newPosition = player.transform.position[2];
            if (newPosition > lastPosition + 5)
            {
                SpawnPlayer(newPosition);
                lastPosition = newPosition;
            }
        }
    }
    private int counter;
    void SpawnPlayer(float playerPos)
    {
        Instantiate(playerPrefab, new Vector3(Random.Range(-4f, 4f), 3f, playerPos + 10), Quaternion.identity);
        counter++;
    }
}
