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
            SceneManager.LoadScene("GameScene");
        });
        //shopButton.onClick.AddListener(OnShopButtonClicked);
        //settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(() => {
            // Quit the game
            Application.Quit();
        });
    }
}
