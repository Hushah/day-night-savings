using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour{
    // Variables
    // // player and enemy prefabs
    public GameObject player;
    public GameObject enemyPrefab;

    // // Spawn rate variables
    public float currentSpawnRate = 10f;
    private float timer = 0f;
    private Vector3 playerLastPosition;
    public float spawnDelay = 2f;
    public TimeController timeController;
    private DateTime currentTimeValue;

    // Start is called before the first frame update
    void Start(){
        // Set the player last position to the player's current position
        playerLastPosition = player.transform.position;

        // Set the current time value to the current time
        currentTimeValue = timeController.currentTime;
    }

    // Update is called once per frame
    void Update(){
        currentTimeValue = timeController.currentTime;
        // Debug.Log("Current Time: " + currentTimeValue.ToString("HH:mm"));    // DEBUG LINE

        // Update the players last position every frame
        playerLastPosition = player.transform.position;

        // Increment the timer
        timer += Time.deltaTime;

        // If the timer is greater than the current spawn rate
        if(timer >= currentSpawnRate) {
            // Spawn an enemy
            StartCoroutine(SpawnEnemyWithDelay(playerLastPosition, spawnDelay));

            // Reset the timer
            timer = 0f;
        }

        // Change spawn rate based on time of day
        ChangeSpawnRate(currentTimeValue.Hour);
    }

    void ChangeSpawnRate(int currentHour) {
        // Define an array of spawn rates for each hour of the day
        float[] spawnRates = { 2.4f, 2.4f, 2.4f, 2.3f, 2.3f, 2.2f, 2.2f, 2.1f, 2.1f, 2f, 2f, 1.9f, 1.8f, 1.7f, 1.6f, 1.5f, 1.4f, 1.3f, 1.2f, 1.1f, 1f, 0.8f, 0.6f, 0.4f, 0.4f };

        // Set the current spawn rate based on currentHour
        currentSpawnRate = spawnRates[currentHour];
    }

    // Spawn an enemy after a delay
    IEnumerator SpawnEnemyWithDelay(Vector3 position, float delay) {
        yield return new WaitForSeconds(delay);
        SpawnEnemy(position);
        Debug.Log("Spawned Enemy!");
    }

    void SpawnEnemy(Vector3 position) {
        // Cast a ray down from the spawn point to find the ground
        RaycastHit hit;

        // If the ray hit the ground
        if(Physics.Raycast(position, Vector3.down, out hit)) { 
            // Spawn the enemy at the hit point
            Instantiate(enemyPrefab, hit.point, Quaternion.identity);
        } else {
            // Else spawn the enemy at the spawn point
            Instantiate(enemyPrefab, position, Quaternion.identity);
        }

    }
}
