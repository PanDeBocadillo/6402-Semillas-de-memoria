using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Pause the game when this script is active
        PauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Pauses the game by setting time scale to 0
    void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Paused");
    }

    // Resumes the game by setting time scale to 1
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Game Resumed");
    }
}
