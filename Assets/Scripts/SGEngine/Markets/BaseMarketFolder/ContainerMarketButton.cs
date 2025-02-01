using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Обработчик событий открытия/закрытия магазина
/// </summary>
public class ContainerMarketButton : MonoBehaviour {
    /// <summary>
    /// Магазин, который будет открыт
    /// </summary>
    public EnumMarkets market;
    /// <summary>
    /// Сортировка объектов магазина про открытии
    /// </summary>
    public EnumTypeMarketOpen typeMarketOpen;
    [SerializeField]
    private MarketManager marketManager;

    [SerializeField]
    private GameObject MarketObject;

    [SerializeField]
    private GameObject MarketItem;

    /// <summary>
    /// Метод открытия магазина с установкой объекта магазина в менеджере
    /// </summary>
    public void OpenMarket()
    {
        MarketObject.SetActive(true);
        marketManager.SetMarket(market, MarketItem);
    }
    /// <summary>
    /// Метод открытия магазина с сортировкой
    /// </summary>
    public void OpenMarketWithType()
    {
        MarketObject.SetActive(true);
        marketManager.OpenMarket(typeMarketOpen);
    }
    
    /// <summary>
    /// Метод закрытия магазина
    /// </summary>
    public void CloseMarket()
    {
        marketManager.CloseMarket();
        MarketObject.SetActive(false);
    }
}
