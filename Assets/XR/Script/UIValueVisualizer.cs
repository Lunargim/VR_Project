using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIValueVisualizer : MonoBehaviour
{
   public Slider triggerValueText;
   public Slider gripValueText;
   public Toggle primaryValueToggle;
   public Toggle secondaryValueToggle;
   public TextMeshProUGUI thumbstickValueText;
   public TextMeshProUGUI menuValueText;
    
   public OculusInputCacheValue cacheValues;



   public void Update()
   {
      UpdateTriggerValue();
      UpdateGripValue();
      UpdatePrimaryValue();
      UpdateSecondaryValue();
      UpdateMenuValue();
      UpdateThumbstickValue();
      
   }

   private void UpdateThumbstickValue()
   {
      var tSValue = cacheValues.thumbstickValue;
      thumbstickValueText.text = tSValue.ToString();
   }

   private void UpdateMenuValue()
   {
      var menuValue = cacheValues.menuButtonPressed;
      menuValueText.text = menuValue.ToString();
   }

   private void UpdateSecondaryValue()
   {
      var secondaryValue = cacheValues.secondaryButtonPressed;
      secondaryValueToggle.isOn = secondaryValue;
   }

   private void UpdatePrimaryValue()
   {
      var primaryValue = cacheValues.primaryButtonPressed;
      primaryValueToggle.isOn = primaryValue;
   }

   private void UpdateGripValue()
   {
      var gripValue = cacheValues.gripValue;
      gripValueText.value = gripValue;
   }

   private void UpdateTriggerValue()
   {
      var triggerValue = cacheValues.triggerValue;
      triggerValueText.value = triggerValue;
   }
}
