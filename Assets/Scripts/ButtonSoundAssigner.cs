using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ButtonSoundAssigner : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager; // Reference to the SoundManager

    private void Start() {
        // Find all buttons in the scene
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons) {
            // Add the PlayButtonClickSound method to the onClick event
            button.onClick.AddListener(() => soundManager.ButtonClickSound());
        }
    }
}
