using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private float fallDelay = 0.8f;

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            TileManager.Instance.CreateTiles();
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall() {
        // Make the tile fall after we have passed it
        yield return new WaitForSeconds(fallDelay);
        GetComponent<Rigidbody>().isKinematic = false;
        // Resycle the tile after 2 seconds
        yield return new WaitForSeconds(2);
        switch(gameObject.name) {
            case "LeftTile":
                TileManager.Instance.LeftTiles.Push(gameObject);
                break;
            case "TopTile":
                TileManager.Instance.TopTiles.Push(gameObject);
                break;
            case "StartTile":
                TileManager.Instance.StartTiles.Push(gameObject);
                break;
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.SetActive(false);
    }
}
