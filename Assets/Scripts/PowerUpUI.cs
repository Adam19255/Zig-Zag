using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpUI : MonoBehaviour
{
    public static PowerUpUI Instance { get; private set; } // Singleton instance

    [SerializeField] private TextMeshProUGUI gemsText;

    private void Awake() {
        Instance = this; // Set the singleton instance

        // Load the initial gems count from PlayerPrefs
        gemsText.text = PlayerPrefs.GetInt("Gems", 0).ToString() + " x";
    }

    public void UpdateGems(int gems) {
        // Update the gems text
        gemsText.text = gems.ToString() + " x";

        // Save the gems count to PlayerPrefs
        PlayerPrefs.SetInt("Gems", gems);
        PlayerPrefs.Save(); // Ensure the changes are saved
    }
}
