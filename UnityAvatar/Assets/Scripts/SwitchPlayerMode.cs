using System;
using UnityEngine;

[DisallowMultipleComponent]
public class SwitchPlayerMode : MonoBehaviour
{
    [SerializeField] private Canvas dialogCanvas;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private LookWithMouse lookWithMouse;
    
    private bool _isDialogMode;

    private bool IsDialogMode
    {
        get => _isDialogMode;
        set
        {
            if (_isDialogMode == value) return;

            _isDialogMode = value;

            if (_isDialogMode)
            {
                dialogCanvas.enabled = true;
                playerMovement.enabled = false;
                lookWithMouse.enabled = false;
            }
            else
            {
                dialogCanvas.enabled = false;
                playerMovement.enabled = true;
                lookWithMouse.enabled = true;
            }
        }
    }

    public void Switch()
    {
        IsDialogMode = !IsDialogMode;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Switch();
    }
}