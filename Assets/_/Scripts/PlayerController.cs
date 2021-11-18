using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionMap input;
    private InputAction moveAction;

    [SerializeField] private float rotationSpeed;
    private float direction;

    private void Start()
    {
        direction = 0.0f;
        
        moveAction = input.FindAction("Move");
        moveAction.started += OnMoving;
        moveAction.canceled += OnMoving; // Sets the direction vector to 0
        moveAction.Enable();
        input.Enable();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * direction * rotationSpeed);
    }

    private void OnMoving(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<float>();
    }
}
