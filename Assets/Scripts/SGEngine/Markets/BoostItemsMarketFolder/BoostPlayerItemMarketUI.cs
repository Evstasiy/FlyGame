using Assets.Scripts.SGEngine.DataBase.Models;
using Assets.Scripts.SGEngine.MarketFolder.EnumsMarket;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Обработчик событий UI товара
/// </summary>
public class BoostPlayerItemMarketUI : MonoBehaviour 
{
    [SerializeField]
    private TMP_Text PriceTextLable;
    [SerializeField]
    private TMP_Text NameTextLable;
    [SerializeField]
    private TMP_Text DescriptionTextLable;
    [SerializeField]
    private TMP_Text UiItem_BuffEffectNowTMP;
    [SerializeField]
    private TMP_Text UiItem_BuffEffectNextTMP;
    [SerializeField]
    private TMP_Text UiItem_BuyTextTMP;
    [SerializeField]
    private TMP_Text UiItem_EffectNowTextTMP;
    [SerializeField]
    private UnityEngine.UI.Image Icon;

    [SerializeField]
    private GameObject StateBackground;
    [SerializeField]
    private GameObject LockIcon;
    [SerializeField]
    private GameObject PurchasedIcon;

    /// <summary>
    /// Контейнер информации о товаре
    /// </summary>
    public BoostPlayerItemModel itemModel { get; set; }

    /// <summary>
    /// Менеджер магазинов для возаимодействия
    /// </summary>
    public MarketManager marketBase { private get; set; }

    private EnumStatesItemMarket CurrentState = EnumStatesItemMarket.Lock;

    /// <summary>
    /// Устанавливает контейнер с информацией о товаре
    /// </summary>
    /// <param name="item"></param>
    public void SetItemMarket(BoostPlayerItemModel item)
    {
        itemModel = item;
        PriceTextLable.text = itemModel.FinalPrice.ToString();
        //Icon.sprite = itemModel.BasePrice.ToString();
        NameTextLable.text = itemModel.Name.ToString();
        DescriptionTextLable.text = itemModel.Description.ToString();
        UiItem_BuffEffectNowTMP.text = itemModel.FinalEffectNow.ToString();
        UiItem_BuffEffectNextTMP.text = "+" + itemModel.BaseEffectCount.ToString();
    }
    
    public void PrepareUITranslate(Dictionary<string, UIItem> uiItems)
    {
        UiItem_EffectNowTextTMP.text = uiItems["EffectNowText"].Description.ToString();
        UiItem_BuyTextTMP.text = uiItems["BuyBtnText"].Description.ToString();
    }

    public void OnBuyItemClick()
    {
        var result = marketBase.TryToBuy(itemModel);
        SetUiForBuy(result);
    }

    public void UpdateInfoInItem()
    {
        PriceTextLable.text = itemModel.FinalPrice.ToString();
    }
    
    public void SetItemUIState(EnumStatesItemMarket itemState)
    {
        CurrentState = itemState;
        switch (CurrentState)
        {
            case EnumStatesItemMarket.Lock:
                LockItem();
                break;
            case EnumStatesItemMarket.Unlock:
                UnlockItem();
                break;
            case EnumStatesItemMarket.Purchased:
                PurchasedItem();
                break;
        }
    }

    private void SetUiForBuy(EnumActionMarketItem enumActionMarketItem) 
    {
        switch (enumActionMarketItem) 
        {
            case EnumActionMarketItem.NotSold:
                StateBackground.SetActive(true);
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

    private void UnlockItem()
    {
        enabled = true;
        StateBackground.SetActive(false);
    }

    private void LockItem()
    {
        enabled = false;
        PurchasedIcon.SetActive(false);
        LockIcon.SetActive(true);
        StateBackground.SetActive(true);
    }

    private void PurchasedItem()
    {
        enabled = false;
        PurchasedIcon.SetActive(true);
        LockIcon.SetActive(false);
        StateBackground.SetActive(true);
    }

    private void BuyItemUI() 
    {
        UiItem_BuffEffectNowTMP.text = itemModel.FinalEffectNow.ToString();
    }
}
