using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public Transform[] spawnPoints; // Array of spawn points (3 points only, pre selected)

    public float initialSpawnInterval = 3f; // Initial time between spawns
    public float spawnIntervalDecreaseRate = 15.05f; // Rate at which the spawn interval decreases
    public float minimumSpawnInterval = 1f;
    private int currentSpawnPointIndex = 0; // Keep track of the current spawn point
    private float currentSpawnInterval;
    public TextMeshProUGUI ScoreText;
    NetworkVariable<int> score = new();
    //public TextMeshProUGUI DeathsTmp; // this should be in a manager, not each enemy

    public void Initialize()
    {

        //ScoreText.text = Enemy.Score.ToString();  //Do this every time an enemy dies

        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnEnemies());
    }

    private void Start()
    {
        if (IsServer)
        {
            score.Value = 0;
            ScoreText.text = "Score: " + score.Value;
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Get the current spawn point
            Transform spawnPoint = spawnPoints[currentSpawnPointIndex];

            GameObject spawnis = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            spawnis.GetComponent<NetworkObject>().Spawn();

            // Go to the next spawn point in the array
            currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnPoints.Length;

            // Wait for the current spawn interval before spawning the next enemy!!!
            yield return new WaitForSeconds(currentSpawnInterval);

            // Make enemies spawn faster
            if (currentSpawnInterval > minimumSpawnInterval)
            {
                currentSpawnInterval -= spawnIntervalDecreaseRate * Time.deltaTime;
            }
        }
    }

    [Rpc(SendTo.Server)]
    public void SendScoreRPC()
    {
        UpdateScoreUIRpc();
        score.Value = score.Value + 1; // wrong but ui works, shows -1 of actual score
    }
    [Rpc(SendTo.Everyone)]
    private void UpdateScoreUIRpc()
    {
        ScoreText.text = "Score: " + score.Value;
    }

}