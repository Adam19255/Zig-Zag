using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance { get; private set; } // Singleton instance

    [SerializeField] private Button soundEffectButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI soundEffectText;

    private void Awake() {
        Instance = this; // Set the singleton instance

        soundEffectButton.onClick.AddListener(() => {
            SoundManager.Instance.ButtonClickSound(); // Play the sound
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        backButton.onClick.AddListener(() => {
            SoundManager.Instance.ButtonClickSound(); // Play the sound
            Hide(); // Hide the settings UI
        });
    }

    private void Start() {
        UpdateVisual();
        Hide(); // Hide the settings UI by default
    }

    private void UpdateVisual() {
        soundEffectText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
    }

    public void Show() {
        gameObject.SetActive(true); // Show the settings UI
    }

    public void Hide() {
        gameObject.SetActive(false); // Hide the settings UI
    }
}
