using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using UnityEngine;

public class UpgradeBoostItemModel : IItem 
{
    public int Id { get; set; }
    public int LvlToUnlock { get; set; }
    public int IdToUnlock { get; set; }
    public int Price { get; set; }
    public int PriceSpecialMoney { get; set; }
    public int CountEffect { get; set; }
    public int TimeEffectSecond { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public string InternalName { get; set; }

    public int UserCount { get; set; }


    public static implicit operator SaveUpgradeBoostItemModel(UpgradeBoostItemModel Item) 
    {
        return new SaveUpgradeBoostItemModel { Id = Item.Id };
    }

    public int GetId()
    {
        return Id;
    }
}
