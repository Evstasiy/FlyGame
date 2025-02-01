using UnityEngine;

[CreateAssetMenu(fileName = "ItemObjectModel", menuName = "ScriptableObjects/ItemObjectModel", order = 1)]
public class ItemObjectModel : ScriptableObject
{
    public ItemData[] itemDatas;
}

[System.Serializable]
public class ItemData
{
    public Sprite Icon;
    public ItemDataEnum Key;
    public int Value;
}

public enum ItemDataEnum
{
    Coin,
    SpecialCoin,
    Speed
}
