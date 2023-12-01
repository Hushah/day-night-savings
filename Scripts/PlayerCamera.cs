using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour{
    // Variables
    // // Mouse Sensitivity
    public float sensitivityX;
    public float sensitivityY;

    // // Player Orientation
    public Transform playerOrientation;
    public Transform playerArms;

    // // Rotation
    public float xRotation;
    public float yRotation;

    // // Pause
    public bool paused = false;

    // Start is called before the first frame update
    void Start(){
        // Locking cursor to middle of the screen and making invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update(){
        // If game is not paused
        if (!paused) {
            // Get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityY;

            yRotation += mouseX;
            xRotation -= mouseY;

            // Stop player from looking above and below 90 degrees
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Rotate camera and orientation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

    }
}
