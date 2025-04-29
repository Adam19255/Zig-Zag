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
