using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private float sceneLoadDelay = 0.2f;

    private void Awake() {
        // Set the main menu button listener
        mainMenuButton.onClick.AddListener(() => {
            StartCoroutine(LoadSceneWithDelay(Loader.Scene.MainMenuScene));
        });
    }

    private IEnumerator LoadSceneWithDelay(Loader.Scene scene) {
        // Play the button click sound
        SoundManager.Instance.ButtonClickSound();
        // Wait for the sound to finish
        yield return new WaitForSeconds(sceneLoadDelay);
        // Load the scene
        Loader.Load(scene);
    }
}
