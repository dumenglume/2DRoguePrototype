using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Input
    PlayerInput playerInput;
    float inputX, inputY;

    // Movement
    PlayerMovement playerMovement;

    // Health / Stress / Sanity
    PlayerHealth playerHealth;

    void Awake() 
    {
        playerInput    = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth   = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        InputMovement();
    }

    void InputMovement()
    {
        inputX = playerInput.InputX;
        inputY = playerInput.InputY;

        Vector2 inputDirection = new Vector2(inputX, inputY);

        bool inputIsDetected = inputDirection.x != 0 || inputDirection.y != 0;

        if (inputIsDetected && !playerMovement.IsMoving)
        {
            Debug.Log("Input detected: " + inputDirection);

            playerMovement.AttemptToMove(inputDirection);
        }
    }
}
