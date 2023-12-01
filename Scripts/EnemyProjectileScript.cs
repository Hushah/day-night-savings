using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour{
    // Variables
    // // The speed of the projectile
    public float projectileSpeed = 5f;
    // // The duration of the projectile
    public float projectileLifeDuration = 5f;
    // // The timer for the projectile
    private float lifeTimer;

    // Start is called before the first frame update
    void Start(){
        // Set the life timer
        lifeTimer = projectileLifeDuration;
    }

    // Update is called once per frame
    void Update(){
        // Make the projectile move
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;

        // Check if the projectile should be destroyed
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) {
            Destroy(gameObject);
        }
    }

    // Kill player if projectile hits player
    void OnTriggerEnter(Collider other) {
        // If the projectile hits the player
        if (other.CompareTag("Player")) {
            Destroy(gameObject);
            // Debug.Log("Player is dead");     // DEBUG LINE
            // End the game
            FindObjectOfType<GameManagerScript>().EndGameLose();
        }
    }
}