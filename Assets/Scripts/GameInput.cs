using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private Vector3 moveDir;

    public Vector3 GetMovementVector() {
        // Get the input from the player

        // Mouse input
        if (Input.GetMouseButtonDown(0)) {
            if (moveDir == Vector3.forward) {
                moveDir = Vector3.left;
            }
            else {
                moveDir = Vector3.forward;
            }
        }

        // Keyboard input
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (moveDir == Vector3.forward) {
                moveDir = Vector3.left;
            }
            else {
                moveDir = Vector3.forward;
            }
        }

        return moveDir;
    }
}
