using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;

public class SkinModel : IItem
{
    public int Id { get; set; }
    public string IconPath { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PathToSkin { get; set; }
    public int Price { get; set; }
    public int PriceSpecialMoney { get; set; }
    public int LvlToUnlock { get; set; }

    //public int UserCount { get; set; }


    public static implicit operator SaveSkinModel(SkinModel Item)
    {
        return new SaveSkinModel { Id = Item.Id };
    }
    public static implicit operator SaveItemXML(SkinModel Item)
    {
        return new SaveItemXML { Id = Item.Id };
    }
    
    public static explicit operator SkinModel(SkinXML Item)
    {
        return new SkinModel { Id = Item.Id };
    }

    public int GetId()
    {
        return Id;
    }
}
