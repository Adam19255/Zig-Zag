using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader{

    public enum Scene {
        MainMenuScene,
        GameScene,
        ShopScene,
        SettingsScene,
        LoadingScene,
    }
    private static Scene targetScene;

    public static void Load(Scene targetScene) {
        // Set the target scene to the one passed in
        Loader.targetScene = targetScene;
        // Start the loading process
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback() {
        // This method is called when the loading scene is loaded
        // Load the target scene
        SceneManager.LoadScene(targetScene.ToString());
    }
}
