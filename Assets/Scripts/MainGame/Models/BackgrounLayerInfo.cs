using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundSettings", menuName = "ScriptableObjects/BackgroundSettings", order = 1)]
public class BackgrounLayerInfo : ScriptableObject
{
    public LayerMove BackgrounObject;
    public float DestroyPositionX;
    public bool CreateNewLayer = true;
}
