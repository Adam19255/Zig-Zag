using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject particles;
    [SerializeField] private Transform mainCamera;

    private Rigidbody rb;
    private float customGravity = -100f;
    private Vector3 previousMoveDir = Vector3.zero; // Store the previous move direction
    private bool changeDirection = false; // Track if the direction has changed
    private bool isDead;


    // Start is called before the first frame update
    void Start() {
        isDead = false;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable default gravity
    }

    private void FixedUpdate() {
        // Apply custom gravity
        rb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update() {
        if (isDead) {
            // If the player is dead, stop updating
            return;
        }
        // Get the input from the GameInput class
        Vector3 moveDir = gameInput.GetMovementVector();
        float moveAmount = speed * Time.deltaTime;

        // Check if the move direction has changed
        if (moveDir != previousMoveDir) {
            changeDirection = true;
        }
        else {
            changeDirection = false;
        }
        // Update the previous move direction
        previousMoveDir = moveDir;

        // Move the player
        transform.Translate(moveDir * moveAmount);

        // Get the first child of the player object
        Transform child = transform.GetChild(0);
        if (child != null) {

            // Rotate the player based on the movement direction
            if (moveDir == Vector3.forward) {
                if (changeDirection) {
                    // Reset rotation if direction changed
                    child.rotation = Quaternion.identity;
                }
                child.Rotate(Vector3.left, -270f * Time.deltaTime);
            }
            else if (moveDir == Vector3.left) {
                if (changeDirection) {
                    // Reset rotation if direction changed
                    child.rotation = Quaternion.identity;
                }
                child.Rotate(Vector3.forward, 270f * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Gem")) {
            other.gameObject.SetActive(false);
            // Instantiate the particle effect at the player's position
            GameObject particleEffect = Instantiate(particles, transform.position, Quaternion.identity);
            // Destroy the particle effect after 3 second
            Destroy(particleEffect, 3f);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Tile")) {
            // Check if the player is on the tile
            RaycastHit hit;
            // Start the raycast slightly above the player to ensure it's not starting inside a collider
            Vector3 rayStart = transform.position + Vector3.up * 0.1f;
            // Make the raycast longer to ensure it can reach the tiles
            float rayLength = 5f;

            if (!Physics.Raycast(rayStart, Vector3.down, out hit, rayLength)) {
                isDead = true;
                // Stop the camera from following the player
                if (mainCamera != null) {
                    mainCamera.parent = null; // Detach the camera from the player
                }
            }
        }
    }
}
