using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBaseEventsLayer : MonoBehaviour
{
    public LayerMove LayerMove;

    public void SetActiveFalse()
    {
        LayerMove.gameObject.SetActive(false);
    }
}
