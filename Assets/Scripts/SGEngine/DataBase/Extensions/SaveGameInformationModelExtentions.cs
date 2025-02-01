using Assets.Scripts.SGEngine.DataBase.Models;
using System.Linq;

namespace Assets.Scripts.SGEngine.DataBase.Extensions
{
    public static class SaveGameInformationModelExtentions
    {
        public static SaveGameInformation ConvertToXML(this SaveGameInformationModel model)
        {
            var saveItems = model.SaveWorldObjects.SaveItems.SaveItems
                .Select(x => new SaveItemXML()
                {
                    Id = x.Id,
                    isLock = x.IsLock
                }).ToList();
            
            var saveAchivmentItems = model.SaveWorldObjects.SaveAchievementsModel.SaveAchievements
                .Select(x => new SaveItemXML()
                {
                    Id = x.Id
                }).ToList();
            
            var saveSkins = model.SaveWorldObjects.SaveSkinsModel.SaveSkins
                .Select(x => new SaveItemXML()
                {
                    Id = x.Id
                }).ToList();
            
            var saveUpgradeGameItems = model.SaveUpgrades.SaveUpgradeGameItem
                .Select(x => new Save_UpdateNewObject()
                {
                    Id = x.Id
                }).ToList();

            var saveBoostItems = model.SaveUpgrades.SaveBoostItems
                .Select(x => new Save_BoostObject()
                {
                    Id = x.Id,
                    UserCount = x.UserCount
                }).ToList();
            
            var saveUpgradeBoostItems = model.SaveUpgrades.SaveUpgradeBoostItems
                .Select(x => new Save_UpdateBoostObject()
                {
                    Id = x.Id
                }).ToList();

            return new SaveGameInformation()
            {
                Save_PlayerFeature = new Save_PlayerFeatures()
                {
                    Save_PlayerFeatureBase = new Save_PlayerFeatureBase()
                    {
                        experience = model.PlayerInformation.PlayerFeature.Experience,
                        mainMoney = model.PlayerInformation.PlayerFeature.MainMoney,
                        specialMoney = model.PlayerInformation.PlayerFeature.SpecialMoney,
                        selectedSkinId = model.PlayerInformation.PlayerFeature.SelectedSkinId,
                        playerDistanceRecord = model.PlayerInformation.PlayerFeature.PlayerDistanceRecord
                    },
                    Save_PlayerFeatureOptions = new Save_PlayerFeatureOptions()
                    {
                        isAd = model.PlayerInformation.PlayerOptions.IsAd,
                        language = model.PlayerInformation.PlayerOptions.Language,
                        isMusic = model.PlayerInformation.PlayerOptions.IsMusic,
                        isFinishedTutorial = model.PlayerInformation.PlayerOptions.IsFinishedTutorial,
                    }
                },
                Save_WorldObjects = new Save_WorldObjects()
                {
                    Save_Items = new Save_Items()
                    {
                        SaveItemList = saveItems
                    },
                    Save_Achivments = new Save_Achivments()
                    {
                        SaveAchivmentItem = saveAchivmentItems
                    },
                    Save_Skins = new Save_Skins()
                    {
                        SaveSkinItem = saveSkins
                    }
                },
                Save_Upgrades = new Save_Upgrades()
                {
                     Save_Upgrade_Items = new Save_Upgrade_Items()
                     {
                         Save_New_Items = new Save_New_Items()
                         {
                             Save_UpdateNewOjectItem = saveUpgradeGameItems
                         },
                         Save_Boost_Items = new Save_Boost_Items()
                         {
                             Save_BoostObjectItem = saveBoostItems
                         },
                         Save_UpdateBoost_Items = new Save_UpdateBoost_Items()
                         {
                             Save_UpdateBoostObject = saveUpgradeBoostItems
                         }
                     }
                }
            };
        }
        
        public static SaveGameInformationModel XMLConvertToModel(this SaveGameInformation saveGameInfo)
        {
            var saveItems = saveGameInfo.Save_WorldObjects.Save_Items.SaveItemList
                .Select(x => new SaveGameItemModel()
                    {
                        Id = x.Id,
                        IsLock = x.isLock
                    }).ToList();
            
            var saveAchievementsItems = saveGameInfo.Save_WorldObjects.Save_Achivments.SaveAchivmentItem
                .Select(x => new SaveAchievementModel()
                    {
                        Id = x.Id
                    }).ToList();
            
            var saveSkins = saveGameInfo.Save_WorldObjects.Save_Skins.SaveSkinItem
                .Select(x => new SaveSkinModel()
                    {
                        Id = x.Id
                    }).ToList();
            
            var saveUpgradeGameItems = saveGameInfo.Save_Upgrades.Save_Upgrade_Items.Save_New_Items.Save_UpdateNewOjectItem
                .Select(x => new SaveUpgradeGameItemModel()
                    {
                        Id = x.Id,
                    }).ToList();
            
            var saveBoostItems = saveGameInfo.Save_Upgrades.Save_Upgrade_Items.Save_Boost_Items.Save_BoostObjectItem
                .Select(x => new SaveBoostItemModel()
                {
                    Id = x.Id,
                    UserCount = x.UserCount
                }).ToList();
            var saveUpgradeBoostItems = saveGameInfo.Save_Upgrades.Save_Upgrade_Items.Save_UpdateBoost_Items.Save_UpdateBoostObject
                .Select(x => new SaveUpgradeBoostItemModel()
                {
                    Id = x.Id
                }).ToList();

            PlayerInformationModel playerInformation = new PlayerInformationModel()
            {
                PlayerFeature = new PlayerFeatureModel()
                {
                    Experience = saveGameInfo.Save_PlayerFeature.Save_PlayerFeatureBase.experience,
                    MainMoney = saveGameInfo.Save_PlayerFeature.Save_PlayerFeatureBase.mainMoney,
                    SpecialMoney = saveGameInfo.Save_PlayerFeature.Save_PlayerFeatureBase.specialMoney,
                    SelectedSkinId = saveGameInfo.Save_PlayerFeature.Save_PlayerFeatureBase.selectedSkinId,
                    PlayerDistanceRecord = saveGameInfo.Save_PlayerFeature.Save_PlayerFeatureBase.playerDistanceRecord,
                },
                PlayerOptions = new PlayerOptionsModel()
                {
                     IsAd = saveGameInfo.Save_PlayerFeature.Save_PlayerFeatureOptions.isAd,
                     IsMusic = saveGameInfo.Save_PlayerFeature.Save_PlayerFeatureOptions.isMusic,
                     Language = saveGameInfo.Save_PlayerFeature.Save_PlayerFeatureOptions.language,
                     IsFinishedTutorial = saveGameInfo.Save_PlayerFeature.Save_PlayerFeatureOptions.isFinishedTutorial,
                }
            };

            SaveWorldObjectsModel saveWorldObjects = new SaveWorldObjectsModel()
            {
                SaveItems = new SaveItemsModel()
                {
                    SaveItems = saveItems
                },
                SaveAchievementsModel = new SaveAchievementsModel()
                {
                    SaveAchievements = saveAchievementsItems
                },
                SaveSkinsModel = new SaveSkinsModel()
                {
                    SaveSkins = saveSkins
                }
            };
            SaveUpgradesModel saveUpgrades = new SaveUpgradesModel()
            {
                SaveUpgradeGameItem = saveUpgradeGameItems,
                SaveBoostItems = saveBoostItems,
                SaveUpgradeBoostItems = saveUpgradeBoostItems
            };

            return new SaveGameInformationModel() 
            { 
                PlayerInformation = playerInformation,
                SaveWorldObjects = saveWorldObjects,
                SaveUpgrades = saveUpgrades
            };
        }
    }
}
