using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootScript : MonoBehaviour{
    // Variables
    // // Projectile Prefabs
    public GameObject rightProjectilePrefab;
    public GameObject leftProjectilePrefab;

    // // Projectile Start Points
    public GameObject projectileStartPointRight;
    public GameObject projectileStartPointLeft;

    // // Variables to detect if player is allowed to shoot or to pause the game
    public bool enableScript;
    public bool paused;

    // // Punishment Variables
    public float shootDisableDuration = 3f;
    private float shootDisableTimer;

    // // Audio Variables
    public AudioSource audioSource;
    public AudioClip[] throwSound;  // 0 == leftProjectile, 1 == rightProjectile, 2 == punishmentOverSound
    public float throwSoundVolume = 0.15f;
    

    // Start is called before the first frame update
    void Start(){
        // Start game with script enabled and not paused
        enableScript = true;
        paused = false;

        // Set the shoot disable timer
        shootDisableTimer = shootDisableDuration;

        // Get the audio source
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = throwSound[0];
    }

    // Update is called once per frame
    void Update(){
        // If game is not paused
        if (!paused) {
            // If the script is enabled
            if (enableScript) {
                // If left mouse button is pressed
                if (Input.GetMouseButtonDown(0)) {
                    // Instantiate the left projectile
                    InstantiateProjectiles(leftProjectilePrefab, projectileStartPointLeft, 0);
                    
                // Else if right mouse button is pressed
                } else if (Input.GetMouseButtonDown(1)) {
                    // Instantiate the right projectile
                    InstantiateProjectiles(rightProjectilePrefab, projectileStartPointRight, 1);
                }

            // Else if the script is disabled (player is being punished)
            } else {
                // Decrease the shoot disable timer
                shootDisableTimer -= Time.deltaTime;
                // If the shoot disable timer is less than or equal to 0
                if (shootDisableTimer <= 0f) {
                    // Enable the script (Allow player to shoot again)
                    enableScript = true;
                    // Reset the shoot disable timer
                    shootDisableTimer = shootDisableDuration;

                    // Play punishment over sound
                    audioSource.clip = throwSound[2];
                    audioSource.Play();
                }
            }
        }
    }
    
    // Method to instantiate projectiles
    public void InstantiateProjectiles(GameObject prefab, GameObject startPoint, int leftOrRight) {
        // Instantiate a projectile based on the given prefab
        GameObject projectileObject = Instantiate(prefab);

        // Set the projectile's position to the start point's position
        projectileObject.transform.position = startPoint.transform.position;

        // Set the projectile's forward direction to the start point's forward direction
        projectileObject.transform.forward = startPoint.transform.forward;

        // Create a new game object to hold the projectile audio
        GameObject projectileAudioObject = new GameObject("ProjectileAudio");
        // Add an audio source to the projectile audio object
        AudioSource projectileAudioSource = projectileAudioObject.AddComponent<AudioSource>();
        // Set the projectile audio source's clip to the throw sound
        projectileAudioSource.clip = throwSound[leftOrRight];
        // Set the projectile audio source's volume to the throw sound volume
        projectileAudioSource.volume = throwSoundVolume;
        // Play the projectile audio source's clip
        projectileAudioSource.Play();
        // Destroy the projectile audio object after the clip is done playing
        Destroy(projectileAudioObject, projectileAudioSource.clip.length);
    }
}
