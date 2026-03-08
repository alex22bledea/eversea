
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomCameraInput : MonoBehaviour, AxisState.IInputAxisProvider
{
    [SerializeField] private InputActionReference lookAction;

    [Space(12)]
    [SerializeField] private FloatVariableSO mouseSensitivity;

    private bool canMove = false;

    public bool SetCanMove(bool value) => canMove = value;

    void OnEnable() => lookAction.action.Enable();
    void OnDisable() => lookAction.action.Disable();

    public float GetAxisValue(int axis)
    {
        if (!canMove)
            return 0f;

        Vector2 delta = lookAction.action.ReadValue<Vector2>();

        switch (axis)
        {
            case 0: return delta.x * mouseSensitivity.Value;
            case 1: return delta.y * mouseSensitivity.Value;
            case 2: return 0;
        }
        return 0;
    }
}
