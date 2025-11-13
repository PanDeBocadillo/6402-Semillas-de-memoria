using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ElementValueHandler : MonoBehaviour
{
    [SerializeField] private Button actionButton; // The button to link the functionality
    private string elementValue; // The element value to handle

    private void Start()
    {
        if (actionButton != null)
        {
            // Add a listener to the button to handle the click event
            actionButton.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogWarning("Action button is not assigned in ElementValueHandler.");
        }
    }

    /// <summary>
    /// Sets the element value to be handled by this script.
    /// </summary>
    /// <param name="value">The element value to set.</param>
    public void SetElementValue(string value)
    {
        elementValue = value;
        Debug.Log($"Element value set to: {elementValue}");
    }

    /// <summary>
    /// Handles the button click event.
    /// </summary>
    private void OnButtonClick()
    {

       if (!string.IsNullOrEmpty(elementValue))
        {
            Debug.Log($"Button clicked! Opening URL: {elementValue}");
            Application.OpenURL(elementValue); // Open the web page with the element value
        }
        //else
        {
            Debug.LogWarning("Element value is not set or is empty.");
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