using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers.GameItemInformation.GameWorker;
using System.Collections.Generic;

namespace Assets.Scripts.SGEngine.DataBase.Models
{
    public class GameParametersModel
    {
        public UiItemsModel UiItemsModel { get; set; }

        public WorldObjectsModel WorldObjects { get; set; }

        public UpgradesModel Upgrades { get; set; }
    }

    public class WorldObjectsModel
    {
        public ItemsModel Items { get; set; }
        public AchievementsItemsModel AchievementsItemsModel { get; set; }
        public SkinsModel SkinsModel { get; set; }
    }
    
    public class ItemsModel
    {
        public List<GameItemModel> GameItems { get; set; }
    }
    
    public class AchievementsItemsModel
    {
        public List<AchievementModel> AchievementItems { get; set; }
    }
    
    public class SkinsModel
    {
        public List<SkinModel> SkinsItems { get; set; }
    }

    public class UiItemsModel
    {
        public List<UIItem> UIItems { get; set; }
    }
    
    public class UpgradesModel
    {
        public List<UpgradeGameItemModel> UpgradeGameItems { get; set; }

        public List<UpgradeBoostItemModel> UpdateBoostItems { get; set; }

        public List<BoostPlayerItemModel> BoostPlayerItems { get; set; }
    }

}
