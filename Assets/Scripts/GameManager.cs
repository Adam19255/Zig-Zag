using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public int Gems {
        get => PlayerPrefs.GetInt("Gems", 0);
        private set {
            PlayerPrefs.SetInt("Gems", value);
            PlayerPrefs.Save();
        }
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool SpendGems(int amount) {
        if (Gems >= amount) {
            Gems -= amount;
            return true;
        }
        return false;
    }

    public void AddGems(int amount) {
        Gems += amount;
    }
}
