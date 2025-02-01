using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;

public class UpgradeGameItemModel : IItem 
{
    public int Id { get; set; }

    public int LvlToUnlock { get; set; }

    public int Price { get; set; }

    public int PriceSpecialMoney { get; set; }

    public int TimeToSearch { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int GetId() 
    {
        return Id;
    }
    public static implicit operator SaveUpgradeGameItemModel(UpgradeGameItemModel item)
    {
        return new SaveUpgradeGameItemModel { Id = item.Id };
    }
}
