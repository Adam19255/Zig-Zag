using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour{
    private float fallDelay = 0.8f;

    // Make the tile fall after we have passed it
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {

            // Access the child component and set its BoxCollider's isTrigger to true so the tile will fall faster
            Transform child = transform.GetChild(0);
            if (child != null) {
                BoxCollider childCollider = child.GetComponent<BoxCollider>();
                if (childCollider != null) {
                    childCollider.isTrigger = true; // Enable trigger after the player passes
                }
            }

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

        // Access the child component and set its BoxCollider's isTrigger to false so the tile will not fall when recycled
        Transform child = transform.GetChild(0);
        if (child != null) {
            BoxCollider childCollider = child.GetComponent<BoxCollider>();
            if (childCollider != null) {
                childCollider.isTrigger = false; // Disable trigger when storing in the stack
            }
        }

        switch (gameObject.name) {
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
