using Assets.Scripts.MainGame.Models;
using UnityEngine;


[CreateAssetMenu(fileName = "BiomWorldModel", menuName = "ScriptableObjects/BiomWorldSettings", order = 1)]
public class BiomWorldModel : ScriptableObject
{
    public BiomsEnum BiomName;
    public BackgrounLayerInfo BackgrounLayerInfo;
    public float BiomIndex = 0;
    public bool IsActive = true;
    public InteractiveObjectModel[] InteractiveObjectsInZone;
}
