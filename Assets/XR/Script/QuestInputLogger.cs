using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestInputLogger : MonoBehaviour
{
    public InputAction menuButton;
    public InputAction triggerButton;
    public InputAction gripButton;
    public InputAction secondayButton;
    public InputAction primaryButton;
    public InputAction thumbstickButton;

    void Awake()
    {
        BindAction(triggerButton);
        BindAction(gripButton);
        BindAction(thumbstickButton);
        BindAction(primaryButton);
        BindAction(secondayButton);
        BindAction(menuButton);
    }

    private void BindAction(InputAction action)
    {
        if (action != null)
        {
            action.performed += LogInput;
            action.canceled += LogInput;
            action.Enable();
        }
    }

    private void UnbindAction(InputAction action)
    {
        if (action != null)
        {
            action.performed -= LogInput;
            action.canceled -= LogInput;
            action.Disable();
        }
    }

    private void OnDisable()
    {
        UnbindAction(triggerButton);
        UnbindAction(gripButton);
        UnbindAction(thumbstickButton);
        UnbindAction(primaryButton);
        UnbindAction(secondayButton);
        UnbindAction(menuButton);
    }

    private void LogInput(InputAction.CallbackContext context)
    {
        Debug.Log($"{context.action.name}: {context.ReadValueAsObject()}");
    }
}