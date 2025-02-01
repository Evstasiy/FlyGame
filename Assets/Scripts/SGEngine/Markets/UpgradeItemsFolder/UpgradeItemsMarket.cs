using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using Assets.Scripts.SGEngine.MarketFolder.EnumsMarket;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeItemsMarket : IMarketBase
{

    public GameObject marketItem;
    private DataBaseRepository dataBaseRepository;
    private PlayerFeaturesRepository playerRepo => DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos;
    private UITranslatorRepository uiTranslateRepo => DataBaseRepository.dataBaseRepository.UITranslatorRepos;

    private int lastLvlOpen = 0;
    private IList<UpgradeGameItemUI> upgradeItems = new List<UpgradeGameItemUI>();

    public UpgradeItemsMarket() 
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
        upgradeItems = new List<UpgradeGameItemUI>();
        var allUpgradeItems = GetItemsForOpen(typeMarketOpen);
        var gameItemsObjs = new List<GameObject>();

        foreach (var gameItem in allUpgradeItems) {
            var itemObject = Object.Instantiate(marketItem, Vector3.zero, Quaternion.identity, contentZone);
            var itemMarket = itemObject.GetComponent<UpgradeGameItemUI>();
            itemMarket.SetItemMarket(gameItem);
            itemMarket.marketBase = marketManager;
            gameItemsObjs.Add(itemObject);
            upgradeItems.Add(itemMarket);
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
    private List<UpgradeGameItemModel> GetItemsForOpen(EnumTypeMarketOpen typeMarketOpen) 
    {
        var allItems = dataBaseRepository.UpgradeGameItemsRepos.allUpgradeGameItems;
        switch (typeMarketOpen) 
        {
            case EnumTypeMarketOpen.DefaultOpen:
                allItems = allItems.OrderBy(x => x.LvlToUnlock).ToList();
                break;
            default:
                //allGameItems.OrderBy(x => x.Name).ToList();
                allItems = allItems.OrderByDescending(x => x.LvlToUnlock).ToList();
                break;
        }
        return allItems;
    }

    private void CheckAndSetItemsStatus()
    {
        var saveItems = dataBaseRepository.UpgradeGameItemsRepos.saveUpgradeGameItems;
        if (saveItems.Count > 0)
        {
            lastLvlOpen = saveItems.Max(x => x.LvlToUnlock);
        }
        foreach (var item in upgradeItems)
        {
            EnumStatesItemMarket currentItemState;
            if (saveItems.Any(x => x.Id == item.gameItem.Id))
            {
                currentItemState = EnumStatesItemMarket.Purchased;
            } 
            else if (playerRepo.GetPlayerLevel() < item.gameItem.LvlToUnlock || lastLvlOpen + 1 < item.gameItem.LvlToUnlock)
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
        var saveUpgradeGameItemsIds = dataBaseRepository.UpgradeGameItemsRepos.saveUpgradeGameItems.Select(x => x.Id);
        UpgradeGameItemModel item = (UpgradeGameItemModel)itemMarket;
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
        UpgradeGameItemModel item = (UpgradeGameItemModel)itemMarket;
        dataBaseRepository.UpgradeGameItemsRepos.Add(item);
        playerRepo.AddPlayerMoney(-1 * item.Price);
        CheckAndSetItemsStatus();
        return true;
    }

    public MarketModel GetMarketModel()
    {
        return new MarketModel()
        {
            MarketType = EnumMarkets.UpgradeItemMarket,
            Name = "UpgradeItemMarket"
        };
    }
}
