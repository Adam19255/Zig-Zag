using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject currentTile;
    [SerializeField] private GameObject startTile;
    private static TileManager instance;
    private Stack<GameObject> leftTiles = new Stack<GameObject>();
    private Stack<GameObject> topTiles = new Stack<GameObject>();
    private Stack<GameObject> startTiles = new Stack<GameObject>();
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
        AddTiles(50);

        // Add the startTile to the stack
        startTiles.Push(startTile);

        for (int i = 0; i < 25; i++) {
            CreateTiles();
        }
    }

    public void AddTiles(int amount) {
        for (int i = 0; i < amount; i++) {
            leftTiles.Push(Instantiate(tilePrefabs[0]));
            topTiles.Push(Instantiate(tilePrefabs[1]));
            leftTiles.Peek().name = "LeftTile";
            leftTiles.Peek().SetActive(false);
            topTiles.Peek().name = "TopTile";
            topTiles.Peek().SetActive(false);
        }
    }

    public void CreateTiles() {
        if (leftTiles.Count == 0 || topTiles.Count == 0) {
            AddTiles(10);
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
