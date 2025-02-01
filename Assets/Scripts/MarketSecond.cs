using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarketSecond : MonoBehaviour
{
    public void AddItem() {
        SimpleEventController.simpleEventController.AddItemEvent();
    }
}
