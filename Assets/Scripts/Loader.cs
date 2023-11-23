using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{

    // Enumeration defining different scenes to load
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    // Target scene to load
    private static Scene targetScene;

    // Static method to initiate the loading of a specified scene
    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;

        // Load the LoadingScene to start the transition
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    // Callback method called from the LoadingScene to load the previously specified target scene
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
