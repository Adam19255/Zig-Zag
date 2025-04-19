using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject currentTile;
    private static TileManager instance;

    public static TileManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<TileManager>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < 50; i++) {
            CreateTile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTile() {
        int randomIndex = Random.Range(0, tilePrefabs.Length);
        GameObject newTile = Instantiate(tilePrefabs[randomIndex], currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position, Quaternion.identity);
        currentTile = newTile;
    }
}
