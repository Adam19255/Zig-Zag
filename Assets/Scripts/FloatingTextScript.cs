using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextScript : MonoBehaviour {
    private float destroyTime = 2f; // Time in seconds before the text is destroyed
    private Vector3 Offset = new Vector3(0, 2, 0); // Offset to position the text above the object
    private TextMeshPro textMeshPro; // Reference to the TextMeshPro component

    // Start is called before the first frame update
    void Start() {
        textMeshPro = GetComponent<TextMeshPro>();

        if (textMeshPro != null) {
            Color color = textMeshPro.color;
            color.a = 1f; // Start fully visible
            textMeshPro.color = color;
        }

        StartCoroutine(FadeOutText()); // Start fading out the text
        Destroy(gameObject, destroyTime); // Destroy the text object after the specified time
        transform.position = transform.position + Offset; // Set the position of the text above the object
    }


    private IEnumerator FadeOutText() {
        if (textMeshPro != null) {
            Color color = textMeshPro.color;
            while (color.a > 0) {
                color.a -= Time.deltaTime / destroyTime; // Gradually reduce opacity
                textMeshPro.color = color;
                yield return null;
            }
        }
    }

}
