using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUnlockScene : MonoBehaviour
{
    public SceneActor sceneActor;
    void Start()
    {
        
    }
    public void OnClick() {
        sceneActor.UnlockScene();

    }
    public void OnClickLock() {
        sceneActor.LockScene();

    }
}
