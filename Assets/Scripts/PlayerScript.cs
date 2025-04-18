using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameInput gameInput;

    // Update is called once per frame
    void Update()
    {
        // Get the input from the GameInput class
        Vector3 moveDir = gameInput.GetMovementVector();
        float moveAmount = speed * Time.deltaTime;
        transform.Translate(moveDir * moveAmount);
    }
}
