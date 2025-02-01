using System.Collections.Generic;

namespace Assets.Scripts.SGEngine.DataBase.Models
{
    [System.Serializable]
    public class SaveGameInformationModel
    {
        public PlayerInformationModel PlayerInformation;
        public SaveWorldObjectsModel SaveWorldObjects ;
        public SaveUpgradesModel SaveUpgrades ;

        public SaveGameInformationModel() 
        {
            PlayerInformation = new PlayerInformationModel()
            {
                PlayerFeature = new PlayerFeatureModel(),
                PlayerOptions = new PlayerOptionsModel()
            };
            SaveWorldObjects = new SaveWorldObjectsModel()
            {
                SaveItems = new SaveItemsModel()
                {
                    SaveItems = new List<SaveGameItemModel>()
                },
                SaveAchievementsModel = new SaveAchievementsModel()
                {
                    SaveAchievements = new List<SaveAchievementModel>()
                },
                SaveSkinsModel = new SaveSkinsModel()
                {
                    SaveSkins = new List<SaveSkinModel>()
                }
                
            };
            SaveUpgrades = new SaveUpgradesModel()
            {
                SaveUpgradeGameItem = new List<SaveUpgradeGameItemModel>(),
                SaveBoostItems = new List<SaveBoostItemModel>(),
                SaveUpgradeBoostItems = new List<SaveUpgradeBoostItemModel>()
            };
        }
    }

    [System.Serializable]
    public class SaveWorldObjectsModel
    {
        public SaveItemsModel SaveItems ;
        public SaveAchievementsModel SaveAchievementsModel ;
        public SaveSkinsModel SaveSkinsModel ;
    }

    [System.Serializable]
    public class SaveUpgradesModel
    {
        public List<SaveUpgradeGameItemModel> SaveUpgradeGameItem ;
        public List<SaveBoostItemModel> SaveBoostItems ;
        public List<SaveUpgradeBoostItemModel> SaveUpgradeBoostItems ;
    }

    [System.Serializable]
    public class SaveItemsModel
    {
        public List<SaveGameItemModel> SaveItems ;
    }

    [System.Serializable]
    public class SaveAchievementsModel
    {
        public List<SaveAchievementModel> SaveAchievements ;
    }

    [System.Serializable]
    public class SaveSkinsModel
    {
        public List<SaveSkinModel> SaveSkins ;
    }

    [System.Serializable]
    public class SaveUpgradeGameItemModel
    {
        public int Id ;
    }

    [System.Serializable]
    public class SaveBoostItemModel
    {
        public int Id ;
        public int UserCount ;
    }

    [System.Serializable]
    public class SaveGameItemModel
    {
        public int Id ;
        public bool IsLock ;
    }

    [System.Serializable]
    public class SaveAchievementModel
    {
        public int Id ;
    }

    [System.Serializable]
    public class SaveSkinModel
    {
        public int Id ;
    }

    [System.Serializable]
    public class SaveUpgradeBoostItemModel
    {
        public int Id ;
    }
}
