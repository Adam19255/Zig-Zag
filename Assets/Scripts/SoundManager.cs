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
    [SerializeField] private AudioClip noMoney;
    [SerializeField] private AudioClip tileColorChange;

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
        PlayerScript.Instance.On50Points += PlayerScript_On50Points;
    }

    private void PlayerScript_On50Points(object sender, EventArgs e) {
        PlaySound(tileColorChange, PlayerScript.Instance.transform.position, false);
    }

    private void PlayerScript_OnGameStart(object sender, EventArgs e) {
        PlaySound(buttonClick, PlayerScript.Instance.transform.position, false);
    }

    private void PlayerScript_OnPlayerMovement(object sender, EventArgs e) {
        PlaySound(playerMovement, PlayerScript.Instance.transform.position, false);
    }

    private void PlayerScript_OnPlayerDeath(object sender, EventArgs e) {
        PlaySound(playerDeath, PlayerScript.Instance.transform.position, true);
    }

    private void PlayerScript_OnGemPickup(object sender, EventArgs e) {
        PlaySound(gemPickup, PlayerScript.Instance.transform.position, false);
    }

    public void ButtonClickSound() {
        PlaySound(buttonClick, Vector3.zero, false);
    }

    public void NoMoneySound() {
        PlaySound(noMoney, Vector3.zero, false);
    }

    public void TileColorChangeSound() {
        PlaySound(tileColorChange, Vector3.zero, false);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, bool playerDeath, float volumeMultiplier = 1f) {
        GameObject tempAudioSource = new GameObject("TempAudio"); // Create a temporary GameObject to hold the AudioSource
        tempAudioSource.transform.position = position; // Set the position of the temporary GameObject to the specified position

        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volumeMultiplier * volume;
        if (playerDeath) {
            audioSource.spatialBlend = 0.1f; // Reduce volume for player death sound
        }
        else { 
            audioSource.spatialBlend = 0.5f; // Setting spatial blend to better simulate 2D sound
        }
        audioSource.Play();

        Destroy(tempAudioSource, audioClip.length); // Destroy the GameObject after the sound has finished playing
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
