using UnityEngine;

public class ResetDontDestroyOnLoadData : MonoBehaviour
{
    // This method is called when the script's GameObject is awakened
    private void Awake()
    {
        // Check if an instance of GameControlsManager exists
        if (GameControlsManager.Instance != null)
        {
            // Call the ResetDontDestroyOnLoadData method on the GameControlsManager instance
            GameControlsManager.Instance.ResetDontDestroyOnLoadData();
        }
    }
}
