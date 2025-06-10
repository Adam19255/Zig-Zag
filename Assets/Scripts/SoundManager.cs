using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } // Singleton instance

    [SerializeField] private AudioClip playerMovement;
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip gemPickup;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip noMoney;
    [SerializeField] private AudioClip tileColorChange;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeLabel;

    private AudioSource audioSource; // Main AudioSource for UI sounds
    private float volume = 1f; // Default volume

    private void Awake() {
        // Set this as the singleton instance
        Instance = this;

        // Add AudioSource component for UI sounds
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // 2D sound for UI
        audioSource.playOnAwake = false;

        // Load the volume from PlayerPrefs
        volume = PlayerPrefs.GetFloat("Volume", 1f); // Default volume is 1 if not set

        // Set up the volume slider
        if (volumeSlider != null) {
            volumeSlider.value = volume; // Set the slider to the current volume
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged); // Add listener for slider changes

            // Add event trigger for when the slider is released
            EventTrigger trigger = volumeSlider.gameObject.GetComponent<EventTrigger>();
            if (trigger == null) {
                trigger = volumeSlider.gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { OnSliderReleased(); });
            trigger.triggers.Add(entry);
        }

        UpdateVolumeLabel(); // Update the volume label text
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
        // Play tile color change sound on player
        PlayerScript.Instance.PlayPlayerSound(tileColorChange, volume);
    }

    private void PlayerScript_OnGameStart(object sender, EventArgs e) {
        // Play button click sound on player
        PlayerScript.Instance.PlayPlayerSound(buttonClick, volume);
    }

    private void PlayerScript_OnPlayerMovement(object sender, EventArgs e) {
        // Play movement sound on player
        PlayerScript.Instance.PlayPlayerSound(playerMovement, volume);
    }

    private void PlayerScript_OnPlayerDeath(object sender, EventArgs e) {
        // Play death sound on player with reduced spatial blend
        PlayerScript.Instance.PlayPlayerSound(playerDeath, volume, true);
    }

    private void PlayerScript_OnGemPickup(object sender, EventArgs e) {
        // Play gem pickup sound on player
        PlayerScript.Instance.PlayPlayerSound(gemPickup, volume);
    }

    public void ButtonClickSound() {
        PlayUISound(buttonClick);
    }

    public void NoMoneySound() {
        PlayUISound(noMoney);
    }

    public void TileColorChangeSound() {
        PlayUISound(tileColorChange);
    }

    // Method for playing UI sounds through SoundManager's AudioSource
    private void PlayUISound(AudioClip audioClip, float volumeMultiplier = 1f) {
        if (audioClip != null && audioSource != null) {
            audioSource.volume = volumeMultiplier * volume;
            audioSource.PlayOneShot(audioClip);
        }
    }

    // This method is called whenever the slider value changes
    private void OnVolumeChanged(float newValue) {
        volume = newValue; // Update the volume
        PlayerPrefs.SetFloat("Volume", volume); // Save the volume to PlayerPrefs
        PlayerPrefs.Save(); // Save the changes

        // Update the AudioSource volume
        audioSource.volume = volume;

        UpdateVolumeLabel(); // Update the volume label text
    }

    private void OnSliderReleased() {
        PlayUISound(buttonClick);
    }

    public float GetVolume() {
        return volume; // Return the current volume
    }

    private void UpdateVolumeLabel() {
        if (volumeLabel != null) {
            volumeLabel.text = $"Volume: {Mathf.RoundToInt(volume * 100)}";
        }
    }
}
