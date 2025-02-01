using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MarketManager : MonoBehaviour 
{
    private IMarketBase CurrentMarket;
    private GameObject MarketItem;

    [SerializeField]
    private GameObject contentZone;
    [SerializeField]
    private TMP_Text marketNameText;

    private IList<GameObject> MarketItems = new List<GameObject>();

    private bool IsOpenNow = false;
    private DataBaseRepository dataBaseRepository => DataBaseRepository.dataBaseRepository;

    /// <summary>
    /// Метод закрытия магазина с очисткой контента
    /// </summary>
    /// <returns></returns>
    public bool CloseMarket() 
    {
        try 
        {
            CleanContentZone();
            AudioController.Instance.PlayClip("Click");
            return true;
        } 
        catch (Exception ex)
        {
            Debug.LogError($"Во время закрытия магазина {CurrentMarket} произошла ошибка:{ex.Message}");
            return false;
        }
        finally
        {
            IsOpenNow = false;
        }
    }



    /// <summary>
    /// Метод установки магазина и открытия с очисткой контента
    /// </summary>
    /// <param name="market">Магазин для открытия</param>
    public void SetMarket(EnumMarkets market, GameObject marketItem) 
    {
        if (IsOpenNow)
        {
            Debug.LogError("Магазин уже открыт");
            return;
        }

        MarketItem = marketItem;
        switch (market)
        {
            case EnumMarkets.ShopMarket:
                CurrentMarket = new ShopMarket();
                break;
            case EnumMarkets.UpgradeItemMarket:
                CurrentMarket = new UpgradeItemsMarket();
                break;
            case EnumMarkets.BoostPlayerItemsMarket:
                CurrentMarket = new BoostPlayerItemsMarket();
                break;
            case EnumMarkets.UpdateBoostItemsMarket:
                CurrentMarket = new UpgradeBoostItemsMarket();
                break;
            default:
                Debug.LogError("В MarketControl отсутствует назначеный элемент для открытия. Enum name: " + market.ToString());
                return;
        }
        var marketModel = CurrentMarket.GetMarketModel();
        var marketName = dataBaseRepository.UITranslatorRepos.allItems[marketModel.Name]?.Description;
        marketNameText.text = marketName;

        CurrentMarket.SetShopItem(MarketItem);
        OpenMarket(EnumTypeMarketOpen.DefaultOpen);
    }

    /// <summary>
    /// Метод открытия магазина по типу
    /// </summary>
    /// <param name="typeMarketOpen">Тип открытия магазина</param>
    public void OpenMarket(EnumTypeMarketOpen typeMarketOpen)
    {
        if (CurrentMarket == null) 
        {
            Debug.LogError("Магазин не установлен! Enum name: " + typeMarketOpen.ToString());
            return;
        }
        else if (IsOpenNow)
        {
            Debug.LogError("Магазин уже открыт");
            return;
        }

        CleanContentZone();
        MarketItems = CurrentMarket.InstanceItemsInContentZone(contentZone.transform, this, typeMarketOpen);
        IsOpenNow = true;
        AudioController.Instance.PlayClip("Click");
    }

    /// <summary>
    /// Покупка объекта из магазина
    /// </summary>
    /// <param name="item">Покупаемый объект</param>
    /// <returns>Возвращает EnumActionMarketItem с событием покупки</returns>
    public EnumActionMarketItem TryToBuy(IItem item) 
    {
        var result = CurrentMarket.TryToBuy(item);
        if (result != EnumActionMarketItem.NotSold) 
        {
            CurrentMarket.BuyItem(item);
            AudioController.Instance.PlayClip("Buy");
        }
        return result;
    }

    /// <summary>
    /// Удаляет все объекты contentZone, сохраненные ранее в кеше
    /// </summary>
    private void CleanContentZone() 
    {
        try 
        {
            foreach (var item in MarketItems)
            {
                Destroy(item);
            }
        } 
        catch (Exception ex) 
        {
            Debug.LogError("Во время удаления объекта из contentItems произошла ошибка:" + ex.Message);
        }
        
    }
}

