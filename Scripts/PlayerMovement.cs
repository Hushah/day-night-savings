using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    // Variables
    // // Movement Speed
    public float moveSpeed;

    // // Orientation
    public Transform orientation;

    // // Input
    float horizontalInput;
    float verticalInput;

    // // Direction to move
    Vector3 moveDirection;

    // // Rigidbody
    Rigidbody rb;

    // // Audio for footsteps
    public AudioSource audioSource;
    public AudioClip[] footsteps;


    // Start is called before the first frame update
    void Start(){
        // Getting components
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Getting audio source
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = footsteps[0];
    }

    // Update is called once per frame
    void Update(){
        MyInput();
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    void MyInput() {
        // Get input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // If player is moving
        if(horizontalInput != 0 || verticalInput != 0) {
            // If footsteps are not playing (to prevent overlapping)
            if (!audioSource.isPlaying) {
                // Play random footstep sound
                int randomIndex = Random.Range(0, footsteps.Length);
                audioSource.clip = footsteps[randomIndex];
                audioSource.Play();
            }

        // Else stop footsteps
        } else {
            audioSource.Stop();
        }
    }

    void MovePlayer() {
        // Calculate direction to move
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        // Move the player
        rb.velocity = moveDirection.normalized * moveSpeed;
    }
}
