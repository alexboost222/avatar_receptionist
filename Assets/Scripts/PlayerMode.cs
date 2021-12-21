using System;
using UnityEngine;

public class PlayerMode : MonoBehaviour
{
    private const KeyCode ChangeModeKeyCode = KeyCode.E;
    
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private PlayerGravity playerGravity;
    [SerializeField] private Canvas overlayCanvas;

    private bool _isAnswerMode;

    private bool IsAnswerMode
    {
        get => _isAnswerMode;
        set
        {
            playerMovement.enabled = !value;
            playerLook.enabled = !value;
            playerGravity.enabled = !value;
            
            Cursor.lockState = value ? CursorLockMode.Confined : CursorLockMode.Locked;
            
            overlayCanvas.enabled = value;
            
            _isAnswerMode = value;
        }
    }

    private void Awake() => IsAnswerMode = false;

    private void Update()
    {
        if (!Input.GetKeyDown(ChangeModeKeyCode)) return;

        IsAnswerMode = !IsAnswerMode;
    }
}