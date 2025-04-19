using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    [SerializeField] private GameObject leftTilePrefab;
    [SerializeField] private GameObject topTilePrefab;
    [SerializeField] private GameObject currentTile;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++) {
            CreateTile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTile() {
        GameObject newTile = Instantiate(topTilePrefab, currentTile.transform.GetChild(0).transform.GetChild(1).position, Quaternion.identity);
        currentTile = newTile;
    }
}
