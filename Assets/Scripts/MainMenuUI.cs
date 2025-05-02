using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private float sceneLoadDelay = 0.2f;

    private void Awake() {
        playButton.onClick.AddListener(() => StartCoroutine(LoadSceneWithDelay(Loader.Scene.GameScene)));
        shopButton.onClick.AddListener(() => StartCoroutine(LoadSceneWithDelay(Loader.Scene.ShopScene)));
        settingsButton.onClick.AddListener(() => {
            SoundManager.Instance.ButtonClickSound(); // Play the sound
            SettingsUI.Instance.Show(); // Show the settings UI
        });
        quitButton.onClick.AddListener(() => {
            SoundManager.Instance.ButtonClickSound(); // Play the sound
            Application.Quit();
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
