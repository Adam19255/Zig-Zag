using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    [SerializeField] private float speed = 15f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform contactPoint;
    [SerializeField] private GameObject spotLight;
    [SerializeField] private GameOverScript gameOverScript;

    private Vector3 previousMoveDir = Vector3.zero;
    private bool changeDirection = false;
    private int score = 0;
    private float rotationSpeed = 20f;
    private bool isDead = false;
    private bool isGameStarted = false;

    public event EventHandler OnPlayerMovement;
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnGemPickup;
    public event EventHandler OnGameStart;
    public event EventHandler On50Points;

    public static PlayerScript Instance { get; private set; }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        ApplyEquippedSkin(); // NEW
    }

    private void ApplyEquippedSkin() {
        string equippedSkin = PlayerPrefs.GetString("EquippedSkin", "");
        if (!string.IsNullOrEmpty(equippedSkin)) {
            Texture skinTex = Resources.Load<Texture>("Textures/" + equippedSkin); // Texture must be in Resources/Textures
            if (skinTex != null) {
                Transform child = transform.GetChild(0);
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null) {
                    renderer.material = new Material(renderer.material); // clone material
                    renderer.material.mainTexture = skinTex;
                }
            } else {
                Debug.LogWarning($"Texture not found: {equippedSkin}");
            }
        }
    }

    void Update() {
        if (!IsGrounded()) {
            if (mainCamera != null) mainCamera.parent = null;
            if (spotLight != null) spotLight.transform.parent = null;
            return;
        }

        Vector3 moveDir = gameInput.GetMovementVector();
        float moveAmount = speed * Time.deltaTime;

        if (moveDir != previousMoveDir) {
            changeDirection = true;
            if (!isGameStarted) {
                isGameStarted = true;
                OnGameStart?.Invoke(this, EventArgs.Empty);
            }
            OnPlayerMovement?.Invoke(this, EventArgs.Empty);
            score++;
            if (scoreText != null) scoreText.text = score.ToString();
            if (score % 50 == 0 && speed < 30) {
                speed += 1f;
                On50Points?.Invoke(this, EventArgs.Empty);
            }
        } else {
            changeDirection = false;
        }

        previousMoveDir = moveDir;
        transform.Translate(moveDir * moveAmount);

        Transform child = transform.GetChild(0);
        if (child != null) {
            if (moveDir == Vector3.forward) {
                if (changeDirection) child.rotation = Quaternion.identity;
                child.Rotate(Vector3.left, -(rotationSpeed * speed) * Time.deltaTime);
            } else if (moveDir == Vector3.left) {
                if (changeDirection) child.rotation = Quaternion.identity;
                child.Rotate(Vector3.forward, (rotationSpeed * speed) * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Gem")) {
            other.gameObject.SetActive(false);
            if (floatingTextPrefab) {
                Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            }
            score += 2;
            int gems = PlayerPrefs.GetInt("Gems", 0) + 1;
            PlayerPrefs.SetInt("Gems", gems);
            PlayerPrefs.Save();
            PowerUpUI.Instance.UpdateGems(gems);
            OnGemPickup?.Invoke(this, EventArgs.Empty);
            if (scoreText != null) scoreText.text = score.ToString();
        }
    }

    private bool IsGrounded() {
        Collider[] colliders = Physics.OverlapSphere(contactPoint.position, 0.5f, whatIsGround);
        foreach (Collider collider in colliders) {
            if (collider.gameObject != gameObject) return true;
        }

        if (!isDead) {
            isDead = true;
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            if (gameOverScript != null) {
                gameOverScript.TriggerGameOverAnimation();
                gameOverScript.UpdateScore(GetScore());
            }
        }

        return false;
    }

    public int GetScore() {
        return score;
    }
}
