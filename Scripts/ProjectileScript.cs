using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour{
    // Variables
    // // Speed of the projectile
    public float projectileSpeed = 50f;
    // // How long the projectile should last before being destroyed
    public float projectileLifeDuration = 2f;
    // // Variable to track how long the projectile has been alive for
    private float lifeTimer;

    // Boolean to check if the projectile is the right projectile
    public bool isRightProjectile;

    // // Accessing the player's arms and script
    private GameObject playerArms;
    PlayerShootScript playerShootScript;

    // // Audio Variabled
    public AudioSource audioSource;
    public AudioClip[] sounds;  // 0 == destroyOrb, 1 == destroyEnemy, 2 == incorrectProjectile


    // Start is called before the first frame update
    void Start(){
        // Set the life timer
        lifeTimer = projectileLifeDuration;
        
        // Get the player's arms and script
        playerArms = GameObject.Find("playerArms");
        // Get the player's shoot script
        playerShootScript = playerArms.GetComponent<PlayerShootScript>();

        // Get the audio source
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sounds[0];
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

    // Destroy the projectile when it enters a trigger
    void OnTriggerEnter(Collider other) {


        // If the projectile is the right projectile
        if (isRightProjectile) {
            // Create a new game object to play the audio
            GameObject projectileAudioObject = new GameObject("ProjectileAudio");
            AudioSource projectileAudioSource = projectileAudioObject.AddComponent<AudioSource>();

            // Set the volume of the audio
            projectileAudioSource.volume = 0.05f;

            // If the object is the enemy, destroy the enemy
            if (other.CompareTag("Enemy")) {
                // Destroy the enemy and play the sound
                Destroy(other.gameObject);
                projectileAudioSource.PlayOneShot(sounds[1]);
                
            // Else if the object is the enemy projectile
            } else if (other.CompareTag("EnemyProjectile")) {
                // Stop player from shooting
                playerShootScript.enableScript = false;

                // Play the audio
                projectileAudioSource.volume = 0.2f;
                projectileAudioSource.PlayOneShot(sounds[2]);

                // Destroy the enemy projectile
                Destroy(other.gameObject);
                // Debug.Log("Punish the player");  // DEBUG LINE
            }
            
            // Destroy the projectile audio object after the clip is done playing
            Destroy(projectileAudioObject, 1f);

        // Else if the projectile is the left projectile
        } else {
            // Create a new game object to play the audio
            GameObject projectileAudioObject = new GameObject("ProjectileAudio");
            AudioSource projectileAudioSource = projectileAudioObject.AddComponent<AudioSource>();

            // Set the volume
            projectileAudioSource.volume = 0.05f;
            
            // If the object is the enemy
            if (other.CompareTag("Enemy")) {
                // Stop player from shooting
                playerShootScript.enableScript = false;

                // Play the audio
                projectileAudioSource.volume = 0.2f;
                projectileAudioSource.PlayOneShot(sounds[2]);

                // Debug.Log("Punish the player");  // DEBUG LINE

            // Else if the object is the enemy projectile
            } else if (other.CompareTag("EnemyProjectile")) {
                // Destroy the enemy projectile and play the sound
                projectileAudioSource.PlayOneShot(sounds[0]);
                Destroy(other.gameObject);
            }
            
            // Destroy the projectile audio object after the clip is done playing
            Destroy(projectileAudioObject, 1f);
        }

        // No matter what, destroy the projectile
        Destroy(gameObject);
    }
}
