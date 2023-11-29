using UnityEngine.SceneManagement;

// A static class responsible for managing scene loading
public static class Loader
{
    // Enumeration representing different scenes in the game
    public enum Scene
    {
        MainMenuScene,
        LobbyScene,
        GameScene,
        LoadingScene
    }

    // The target scene to be loaded
    private static Scene targetScene;

    // Method to initiate loading of a specific scene
    public static void Load(Scene targetScene)
    {
        // Set the target scene
        Loader.targetScene = targetScene;

        // Load the LoadingScene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    // Callback method called from the LoadingScene to load the target scene
    public static void LoaderCallback()
    {
        // Load the target scene
        SceneManager.LoadScene(targetScene.ToString());
    }
}
