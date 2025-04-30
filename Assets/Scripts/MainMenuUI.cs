using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            // Load the game scene
            Loader.Load(Loader.Scene.GameScene);
        });
        shopButton.onClick.AddListener(() => {
            // Load the shop scene
            Loader.Load(Loader.Scene.ShopScene);
        });
        settingsButton.onClick.AddListener(() => {
            // Load the settings scene
            Loader.Load(Loader.Scene.SettingsScene);
        });
        quitButton.onClick.AddListener(() => {
            // Quit the game
            Application.Quit();
        });
    }
}
