using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class OculusInputCacheValue : MonoBehaviour
{
   public InputAction menuButton;
   public InputAction triggerButton;
   public InputAction gripButton;
   public InputAction secondayButton;
   public InputAction primaryButton;
   public InputAction thumbstickButton;
   
   
   public float triggerValue {get; private set;}
   public float gripValue { get; private set; }
   public Vector2 thumbstickValue {get; private set;}
   public bool primaryButtonPressed { get;  private set; }
   public bool secondaryButtonPressed { get; private set; }
   public bool menuButtonPressed { get; private set; }

   public void Awake()
   {
      BindAction(triggerButton, OnTriggerPerformed, OnTriggerCancelled);
      BindAction(gripButton, OnGripPerformed, OnGripCancelled);
      BindAction(thumbstickButton, OnThumbstickPerformed, OnThumbstickCancelled);
      BindAction(primaryButton, OnPrimaryPerformed, OnPrimaryCancelled);
      BindAction(secondayButton, OnSecondaryPerformed, OnSecondaryCancelled);
      BindAction(menuButton, OnMenuPerformed, OnMenuCancelled);
   }

   private void OnMenuCancelled(InputAction.CallbackContext obj)
   {
      menuButtonPressed = false;
   }

   private void OnMenuPerformed(InputAction.CallbackContext obj)
   {
      menuButtonPressed = true;
   }

   private void OnSecondaryCancelled(InputAction.CallbackContext obj)
   {
      secondaryButtonPressed = false;
   }

   private void OnSecondaryPerformed(InputAction.CallbackContext obj)
   {
      secondaryButtonPressed = true;
   }

   private void OnPrimaryCancelled(InputAction.CallbackContext obj)
   {
      primaryButtonPressed = false;
   }

   private void OnPrimaryPerformed(InputAction.CallbackContext obj)
   {
      primaryButtonPressed = true;
   }

   private void OnThumbstickCancelled(InputAction.CallbackContext obj)
   {
      thumbstickValue = Vector2.zero;
   }

   private void OnThumbstickPerformed(InputAction.CallbackContext obj)
   {
      thumbstickValue = obj.ReadValue<Vector2>();
   }

   private void OnGripCancelled(InputAction.CallbackContext obj)
   {
      gripValue = 0;
   }

   private void OnGripPerformed(InputAction.CallbackContext obj)
   {
      gripValue =  obj.ReadValue<float>();
   }

   private void OnTriggerCancelled(InputAction.CallbackContext obj)
   {
      triggerValue = 0;
   }

   public void BindAction(InputAction action, Action<InputAction.CallbackContext> performed,
      Action<InputAction.CallbackContext> cancelled)
   {
      if (action != null)
      {
         action.performed += performed;
         action.canceled += cancelled;
         action.Enable();
      }
   }

   public void OnTriggerPerformed(InputAction.CallbackContext obj)
   {
      triggerValue = obj.ReadValue<float>();
   }
   
}
