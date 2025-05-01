using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideOverlay : MonoBehaviour
{
    private void Start() {
        PlayerScript.Instance.OnGameStart += PlayerScript_OnGameStart;
        Show(); // Show the guide overlay by default
    }

    private void PlayerScript_OnGameStart(object sender, EventArgs e) {
        Hide(); // Hide the guide overlay when the game starts
    }

    private void Show() {
        // Show the guide overlay
        gameObject.SetActive(true);
    }

    private void Hide() {
        // Hide the guide overlay
        gameObject.SetActive(false);
    }
}
