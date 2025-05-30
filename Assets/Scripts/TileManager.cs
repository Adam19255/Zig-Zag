using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {

    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject currentTile;
    [SerializeField] private GameObject startTile;
    [SerializeField] private Transform tileParent;

    private Stack<GameObject> leftTiles = new Stack<GameObject>();
    private Stack<GameObject> topTiles = new Stack<GameObject>();
    private Stack<GameObject> startTiles = new Stack<GameObject>();

    private static TileManager instance;
    private int leftTileCount = 0;
    private int topTileCount = 0;

    private Color[] tileColors = new Color[10];
    private List<Color> shuffledColorList = new List<Color>();
    private int colorCycleIndex = 0;
    private Color currentColor;
    private int lastAppliedTier = 0;

    public static TileManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<TileManager>();
            }
            return instance;
        }
    }

    public Stack<GameObject> LeftTiles {
        get { return leftTiles; }
        set { leftTiles = value; }
    }

    public Stack<GameObject> TopTiles {
        get { return topTiles; }
        set { topTiles = value; }
    }

    public Stack<GameObject> StartTiles {
        get { return startTiles; }
        set { startTiles = value; }
    }

    void Start() {
        SetupFixedColors();

        shuffledColorList = new List<Color>(tileColors);
        ShuffleColors(shuffledColorList);
        colorCycleIndex = 0;
        currentColor = shuffledColorList[colorCycleIndex];

        if (tileParent == null) {
            tileParent = new GameObject("TileParent").transform;
        }

        AddTiles(50); // Add 50 tiles to the stack
        // Add the startTile to the stack
        startTiles.Push(startTile);

        for (int i = 0; i < 25; i++) { // Create 25 tiles at the start
            CreateTiles();
        }
    }

    // Function to add tiles to the stack to recycle them
    void Update() {
        int score = PlayerScript.Instance.GetScore();
        int tier = score / 50;

        if (tier != lastAppliedTier && score >= 50) {
            lastAppliedTier = tier;

            colorCycleIndex++;
            if (colorCycleIndex >= shuffledColorList.Count) {
                ShuffleColors(shuffledColorList);
                colorCycleIndex = 0;
            }

            currentColor = shuffledColorList[colorCycleIndex];
            ApplyColorToRecentTiles(currentColor);
        }
    }

    void SetupFixedColors() {
        tileColors = new Color[] {
            Color.red,
            Color.green,
            Color.grey,
            Color.white,
            Color.magenta,
            new Color(1f, 0.5f, 0f),           // orange
            new Color(0.5f, 0f, 1f),           // purple
            new Color(0f, 0.75f, 0.75f),       // teal
            new Color(0.306f, 0.121f, 0f),     // brown
        };
    }

    public void AddTiles(int amount) {
        for (int i = 0; i < amount; i++) {
            GameObject left = Instantiate(tilePrefabs[0], tileParent);
            GameObject top = Instantiate(tilePrefabs[1], tileParent);
            left.name = "LeftTile";
            top.name = "TopTile";
            left.SetActive(false);
            top.SetActive(false);
            leftTiles.Push(left);
            topTiles.Push(top);
        }
    }

    public void CreateTiles() {
        if (leftTiles.Count == 0 || topTiles.Count == 0) { // Check if the stacks are empty
            AddTiles(10);// Add more tiles if the stacks are empty
        }

        int randomIndex = Random.Range(0, tilePrefabs.Length);
        
        // Make sure there isn't a chance for more than 10 times of the same tile
        if (leftTileCount > 9) {
            randomIndex = 1;
            leftTileCount = 0;
        } else if (topTileCount > 9) {
            randomIndex = 0;
            topTileCount = 0;
        }

        GameObject newTile = null;

        if (randomIndex == 0) {
            newTile = leftTiles.Pop();
            newTile.SetActive(true);
            newTile.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = newTile;
            leftTileCount++;
        } else if (randomIndex == 1) {
            newTile = topTiles.Pop();
            newTile.SetActive(true);
            newTile.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = newTile;
            topTileCount++;
        }

        // Apply latest color to new tile if score >= 50
        int currentScore = PlayerScript.Instance.GetScore();
        if (currentScore >= 50) {
            Renderer tileRenderer = newTile.transform.GetChild(0).GetComponent<Renderer>();
            if (tileRenderer != null) {
                tileRenderer.material = new Material(tileRenderer.material);
                StartCoroutine(LerpColor(tileRenderer.material, currentColor, 0.5f));
            }
        }

        // Create tiles with gems on it
        int createGem = Random.Range(0, 7);
        if (createGem == 0) {
            currentTile.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void ApplyColorToRecentTiles(Color color) {
        if (tileParent == null) {
            Debug.LogError("[TILE MANAGER] tileParent not assigned!");
            return;
        }

        foreach (Transform tile in tileParent) {
            if (!tile.gameObject.activeInHierarchy) continue;

            if (tile.childCount > 0) {
                Transform meshHolder = tile.GetChild(0);

                Renderer tileRenderer = meshHolder.GetComponent<Renderer>();
                if (tileRenderer != null) {
                    tileRenderer.material = new Material(tileRenderer.material);
                    StartCoroutine(LerpColor(tileRenderer.material, color, 0.5f));
                }
            }
        }
    }

    private IEnumerator LerpColor(Material mat, Color targetColor, float duration) {
        Color startColor = mat.color;
        float time = 0f;

        while (time < duration) {
            mat.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        mat.color = targetColor;
    }

    private void ShuffleColors(List<Color> list) {
        for (int i = 0; i < list.Count; i++) {
            Color temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
