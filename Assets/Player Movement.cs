using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
// Reference to the input action property for movement
    public InputActionProperty moveInput; 

    // Speed of the player's movement
    public float moveSpeed = 2.0f;

    // Reference to the CharacterController component
    private CharacterController characterController;

    // Gravity to keep the player grounded
    private float gravity = -9.81f;
    private Vector3 velocity;
    void Start()
    {
        // Get the CharacterController attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input from the left joystick (Primary2DAxis)
        Vector2 inputVector = moveInput.action.ReadValue<Vector2>();

        // Convert input into a 3D movement vector (X and Z plane)
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        // Align movement with the camera's forward direction
        if (Camera.main != null)
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            forward.y = 0; // Keep movement horizontal
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            moveDirection = forward * moveDirection.z + right * moveDirection.x;
        }

        // Move the CharacterController
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Apply gravity
        if (!characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
        else
        {
            velocity.y = 0; // Reset vertical velocity when grounded
        }
    }
}
