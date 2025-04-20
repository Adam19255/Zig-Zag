using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameInput gameInput;
    private Rigidbody rb;
    private float customGravity = -100f;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable default gravity
    }

    private void FixedUpdate() {
        // Apply custom gravity
        rb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update() {
        // Get the input from the GameInput class
        Vector3 moveDir = gameInput.GetMovementVector();
        float moveAmount = speed * Time.deltaTime;
        transform.Translate(moveDir * moveAmount);

        // Get the first child of the player object
        Transform child = transform.GetChild(0);
        if (child != null) {

            // Rotate the player based on the movement direction
            if (moveDir == Vector3.forward) {
                child.Rotate(Vector3.left, -270f * Time.deltaTime);
            }
            else if (moveDir == Vector3.left) {
                child.Rotate(Vector3.forward, -270f * Time.deltaTime);
            }
        }
    }
}
