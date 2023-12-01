using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyScript : MonoBehaviour { 
    // Variables
    // // Player Variables
    private GameObject player;

    // // Projectile Variables
    public GameObject enemyProjectilePrefab;
    public GameObject enemyProjectileStartPoint;

    // // Fire Rate
    private float secondsBetweenFire = 3f;

    // // Animator
    public Animator EnemyAnim;

    // Start is called before the first frame update
    void Start(){
        // Get the player object
        player = GameObject.Find("Player");

        // Check if the player object exists
        if (player == null) {
            Debug.LogError("Player object not found");
            // Destroy the enemy object
            Destroy(gameObject);
        }


        // Invoke the FireEnemyProjectile method every secondsBetweenFire seconds
        InvokeRepeating("FireEnemyProjectile", secondsBetweenFire, secondsBetweenFire);

    }

    // Update is called once per frame
    void Update(){
        // Get the direction of the player
        Vector3 direction = player.transform.position - transform.position;

        // Create a new Quaternion that preserves the current X and Z rotations
        // and allows rotation on the Y axis based on the direction of the player
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 
                                                  Quaternion.LookRotation(direction).eulerAngles.y, 
                                                  transform.rotation.eulerAngles.z);

        // Assign the new rotation to the enemy
        transform.rotation = newRotation;

        }

    public void FireEnemyProjectile() {
        // Play the enemy's attack animation
        EnemyAnim.SetTrigger("EnemyShoot");

        // Instantiate a projectile based on the given prefab
        GameObject projectileObject = Instantiate(enemyProjectilePrefab);

        // Set the projectile's position to the start point's position
        projectileObject.transform.position = enemyProjectileStartPoint.transform.position;

        // Set the projectile's forward direction to the start point's forward direction
        projectileObject.transform.forward = enemyProjectileStartPoint.transform.forward;



        // EnemyAnim.ResetTrigger("EnemyIdle");
        EnemyAnim.SetTrigger("EnemyIdle");
    }


    void OnTriggerEnter(Collider other) {
        // If the enemy collides with the player
        if (other.CompareTag("Player")) {
            // Debug.Log("Player dies");    // DEBUG LINE
            // End the game
            FindObjectOfType<GameManagerScript>().EndGameLose();
        }
    }
}