using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Обработчик событий UI товара
/// </summary>
public class ShopItemMarket : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler 
{
    [SerializeField]
    private TMP_Text PriceTextLable;
    [SerializeField]
    private TMP_Text NameTextLable;
    [SerializeField]
    private Image Icon;

    /// <summary>
    /// Контейнер информации о товаре
    /// </summary>
    private GameItemModel gameItem { get; set; }

    /// <summary>
    /// Менеджер магазинов для возаимодействия
    /// </summary>
    public MarketManager marketBase { private get; set; }

    /// <summary>
    /// Устанавливает контейнер с информацией о товаре
    /// </summary>
    /// <param name="item"></param>
    public void SetItemMarket(GameItemModel item) {
        gameItem = item;
        gameItem.Price = UnityEngine.Random.Range(8, 21);
        PriceTextLable.text = gameItem.Price.ToString();
        NameTextLable.text = gameItem.Name.ToString();
        if (gameItem.IsUserHas)
        {
            BuyItemUI();
        };
    }

    public void OnPointerClick(PointerEventData eventData) {
        var result = marketBase.TryToBuy(gameItem);
        SetUiForBuy(result); 
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Icon.color = Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData) {
        Icon.color = Color.white;
    }

    private void SetUiForBuy(EnumActionMarketItem enumActionMarketItem) {
        switch (enumActionMarketItem) {
            case EnumActionMarketItem.NotSold:
                Icon.color = Color.red;
                break;
            case EnumActionMarketItem.Sold:
                BuyItemUI();

                break;
            case EnumActionMarketItem.SoldOut:
                BuyItemUI();
                break;
            default:
                break;
        }
    
    }

    private void BuyItemUI() {
        enabled = false;
        Icon.color = Color.gray;
    }
}
