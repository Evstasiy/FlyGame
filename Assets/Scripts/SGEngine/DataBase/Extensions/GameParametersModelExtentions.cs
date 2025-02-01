using Assets.Scripts.SGEngine.DataBase.Models;
using System.Linq;

namespace Assets.Scripts.SGEngine.DataBase.Extensions
{
    public static class GameParametersModelExtentions
    {
        public static GameParametersModel ConvertXMLToModel(this GameParameters gameParameters)
        {
            var items = gameParameters.WorldObjects.Items.TestItems
                .Select(x => new GameItemModel()
                {
                    Id = x.Id,
                    Price = x.Price
                });
            
            var uiItems = gameParameters.UIItems.UIItem
                .Select(x => new UIItem()
                {
                    Id = x.Id,
                    InternalName = x.InternalName
                });
            
            var achievementItems = gameParameters.WorldObjects.AchivmentsItems.AchivmentItems
                .Select(x => new AchievementModel()
                {
                    Id = x.Id,
                    IconPath = x.IconPath
                });
            
            var skinItems = gameParameters.WorldObjects.Skins.SkinsItemsXML
                .Select(x => new SkinModel()
                {
                    Id = x.Id,
                    IconPath = x.IconPath,
                    PathToSkin = x.PathToSkin,
                    Price = x.Price,
                    LvlToUnlock = x.LvlToUnlock,
                    PriceSpecialMoney = x.PriceSpecialMoney
                });

            var worldObjects = new WorldObjectsModel()
            {
                Items = new ItemsModel()
                {
                    GameItems = items.ToList()
                },
                AchievementsItemsModel = new AchievementsItemsModel()
                {
                    AchievementItems = achievementItems.ToList()
                },
                SkinsModel = new SkinsModel()
                {
                    SkinsItems = skinItems.ToList()
                }
            };

            var uiItemsModel = new UiItemsModel()
            {
                UIItems = uiItems.ToList()
            };

            var udgradeItems = gameParameters.Upgrades.Upgrade_Items.New_Items.UpgradeNew_Items.updateObject
                .Select(x => new UpgradeGameItemModel()
                {
                    Id = x.Id,
                    Price = x.Price,
                    LvlToUnlock = x.LvlToUnlock,
                    PriceSpecialMoney = x.PriceSpecialMoney,
                    TimeToSearch = x.TimeToSearch
                }
            ).ToList();


            var boostPlayerItems = gameParameters.Upgrades.Upgrade_Items.Boost_Items.Counter_Boost_Items.Counter_Boost_Player_Items
                .Select(x => new BoostPlayerItemModel()
                {
                    Id = x.Id,
                    MultiplyStepPriceByCount = x.MultiplyStepPriceByCount,
                    BaseEffectCount = x.BaseEffectCount,
                    BasePrice = x.BasePrice,
                    IconPath = x.IconPath,
                    InternalName = x.InternalName,
                    MaxBuyCount = x.MaxBuyCount
                }
            ).ToList();
            
            var upgradeBoostItems = gameParameters.Upgrades.Upgrade_Items.Boost_Items.UpgradeBoost_Items.updateBoostObject
                .Select(x => new UpgradeBoostItemModel()
                {
                    Id = x.Id,
                    Price = x.Price,
                    LvlToUnlock = x.LvlToUnlock,
                    IdToUnlock = x.IdToUnlock,
                    PriceSpecialMoney = x.PriceSpecialMoney,
                    CountEffect = x.CountEffect,
                    TimeEffectSecond = x.TimeEffectSecond,
                    IconPath = x.IconPath,
                    InternalName = x.InternalName
                }
            ).ToList();

            var upgrades = new UpgradesModel()
            {
                UpgradeGameItems = udgradeItems,
                BoostPlayerItems = boostPlayerItems,
                UpdateBoostItems = upgradeBoostItems
            };

            return new GameParametersModel()
            {
                WorldObjects = worldObjects,
                Upgrades = upgrades,
                UiItemsModel = uiItemsModel
            };
        }
    }
}
