using UnityEngine;

[DisallowMultipleComponent]
public class SwitchPlayerMode : MonoBehaviour
{
    [SerializeField] private Canvas dialogCanvas;
    [SerializeField] private CharacterController characterController;
    public enum PlayerMode
    {
        Walk,
        Dialog
    }

}