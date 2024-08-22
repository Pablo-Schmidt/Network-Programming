using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    public float initialSpeed = 2f; // Initial speed of the enemy
    public float speedIncreaseRate = 0.1f; // How much the speed increases over time
    private Transform targetPlayer; // The player the enemy will follow
    public float currentSpeed;
    public static int Score = 0;

    public void Start()
    {
        currentSpeed = initialSpeed;
        InvokeRepeating(nameof(UpdateTarget), 0f, 1f); // Update target player every second
    }

    void Update()
    {
        // Increase speed over time!!! 
        currentSpeed += speedIncreaseRate * Time.deltaTime;

        // Move towards the target player
        if (targetPlayer != null)
        {
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            transform.position += direction * currentSpeed * Time.deltaTime;
        }
    }

    private void UpdateTarget()
    {
        // Find all players in the scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 0)
            return;

        // Find the closest player
        float shortestDistance = Mathf.Infinity;
        Transform nearestPlayer = null;

        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = player.transform;
            }
        }

        if (nearestPlayer != null)
        {
            targetPlayer = nearestPlayer;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer)
        {
            // Check if the enemy collides with a bullet
            if (collision.CompareTag("Bullet"))
            {
                Score++;
                EnemySpawner eS = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
                eS.SendScoreRPC();
                Debug.Log("Enemy destroyed! Score: " + Score);
                // Destroy the enemy and the bullet
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
