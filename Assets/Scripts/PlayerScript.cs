using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour{
    [SerializeField] private float speed = 15f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private LayerMask whatIsGround; // Layer mask for ground detection for the player
    [SerializeField] private Transform contactPoint; // Reference to the contact point for the player
    [SerializeField] private GameObject spotLight; // Reference to the spotlight for the player
    [SerializeField] private GameOverScript gameOverScript; // Reference to the GameOverScript

    private Vector3 previousMoveDir = Vector3.zero; // Store the previous move direction
    private bool changeDirection = false; // Track if the direction has changed
    private int score = 0; // Initialize score to 0
    private float rotationSpeed = 20f; // Speed of rotation for the player visuals
    private bool isDead = false; // Track if the player is dead
    private bool isGameStarted = false; // Track if the game has started

    public event EventHandler OnPlayerMovement;
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnGemPickup;
    public event EventHandler OnGameStart;
    public event EventHandler On50Points;

    public static PlayerScript Instance { get; private set; }

    void Awake() {
        // Ensure only one instance of PlayerScript exists
        if (Instance == null) {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update() {
        // Check if the player is dead
        if (!IsGrounded()) {
            // Stop the camera from following the player
            if (mainCamera != null) {
                mainCamera.parent = null; // Detach the camera from the player
            }
            // Stop the spotlight from following the player
            if (spotLight != null) {
                spotLight.transform.parent = null; // Detach the spotlight from the player
            }
            return; // Exit the update method if the player is not grounded
        }

        // Get the input from the GameInput class
        Vector3 moveDir = gameInput.GetMovementVector();
        float moveAmount = speed * Time.deltaTime;

        // Check if the move direction has changed
        if (moveDir != previousMoveDir) {
            changeDirection = true;
            // Check if the game has started
            if (!isGameStarted) {
                isGameStarted = true; // Set the game as started
                OnGameStart?.Invoke(this, EventArgs.Empty);
            }
            OnPlayerMovement?.Invoke(this, EventArgs.Empty);
            // Update the score
            score++;
            // Update the score text
            if (scoreText != null) {
                scoreText.text = score.ToString();
            }
            // Update player speed
            if (score % 50 == 0 && speed < 30) {
                speed += 1f; // Increase speed every 50 points
                On50Points?.Invoke(this, EventArgs.Empty);
            }
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
                child.Rotate(Vector3.left, -(rotationSpeed * speed) * Time.deltaTime);
            }
            else if (moveDir == Vector3.left) {
                if (changeDirection) {
                    // Reset rotation if direction changed
                    child.rotation = Quaternion.identity;
                }
                child.Rotate(Vector3.forward, (rotationSpeed * speed) * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Gem")) {
            other.gameObject.SetActive(false);
            // Instantiate floating text at the player's position
            if (floatingTextPrefab) {
                GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            }
            // Update the score for collecting a gem
            score += 2;
            // Save the gems collected
            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems", 0) + 1);
            PlayerPrefs.Save(); // Save the new gems
            OnGemPickup?.Invoke(this, EventArgs.Empty);
            // Update the score text
            if (scoreText != null) {
                scoreText.text = score.ToString();
            }
        }
    }

    private bool IsGrounded() {
        Collider[] colliders = Physics.OverlapSphere(contactPoint.position, 0.5f, whatIsGround);
        foreach (Collider collider in colliders) {
            if (collider.gameObject != gameObject) { // Ignore the player itself
                return true; // Player is grounded
            }
        }
        if (!isDead) {
            isDead = true; // Set isDead to true if the player is not grounded
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            // Trigger the game over animation via GameOverScript
            if (gameOverScript != null) {
                gameOverScript.TriggerGameOverAnimation();
                gameOverScript.UpdateScore(GetScore());
            }
        }
        return false; // Player is not grounded
    }

    public int GetScore() {
        return score;
    }

}
