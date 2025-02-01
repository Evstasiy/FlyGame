using Assets.Scripts.SGEngine.DataBase.Models;
using System.Linq;

namespace Assets.Scripts.SGEngine.DataBase.Extensions
{
    public static class GameLocalizationExtentions
    {
        public static GameLocalizationModel ConvertToModel(this GameLocalization gameLocalization)
        {
            var items = gameLocalization.WorldObjects_Localization.Items_Localization.descriptionItems
                .Select(x => new DescriptionItemsModel()
                {
                    Id = x.Id,
                    MainDescription = x.MainDescription,
                    SecondaryDescription = x.SecondaryDescription
                }).ToList();
            
            var uiItems = gameLocalization.UI_Localization.descriptionItems
                .Select(x => new DescriptionItemsModel()
                {
                    Id = x.Id,
                    MainDescription = x.MainDescription,
                    SecondaryDescription = x.SecondaryDescription
                }).ToList();
            
            var achivmentsItems = gameLocalization.WorldObjects_Localization.Achivments_Localization.descriptionItems
                .Select(x => new DescriptionItemsModel()
                {
                    Id = x.Id,
                    MainDescription = x.MainDescription,
                    SecondaryDescription = x.SecondaryDescription
                }).ToList();
            
            var skinsItems = gameLocalization.WorldObjects_Localization.Skins_Localization.descriptionItems
                .Select(x => new DescriptionItemsModel()
                {
                    Id = x.Id,
                    MainDescription = x.MainDescription,
                    SecondaryDescription = x.SecondaryDescription
                }).ToList();
            
            var updateGameitems = gameLocalization.Upgrades_Localization.Upgrade_Items_Localization.Game_Items_Update_Localization.descriptionItems
                .Select(x => new UpdateItemsLocalizationModel()
                {
                    Id = x.Id,
                    MainDescription = x.MainDescription,
                    SecondaryDescription = x.SecondaryDescription
                }).ToList();
            
            var boostItems = gameLocalization.Upgrades_Localization.Upgrade_Items_Localization.Boost_Items_Localization.descriptionItems
                .Select(x => new BoostItemsLocalizationModel()
                {
                    Id = x.Id,
                    MainDescription = x.MainDescription,
                    SecondaryDescription = x.SecondaryDescription
                }).ToList();
            
            var upgradeBoostItems = gameLocalization.Upgrades_Localization.Upgrade_Items_Localization.Upgrade_Boost_Items_Localization.descriptionItems
                .Select(x => new UpdateBoostItemsLocalizationModel()
                {
                    Id = x.Id,
                    MainDescription = x.MainDescription,
                    SecondaryDescription = x.SecondaryDescription
                }).ToList();

            return new GameLocalizationModel()
            {
                UIItemsLocalizationModel = new UIItemsLocalizationModel()
                {
                    DescriptionItems = uiItems
                },
                WorldObjectsLocalization = new WorldObjectsLocalizationModel()
                {
                    ItemsLocalization = new ItemsLocalizationModel()
                    {
                        DescriptionItems = items
                    },
                    AchivmentsLocalization = new AchivmentsLocalizationModel()
                    {
                        DescriptionItems = achivmentsItems
                    },
                    SkinsLocalization = new SkinsLocalizationModel()
                    {
                        DescriptionItems = skinsItems
                    }
                },
                UpdateLocalization = new UpdateLocalizationModel()
                {
                    UpdateGameItemsLocalization = updateGameitems,
                    UpdateBoostItemsLocalization = upgradeBoostItems,
                    BoostItemsLocalization = boostItems
                }
            };
        }
    }
}
