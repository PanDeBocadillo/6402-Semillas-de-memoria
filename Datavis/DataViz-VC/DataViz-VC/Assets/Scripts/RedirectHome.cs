using UnityEngine;

public class RedirectHome : MonoBehaviour
{
    [SerializeField] private string url = "https://juanduran2023.github.io/6402-/"; // URL to redirect to

    // Method to be called when the button is clicked
    public void RedirectToWebPage()
    {
        if (!string.IsNullOrEmpty(url))
        {
            Debug.Log($"Redirecting to: {url}");
            Application.OpenURL(url); // Open the URL in the default web browser
        }
        else
        {
            Debug.LogWarning("URL is empty or null. Cannot redirect.");
        }
    }
}
