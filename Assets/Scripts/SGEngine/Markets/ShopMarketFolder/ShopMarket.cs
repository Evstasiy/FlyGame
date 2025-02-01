using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopMarket : IMarketBase {

    public GameObject marketItem;
    private DataBaseRepository dataBaseRepository;
    private PlayerFeaturesRepository playerRepo => DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos;
    
    public ShopMarket() {
        dataBaseRepository = DataBaseRepository.dataBaseRepository;
    }

    public void SetShopItem(GameObject itemMarket) {
        if (this.marketItem == null)
            marketItem = itemMarket;
    }

    public List<GameObject> InstanceItemsInContentZone(Transform contentZone, MarketManager marketManager, EnumTypeMarketOpen typeMarketOpen)
    {
        var allGameItems = GetItemsForOpen(typeMarketOpen);
        var gameItemsObjs = new List<GameObject>();
        foreach (var gameItem in allGameItems) 
        {
            var itemObject = Object.Instantiate(marketItem, Vector3.zero, Quaternion.identity, contentZone);
            var shopItem = itemObject.GetComponent<ShopItemMarket>();
            shopItem.SetItemMarket(gameItem);
            shopItem.marketBase = marketManager;
            gameItemsObjs.Add(itemObject);
        }
        return gameItemsObjs;
    }

    /// <summary>
    /// Предоставляет товар исходя из типа открытия магазина
    /// </summary>
    /// <param name="typeMarketOpen">Тип открытия магазина</param>
    /// <returns>Товары</returns>
    private List<GameItemModel> GetItemsForOpen(EnumTypeMarketOpen typeMarketOpen)
    {
        var allGameItems = dataBaseRepository.GameItemRepos.allGameItems;

        switch (typeMarketOpen) {
            case EnumTypeMarketOpen.DefaultOpen:
                allGameItems = allGameItems.OrderBy(x => x.Name).ToList();
                break;
            case EnumTypeMarketOpen.OrderByPrice:
                allGameItems = allGameItems.OrderBy(x => x.Id).ToList();
                break;
            default:
                //allGameItems.OrderBy(x => x.Name).ToList();
                allGameItems = allGameItems.OrderByDescending(x => x.Id).ToList();
                break;
        }
        return allGameItems;
    }

    public EnumActionMarketItem TryToBuy(IItem itemMarket) 
    {
        GameItemModel item = (GameItemModel)itemMarket;
        if (item.Price <= playerRepo.GetPlayerMoney()) 
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
        GameItemModel item = (GameItemModel)itemMarket;
        dataBaseRepository.GameItemRepos.Add(item);
        return true;
    }

    public MarketModel GetMarketModel()
    {
        return new MarketModel()
        {
            MarketType = EnumMarkets.ShopMarket,
            Name = "ShopMarket"
        };
    }
}
