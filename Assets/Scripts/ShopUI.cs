using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour {
    public static ShopUI Instance { get; private set; } // Singleton instance

    [SerializeField] private Button backButton; // Reference to the back button

    private void Awake() {
        Instance = this; // Set the singleton instance
        backButton.onClick.AddListener(() => {
            SoundManager.Instance.ButtonClickSound(); // Play the sound
            Hide(); // Hide the shop UI
        });
    }

    private void Start() {
        Hide(); // Hide the shop UI by default
    }

    public void Show() {
        gameObject.SetActive(true); // Show the shop UI
    }

    public void Hide() {
        gameObject.SetActive(false); // Hide the shop UI
    }
}