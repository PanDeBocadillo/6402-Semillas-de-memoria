using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class FmodParameterHandler : MonoBehaviour
{
    [SerializeField] private Button actionButton; // Button to trigger the parameter change
    [SerializeField] private string parameterName; // Name of the FMOD parameter
    [SerializeField] private float value1 = 0f; // First value of the parameter
    [SerializeField] private float value2 = 1f; // Second value of the parameter
    [SerializeField] private StudioEventEmitter eventEmitter; // FMOD Event Emitter

    private bool isValue1 = true; // Tracks which value is currently set

    private void Start()
    {
        if (actionButton != null)
        {
            // Add a listener to the button to handle the click event
            actionButton.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogWarning("Action button is not assigned in FmodParameterHandler.");
        }

        if (eventEmitter == null)
        {
            Debug.LogWarning("FMOD Event Emitter is not assigned in FmodParameterHandler.");
        }
    }

    /// <summary>
    /// Handles the button click event to toggle the FMOD parameter.
    /// </summary>
    private void OnButtonClick()
    {
        if (eventEmitter != null && !string.IsNullOrEmpty(parameterName))
        {
            float newValue = isValue1 ? value2 : value1; // Toggle between value1 and value2
            eventEmitter.SetParameter(parameterName, newValue);
            Debug.Log($"FMOD Parameter '{parameterName}' set to {newValue}");
            isValue1 = !isValue1; // Toggle the state
        }
        else
        {
            Debug.LogWarning("FMOD Event Emitter or parameter name is not properly assigned.");
        }
    }

    private void OnDestroy()
    {
        // Remove the listener when the object is destroyed to avoid memory leaks
        if (actionButton != null)
        {
            actionButton.onClick.RemoveListener(OnButtonClick);
        }
    }
}
