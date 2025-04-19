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
    void Update()
    {
        // Get the input from the GameInput class
        Vector3 moveDir = gameInput.GetMovementVector();
        float moveAmount = speed * Time.deltaTime;
        transform.Translate(moveDir * moveAmount);
    }
}
