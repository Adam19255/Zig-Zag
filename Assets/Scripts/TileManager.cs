using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileManager : MonoBehaviour{

    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject currentTile;
    [SerializeField] private GameObject startTile;

    private Stack<GameObject> leftTiles = new Stack<GameObject>();
    private Stack<GameObject> topTiles = new Stack<GameObject>();
    private Stack<GameObject> startTiles = new Stack<GameObject>();

    private static TileManager instance;
    private int leftTileCount = 0;
    private int topTileCount = 0;


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

    // Start is called before the first frame update
    void Start()
    {
        AddTiles(50); // Add 50 tiles to the stack

        // Add the startTile to the stack
        startTiles.Push(startTile);

        for (int i = 0; i < 25; i++) { // Create 25 tiles at the start
            CreateTiles();
        }
    }

    // Function to add tiles to the stack to recycle them
    public void AddTiles(int amount) {
        for (int i = 0; i < amount; i++) {
            leftTiles.Push(Instantiate(tilePrefabs[0]));
            topTiles.Push(Instantiate(tilePrefabs[1]));
            leftTiles.Peek().name = "LeftTile"; // Set the name of the tile to "LeftTile" in the stack
            leftTiles.Peek().SetActive(false); // Deactivate the tile so it doesn't appear in the scene
            topTiles.Peek().name = "TopTile";
            topTiles.Peek().SetActive(false);
        }
    }

    public void CreateTiles() {
        if (leftTiles.Count == 0 || topTiles.Count == 0) { // Check if the stacks are empty
            AddTiles(10); // Add more tiles if the stacks are empty
        }
        int randomIndex = Random.Range(0, tilePrefabs.Length);

        // Make sure there isn't a chance for more than 10 times of the same tile
        if (leftTileCount > 9) {
            randomIndex = 1;
            leftTileCount = 0;
        }
        else if (topTileCount > 9) {
            randomIndex = 0;
            topTileCount = 0;
        }

        // Create left and top tiles
        if (randomIndex == 0) {
            GameObject newTile = leftTiles.Pop();
            newTile.SetActive(true);
            newTile.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = newTile;
            leftTileCount++;
        }
        else if (randomIndex == 1) {
            GameObject newTile = topTiles.Pop();
            newTile.SetActive(true);
            newTile.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = newTile;
            topTileCount++;
        }

        // Create tiles with gems on it
        int createGem = Random.Range(0, 7);
        if (createGem == 0) {
            currentTile.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
