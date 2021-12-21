using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float gravity;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    private float yVelocity;
    
    private void Update()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && yVelocity < 0) yVelocity = -2;

        yVelocity += gravity * Time.deltaTime;

        characterController.Move(new Vector3(0, yVelocity * Time.deltaTime, 0));
    }
}