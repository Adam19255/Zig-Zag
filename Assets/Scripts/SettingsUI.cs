using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance { get; private set; } // Singleton instance

    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI soundEffectText;

    private void Awake() {
        Instance = this; // Set the singleton instance

        backButton.onClick.AddListener(() => {
            SoundManager.Instance.ButtonClickSound(); // Play the sound
            Hide(); // Hide the settings UI
            MainMenuUI.Instance.Show(); // Show the main menu UI
        });
    }

    private void Start() {
        Hide(); // Hide the settings UI by default
    }

    public void Show() {
        gameObject.SetActive(true); // Show the settings UI
    }

    public void Hide() {
        gameObject.SetActive(false); // Hide the settings UI
    }
}
