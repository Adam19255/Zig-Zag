using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; } // Singleton instance

    private Vector3 moveDir;

    public event EventHandler OnPauseGame; // Event to notify when the game is paused

    private void Awake() {
        if (Instance == null) {
            Instance = this; // Set the singleton instance
        }
        else {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

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

    public void Pause() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnPauseGame?.Invoke(this, EventArgs.Empty);
        }
    }
}
