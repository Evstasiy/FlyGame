using Assets.Scripts.MainGame.Models;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LayerSettings", menuName = "ScriptableObjects/LayerSettings", order = 1)]
public class LayerWorldModel : ScriptableObject
{
    public LayerEnum LayerName;
    public Vector3 SpawnPosition;
    public float SpeedBraking;
    public bool IsActiveLayer = false;
    public BiomWorldModel[] Bioms;
    public int DistanceToChangeBiom;

    public float SizeLayerYMax;
    public float SizeLayerYMin;
    public float SizeLayerWidth;
}
