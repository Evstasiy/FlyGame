using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using Assets.Scripts.SGEngine.MarketFolder.EnumsMarket;
using Assets.Scripts.SGEngine.UserContent.AchievementFolder;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeBoostItemsMarket : IMarketBase   {

    public GameObject marketItem;
    private UpgradeBoostItemsRepository repository => DataBaseRepository.dataBaseRepository.UpgradBoostObjectsRepos;
    private PlayerFeaturesRepository playerRepo => DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos;
    private UITranslatorRepository uiTranslateRepo => DataBaseRepository.dataBaseRepository.UITranslatorRepos;

    private IList<UpgradeBoostGameItemUI> upgradeItems = new List<UpgradeBoostGameItemUI>();

    public UpgradeBoostItemsMarket() 
    {
    }

    public void SetShopItem(GameObject itemMarket) 
    {
        if (marketItem == null)
        {
            marketItem = itemMarket;
        }
    }

    public List<GameObject> InstanceItemsInContentZone(Transform contentZone, MarketManager marketManager, EnumTypeMarketOpen typeMarketOpen)
    {
        upgradeItems = new List<UpgradeBoostGameItemUI>();
        var allUpgradeItems = GetItemsForOpen(typeMarketOpen);
        var gameItemsObjs = new List<GameObject>();

        foreach (var gameItem in allUpgradeItems) {
            var itemObject = Object.Instantiate(marketItem, Vector3.zero, Quaternion.identity, contentZone);
            var itemMarket = itemObject.GetComponent<UpgradeBoostGameItemUI>();
            itemMarket.SetItemMarket(gameItem);
            itemMarket.marketBase = marketManager;
            gameItemsObjs.Add(itemObject);
            upgradeItems.Add(itemMarket);
        }
        CheckAndSetItemsStatus();
        return gameItemsObjs;
    }

    /// <summary>
    /// Предоставляет товар исходя из типа открытия магазина
    /// </summary>
    /// <param name="typeMarketOpen">Тип открытия магазина</param>
    /// <returns>Товары</returns>
    private List<UpgradeBoostItemModel> GetItemsForOpen(EnumTypeMarketOpen typeMarketOpen) 
    {
        var allItems = repository.allUpgradeBoostItems;
        switch (typeMarketOpen) 
        {
            case EnumTypeMarketOpen.DefaultOpen:
                allItems = allItems.OrderBy(x => x.LvlToUnlock).ToList();
                break;
            default:
                //allGameItems.OrderBy(x => x.Name).ToList();
                allItems = allItems.OrderBy(x => x.LvlToUnlock).ToList();
                break;
        }
        return allItems;
    }

    private void CheckAndSetItemsStatus()
    {
        var saveItems = repository.saveUpgradeBoostItems;

        foreach (var item in upgradeItems)
        {
            EnumStatesItemMarket currentItemState;
            if (saveItems.Any(x => x.Id == item.gameItem.Id))
            {
                currentItemState = EnumStatesItemMarket.Purchased;
            } 
            else if (playerRepo.GetPlayerLevel() < item.gameItem.LvlToUnlock 
                || (item.gameItem.IdToUnlock != 0 && !saveItems.Any(x=>x.Id == item.gameItem.IdToUnlock))
                || (playerRepo.GetPlayerMoney() < item.gameItem.Price || playerRepo.GetPlayerSpecialMoney() < item.gameItem.PriceSpecialMoney))
            {
                currentItemState = EnumStatesItemMarket.Lock;
            } 
            else
            {
                currentItemState = EnumStatesItemMarket.Unlock;
            }
            item.SetItemUIState(currentItemState);
        }
    }

    public EnumActionMarketItem TryToBuy(IItem itemMarket) 
    {
        var saveUpgradeGameItemsIds = repository.saveUpgradeBoostItems.Select(x => x.Id);
        UpgradeBoostItemModel item = (UpgradeBoostItemModel)itemMarket;
        if (item.Price <= playerRepo.GetPlayerMoney() && !saveUpgradeGameItemsIds.Any(x => x == item.Id)) 
        {
            return EnumActionMarketItem.Sold;
        } 
        else 
        {
            return EnumActionMarketItem.NotSold;
        }
    }

    public bool BuyItem(IItem itemMarket) 
    {
        UpgradeBoostItemModel item = (UpgradeBoostItemModel)itemMarket;
        CheckAchievementsEvent(item);
        repository.Add(item);
        playerRepo.AddPlayerMoney(-1 * item.Price);
        CheckAndSetItemsStatus();
        return true;
    }
    public MarketModel GetMarketModel()
    {
        return new MarketModel()
        {
            MarketType = EnumMarkets.UpdateBoostItemsMarket,
            Name = "UpdateBoostItemsMarket"
        };
    }

    /*FIX!!!*/
    private void CheckAchievementsEvent(UpgradeBoostItemModel item)
    {
        if (item.IdToUnlock == 0)
        {
            if (item.InternalName == "shield")
            {
                WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.BUY_SHIELD);
            } 
            else if (item.InternalName == "jetpack")
            {
                WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.BUY_JETPACK);
            } 
            else if (item.InternalName == "speedBoost")
            {
                WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.BUY_SPEEDBOOSTER);
            } 
            else if (item.InternalName == "magnet")
            {
                WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.BUY_MAGNET);
            }
        }
        else if(item.LvlToUnlock == 18)
        {
            if (item.InternalName == "shield")
            {
                WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.BUY_MAX_SHIELD);
            } 
            else if (item.InternalName == "jetpack")
            {
                WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.BUY_MAX_JETPACK);
            } 
            else if (item.InternalName == "speedBoost")
            {
                WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.BUY_MAX_SPEEDBOOSTER);
            }
            else if (item.InternalName == "magnet")
            {
                WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.BUY_MAX_MAGNET);
            }
        }
    }
}
