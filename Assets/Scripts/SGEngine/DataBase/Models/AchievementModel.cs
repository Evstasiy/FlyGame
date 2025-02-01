using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;

public class AchievementModel : IItem
{
    public int Id { get; set; }
    public string IconPath { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public int UserCount { get; set; }


    public static implicit operator SaveAchievementModel(AchievementModel Item)
    {
        return new SaveAchievementModel { Id = Item.Id };
    }
    public static implicit operator SaveItemXML(AchievementModel Item)
    {
        return new SaveItemXML { Id = Item.Id };
    }
    
    public static explicit operator AchievementModel(AchivmentXml  Item)
    {
        return new AchievementModel { Id = Item.Id };
    }

    public int GetId()
    {
        return Id;
    }
}
