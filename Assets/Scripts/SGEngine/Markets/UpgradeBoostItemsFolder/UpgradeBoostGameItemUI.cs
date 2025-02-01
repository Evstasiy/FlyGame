using Assets.Scripts.SGEngine.DataBase.Models;
using Assets.Scripts.SGEngine.MarketFolder.EnumsMarket;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBoostGameItemUI : MonoBehaviour 
{
    [SerializeField]
    private TMP_Text PriceTextLable;
    [SerializeField]
    private TMP_Text SpecialPriceTextLable; 
    [SerializeField]
    private TMP_Text LvlTextLable;
    [SerializeField]
    private Image StateImg;
    [SerializeField]
    private Image IconImg;
    [SerializeField]
    private Image StatusItemImg;
    [SerializeField]
    private Image LockIconImg;
    [SerializeField]
    private Image PurchasedIconImg;
    [SerializeField]
    private TMP_Text NameTextLable;
    [SerializeField]
    private TMP_Text DescriptionTextLable;
    [SerializeField]
    private TMP_Text UiItem_BuyTextTMP;

    /// <summary>
    /// Менеджер магазинов для возаимодействия
    /// </summary>
    public MarketManager marketBase { private get; set; }

    public bool IsLock { get; private set; } = true;

    /// <summary>
    /// Контейнер информации о товаре
    /// </summary>
    public UpgradeBoostItemModel gameItem { get; private set; }

    private EnumStatesItemMarket CurrentState = EnumStatesItemMarket.Lock;

    /// <summary>
    /// Устанавливает контейнер с информацией о товаре
    /// </summary>
    /// <param name="item"></param>
    public void SetItemMarket(UpgradeBoostItemModel item) {
        gameItem = item;
        NameTextLable.text = gameItem.Name.ToString();
        DescriptionTextLable.text = gameItem.Description.ToString();

        PriceTextLable.text = gameItem.Price.ToString();
        SpecialPriceTextLable.text = gameItem.PriceSpecialMoney.ToString();
        LvlTextLable.text = gameItem.LvlToUnlock.ToString();

        IconImg.sprite = Resources.Load<Sprite>(gameItem.IconPath);
        PurchasedIconImg.gameObject.SetActive(false);
        LockIconImg.gameObject.SetActive(false);
        StatusItemImg.gameObject.SetActive(false);
    }

    public void PrepareUITranslate(Dictionary<string, UIItem> uiItems)
    {
        UiItem_BuyTextTMP.text = uiItems["BuyBtnText"].Description.ToString();
    }
    public void OnBuyItemClick() 
    {
        var result = marketBase.TryToBuy(gameItem);
        SetUiForBuy(result);
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
            case EnumActionMarketItem.Sold:
            case EnumActionMarketItem.SoldOut:
                SetItemUIState(EnumStatesItemMarket.Purchased);
                break;
        }
    }

    private void UnlockItem()
    {
        StatusItemImg.gameObject.SetActive(false);
        IsLock = false;
    }

    private void LockItem() 
    {
        enabled = false;
        StatusItemImg.gameObject.SetActive(true);
        LockIconImg.gameObject.SetActive(true);
        IsLock = true;
    }

    private void PurchasedItem() 
    {
        enabled = false;
        StatusItemImg.gameObject.SetActive(true);
        PurchasedIconImg.gameObject.SetActive(true);
        StateImg.color = Color.gray;
    }
}
