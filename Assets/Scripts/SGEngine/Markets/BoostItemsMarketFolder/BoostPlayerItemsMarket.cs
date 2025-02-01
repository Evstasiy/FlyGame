using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using Assets.Scripts.SGEngine.MarketFolder.EnumsMarket;
using Assets.Scripts.SGEngine.UserContent.AchievementFolder;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoostPlayerItemsMarket : IMarketBase
{
    public GameObject marketItem;
    private DataBaseRepository dataBaseRepository;
    private PlayerFeaturesRepository playerRepo => DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos;
    private UITranslatorRepository uiTranslateRepo => DataBaseRepository.dataBaseRepository.UITranslatorRepos;

    private IList<BoostPlayerItemMarketUI> itemsInMarket = new List<BoostPlayerItemMarketUI>();

    public BoostPlayerItemsMarket() 
    {
        dataBaseRepository = DataBaseRepository.dataBaseRepository;
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
        itemsInMarket = new List<BoostPlayerItemMarketUI>();
        var allUpgradeItems = GetItemsForOpen(typeMarketOpen);
        var gameItemsObjs = new List<GameObject>();

        foreach (var gameItem in allUpgradeItems) {
            var itemObject = Object.Instantiate(marketItem, Vector3.zero, Quaternion.identity, contentZone);
            var itemMarket = itemObject.GetComponent<BoostPlayerItemMarketUI>();
            itemMarket.SetItemMarket(gameItem);
            itemMarket.marketBase = marketManager;
            gameItemsObjs.Add(itemObject);
            itemsInMarket.Add(itemMarket);
            itemMarket.PrepareUITranslate(uiTranslateRepo.allItems);
        }
        CheckAndSetItemsStatus();
        return gameItemsObjs;
    }

    /// <summary>
    /// Предоставляет товар исходя из типа открытия магазина
    /// </summary>
    /// <param name="typeMarketOpen">Тип открытия магазина</param>
    /// <returns>Товары</returns>
    private List<BoostPlayerItemModel> GetItemsForOpen(EnumTypeMarketOpen typeMarketOpen) 
    {
        var allItems = dataBaseRepository.BoostObjectsRepos.allUpgradeBoostPlayerItems;
        switch (typeMarketOpen) 
        {
            case EnumTypeMarketOpen.DefaultOpen:
                allItems = allItems.OrderByDescending(x => x.Name).ToList();
                break;
            default:
                allItems = allItems.OrderByDescending(x => x.Name).ToList();
                break;
        }
        return allItems;
    }

    private void CheckAndSetItemsStatus()
    {
        foreach (var item in itemsInMarket)
        {
            EnumStatesItemMarket currentItemState;
            if (playerRepo.GetPlayerMoney() < item.itemModel.FinalPrice 
                || item.itemModel.UserCount >= item.itemModel.MaxBuyCount)
            {
                currentItemState = EnumStatesItemMarket.Lock;
            } 
            else
            {
                currentItemState = EnumStatesItemMarket.Unlock;
            }
            item.UpdateInfoInItem();
            item.SetItemUIState(currentItemState);
        }
    }

    public EnumActionMarketItem TryToBuy(IItem itemMarket) 
    {
        BoostPlayerItemModel item = (BoostPlayerItemModel)itemMarket;
        if (playerRepo.GetPlayerMoney()>= item.FinalPrice) 
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
        BoostPlayerItemModel item = (BoostPlayerItemModel)itemMarket;
        playerRepo.AddPlayerMoney(-1 * item.FinalPrice);
        dataBaseRepository.BoostObjectsRepos.Add(item);
        CheckAndSetItemsStatus();
        CheckAchievementsEvent(item);
        return true;
    }

    public MarketModel GetMarketModel()
    {
        return new MarketModel()
        {
            MarketType = EnumMarkets.BoostPlayerItemsMarket,
            Name = "BoostPlayerItemsMarket"
        };
    }

    /*FIX!!!*/
    private void CheckAchievementsEvent(BoostPlayerItemModel item)
    {
        if (item.MaxBuyCount == item.UserCount)
        {
            if (item.InternalName == "playerMobitity")
            {
                WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.UPDATE_MOBILITY_MAX);
            } 
            else if (item.InternalName == "playerDebuffSpeed")
            {
                WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.UPDATE_DEBUFF_SPEED_MAX);
            }
            else if (item.InternalName == "playerMaxSpeed")
            {
                WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.UPDATE_MAX_SPEED);
            } 
        }
    }
}
