using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } // Singleton instance

    [SerializeField] private AudioClip playerMovement;
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip gemPickup;
    [SerializeField] private AudioClip buttonClick;

    private float volume = 1f; // Default volume

    private void Awake() {
        // Set this as the singleton instance
        Instance = this;

        // Load the volume from PlayerPrefs
        volume = PlayerPrefs.GetFloat("Volume", 1f); // Default volume is 1 if not set
    }

    private IEnumerator Start() {
        // Wait until PlayerScript.Instance is initialized
        while (PlayerScript.Instance == null) {
            yield return null; // Wait for the next frame
        }

        // Subscribe to PlayerScript events
        PlayerScript.Instance.OnPlayerMovement += PlayerScript_OnPlayerMovement;
        PlayerScript.Instance.OnPlayerDeath += PlayerScript_OnPlayerDeath;
        PlayerScript.Instance.OnGemPickup += PlayerScript_OnGemPickup;
        PlayerScript.Instance.OnGameStart += PlayerScript_OnGameStart;
    }

    private void PlayerScript_OnGameStart(object sender, EventArgs e) {
        PlaySound(buttonClick, PlayerScript.Instance.transform.position);
    }

    private void PlayerScript_OnPlayerMovement(object sender, EventArgs e) {
        PlaySound(playerMovement, PlayerScript.Instance.transform.position);
    }

    private void PlayerScript_OnPlayerDeath(object sender, EventArgs e) {
        // Get the first child of the player (playerVisual)
        Transform firstChild = PlayerScript.Instance.transform.GetChild(0);
        if (firstChild != null) {
            AudioSource audioSource = firstChild.GetComponent<AudioSource>();
            if (audioSource != null) {
                audioSource.PlayOneShot(playerDeath, volume);
            }
            else {
                Debug.LogWarning("AudioSource not found on playerVisual!");
            }
        }
        else {
            Debug.LogWarning("Player has no first child to play death sound!");
        }
    }


    private void PlayerScript_OnGemPickup(object sender, EventArgs e) {
        PlaySound(gemPickup, PlayerScript.Instance.transform.position);
    }

    public void ButtonClickSound() {
        PlaySound(buttonClick, Vector3.zero);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    public void ChangeVolume() {
        volume += 0.1f; // Increase volume by 0.1
        if (volume > 1f) {
            volume = 0f; // Reset volume to 0 if it exceeds 1
        }

        // Save the volume to PlayerPrefs
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save(); // Save the changes
    }

    public float GetVolume() {
        return volume; // Return the current volume
    }
}
