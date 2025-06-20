using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuUI : MonoBehaviour {
    public static MainMenuUI Instance { get; private set; } // Singleton instance

    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private float sceneLoadDelay = 0.2f;

    private void Awake() {
        Instance = this; // Set the singleton instance

        // Add listeners to the buttons
        playButton.onClick.AddListener(() => StartCoroutine(LoadSceneWithDelay(Loader.Scene.GameScene)));
        shopButton.onClick.AddListener(() => {
            SoundManager.Instance.ButtonClickSound(); // Play the sound
            ShopUI.Instance.Show(); // Show the shop UI
            Hide(); // Hide the main menu UI
        });
        settingsButton.onClick.AddListener(() => {
            SoundManager.Instance.ButtonClickSound(); // Play the sound
            SettingsUI.Instance.Show(); // Show the settings UI
            Hide(); // Hide the main menu UI
        });
        quitButton.onClick.AddListener(() => {
            SoundManager.Instance.ButtonClickSound(); // Play the sound
            QuitGame();
        });

        Time.timeScale = 1.0f;
    }

    private IEnumerator LoadSceneWithDelay(Loader.Scene scene) {
        // Play the button click sound
        SoundManager.Instance.ButtonClickSound();
        // Wait for the sound to finish
        yield return new WaitForSeconds(sceneLoadDelay);
        // Load the scene
        Loader.Load(scene);
    }

    private void QuitGame() {
        if (Application.isEditor) {
            // If we're in the Unity Editor, stop playing
            EditorApplication.isPlaying = false;
        }
        else {
            // If we're in a built application, quit the application
            Application.Quit();
        }
    }

    public void Show() {
        gameObject.SetActive(true); // Show the main menu UI
    }

    public void Hide() {
        gameObject.SetActive(false); // Hide the main menu UI
    }
}