using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour{
    public Transform cameraPosition;

    // Update is called once per frame
    void Update(){
        // Make camera always move with player
        transform.position = cameraPosition.position;
    }
}
