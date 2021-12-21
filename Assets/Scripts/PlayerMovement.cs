using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string HorizontalAxisInputName = "Horizontal";
    private const string VerticalAxisInputName = "Vertical";

    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed;
    [SerializeField] private Transform playerTransform;
    
    private void Update()
    {
        float horizontalInput = Input.GetAxis(HorizontalAxisInputName);
        float verticalInput = Input.GetAxis(VerticalAxisInputName);

        float horizontalMovement = horizontalInput * speed * Time.deltaTime;
        float verticalMovement = verticalInput * speed * Time.deltaTime;

        Vector3 movementVector = playerTransform.forward * verticalMovement + playerTransform.right * horizontalMovement;

#if UNITY_EDITOR
        Debug.Log($"Movement vector - {movementVector}", this);
#endif

        characterController.Move(movementVector);
    }
}
