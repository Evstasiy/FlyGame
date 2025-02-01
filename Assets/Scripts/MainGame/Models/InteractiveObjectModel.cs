using UnityEngine;

[CreateAssetMenu(fileName = "InteractiveObject", menuName = "ScriptableObjects/InteractiveObject", order = 1)]
public class InteractiveObjectModel : ScriptableObject
{
    public InteractiveObjectEnum ObjectType;
    public GameObject Object;
    public Sprite IconImg;
    public bool IsVisibleInPlayerIndicator;
    public float MinSpeed;
    public float MaxSpeed;
    public float TimeToDestroySec;
    public float DestroyPositionX;
    public bool IsSpawnInBack;
    public int SpawnChancePercent;

    public int MinSpawnCount;
    public int MaxSpawnCount;
    public MassSpawnItemsTypes SpawnType;

    public EffectObjectModel[] Effects;

    public ItemObjectModel ContaiterWithItems;
}
