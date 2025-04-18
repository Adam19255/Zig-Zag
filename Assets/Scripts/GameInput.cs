using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private Vector3 moveDir;

    public Vector3 GetMovementVector() {
        if (Input.GetMouseButtonDown(0)) {
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
