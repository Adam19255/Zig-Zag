using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] private Animator gameOverAnimator; // Reference to the GameOver animator
    [SerializeField] private Text newHighScore;
    [SerializeField] private Image backGround;
    [SerializeField] private Text[] scoreTexts;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private float sceneLoadDelay = 0.2f;


    private void Awake() {
        // Set the main menu button listener
        mainMenuButton.onClick.AddListener(() => {
            StartCoroutine(LoadSceneWithDelay(Loader.Scene.MainMenuScene));
        });

        // Set the restart button listener
        restartButton.onClick.AddListener(() => {
            StartCoroutine(LoadSceneWithDelay(Loader.Scene.GameScene));
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

    public void TriggerGameOverAnimation() {
        if (gameOverAnimator != null) {
            gameOverAnimator.SetTrigger("GameOver");
        }
    }

    public void UpdateScore(int score) {
        if (scoreTexts[1].text != null) {
            scoreTexts[1].text = score.ToString();
        }

        // Check and update the high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore) {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score;
            newHighScore.gameObject.SetActive(true); // Show the new high score text
            backGround.color = new Color32(248, 84, 233, 255); // Change background color to green
            foreach (Text text in scoreTexts) {
                text.color = Color.white; // Change text color to white
            }
        }

        // Display the high score
        if (scoreTexts[3].text != null) {
            scoreTexts[3].text = highScore.ToString();
        }
    }
}
