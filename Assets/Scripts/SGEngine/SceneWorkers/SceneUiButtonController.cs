using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUiButtonController : MonoBehaviour
{
    public SceneActor sceneActor;
    Button button;
    public void SceneIsUnlock() {
        button.interactable = true;
    }
    public void SceneIsLock() {
        button.interactable = false;
    }

    private void Start()
    {
        button = this.GetComponent<Button>();
        button.interactable = sceneActor.IsLoadScene;
        sceneActor.IsUnlockScene += SceneIsUnlock;
        sceneActor.IsLockScene += SceneIsLock;
    }
}
