using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEventController : MonoBehaviour
{
    public static SimpleEventController simpleEventController;
    void Start()
    {
        simpleEventController = this;
    }

    public event Action onAddItemEvent;
    public void AddItemEvent() {
        onAddItemEvent();
    }

}
