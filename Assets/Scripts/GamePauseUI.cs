using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake() {
        resumeButton.onClick.AddListener(() => {
            SoundManager.Instance.ButtonClickSound();
            PlayerScript.Instance.TogglePauseGame();
        } );
        mainMenuButton.onClick.AddListener(() => {
            SoundManager.Instance.ButtonClickSound();
            Loader.Load(Loader.Scene.MainMenuScene);
        } );
    }

    private void Start() {
        PlayerScript.Instance.OnGamePaused += PlayerScript_OnGamePaused;
        PlayerScript.Instance.OnGameUnpaused += PlayerScript_OnGameUnpaused;
        Hide();
    }

    private void PlayerScript_OnGameUnpaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void PlayerScript_OnGamePaused(object sender, System.EventArgs e) {
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
