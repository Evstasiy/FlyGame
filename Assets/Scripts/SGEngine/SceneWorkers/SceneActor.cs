using System;
using UnityEngine;

public class SceneActor : MonoBehaviour
{
    public SceneController sceneController;
    public string sceneName;

    [HideInInspector]
    public bool IsLoadScene;
    public void LoadScene() {
        if(IsLoadScene)
            sceneController.LoadScene(sceneName);
    }
    public void UnlockScene()
    {
        if (IsUnlockScene != null) 
        {
            IsUnlockScene();
            IsLoadScene = true;
        } 
        else 
        {
            Debug.LogError($"Scene {sceneName} has not UnlockScene binding");
        }
    }

    public void LockScene() {
        if (IsLockScene != null)
        {
            IsLockScene();
            IsLoadScene = false;
        } 
        else
        {
            Debug.LogError($"Scene {sceneName} has not LockScene binding");
        }
    }

    public event Action IsUnlockScene;
    public event Action IsLockScene;
}
