using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private const string MouseXInputName = "Mouse X";
    private const string MouseYInputName = "Mouse Y";
    
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform headTransform;
    [SerializeField] private float headTurnLimit;

    private float _headTurn;

    private float HeadTurn
    {
        get => _headTurn;
        set
        {
            if (Mathf.Abs(value) > headTurnLimit) return;
            _headTurn = Mathf.Clamp(value, -90, 90);
        }
    }

    private void Update()
    {
        float mouseXInput = Input.GetAxis(MouseXInputName);
        float mouseYInput = Input.GetAxis(MouseYInputName);

        float currentTurn = mouseXInput * sensitivity * Time.deltaTime;
        float currentHeadTurn = mouseYInput * sensitivity * Time.deltaTime;
        HeadTurn -= currentHeadTurn;

#if UNITY_EDITOR
        /*Debug.Log($"currentHeadTurn = {currentHeadTurn}", this);
        Debug.Log($"HeadTurn = {HeadTurn}", this);*/
#endif
        
        playerTransform.Rotate(Vector3.up * currentTurn);
        headTransform.localRotation = Quaternion.Euler(HeadTurn, 0, 0);
    }
}