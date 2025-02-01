using Assets.Scripts.SGEngine.DataBase.Models;
using Assets.Scripts.SGEngine.MarketFolder.EnumsMarket;
using Assets.Scripts.SGEngine.UserContent.AchievementFolder;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeGameItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler 
{
    [SerializeField]
    private Text PriceTextLable;
    [SerializeField]
    private Text SpecialPriceTextLable; 
    [SerializeField]
    private Text TimeSearchTextLable;
    [SerializeField]
    private Text LvlTextLable;
    [SerializeField]
    private TMP_Text UiItem_BuyTextTMP;
    [SerializeField]
    private TMP_Text UiItem_EffectNowTextTMP;

    /// <summary>
    /// Менеджер магазинов для возаимодействия
    /// </summary>
    public MarketManager marketBase { private get; set; }

    public bool IsLock { get; private set; } = true;

    /// <summary>
    /// Контейнер информации о товаре
    /// </summary>
    public UpgradeGameItemModel gameItem { get; private set; }

    private Image mainImage;

    private EnumStatesItemMarket CurrentState = EnumStatesItemMarket.Lock;

    /// <summary>
    /// Устанавливает контейнер с информацией о товаре
    /// </summary>
    /// <param name="item"></param>
    public void SetItemMarket(UpgradeGameItemModel item) {
        gameItem = item;
        transform.GetChild(1).GetComponent<Text>().text = gameItem.Id.ToString();
        transform.GetChild(2).GetComponent<Text>().text = gameItem.Name.ToString();

        mainImage = gameObject.transform.GetComponent<Image>();

        PriceTextLable.text = gameItem.Price.ToString();
        SpecialPriceTextLable.text = gameItem.PriceSpecialMoney.ToString();
        TimeSearchTextLable.text = gameItem.TimeToSearch.ToString() + " min";
        LvlTextLable.text = gameItem.LvlToUnlock.ToString();
    }

    public void PrepareUITranslate(Dictionary<string, UIItem> uiItems)
    {
        UiItem_EffectNowTextTMP.text = uiItems["EffectNowText"].Description.ToString();
        UiItem_BuyTextTMP.text = uiItems["BuyBtnText"].Description.ToString();
    }

    public void OnPointerClick(PointerEventData eventData) {
        var result = marketBase.TryToBuy(gameItem);
        SetUiForBuy(result);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mainImage.color = Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData) {
        mainImage.color = Color.white;
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

    private void UnlockItem() {
        enabled = true;
        mainImage.color = Color.white;
        IsLock = false;
    }

    private void LockItem() {
        enabled = false;
        mainImage.color = Color.red;
        IsLock = true;
    }

    private void PurchasedItem() {
        enabled = false;
        mainImage.color = Color.gray;
    }
}
