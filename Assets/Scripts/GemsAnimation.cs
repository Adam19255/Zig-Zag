using UnityEngine;
using System.Collections;

public class GemsAnimation : MonoBehaviour {
    [SerializeField] private bool isRotating = false;
    [SerializeField] private bool rotateX = false;
    [SerializeField] private bool rotateY = false;
    [SerializeField] private bool rotateZ = false;
    [SerializeField] private float rotationSpeed = 90f; // Degrees per second

    void Update() {
        if (isRotating) {
            Vector3 rotationVector = new Vector3(
                rotateX ? 1 : 0,
                rotateY ? 1 : 0,
                rotateZ ? 1 : 0
            );
            transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
        }
    }
}

