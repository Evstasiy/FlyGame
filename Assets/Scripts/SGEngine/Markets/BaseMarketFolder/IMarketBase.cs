using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый интерфейс для магазнов
/// </summary>
public interface IMarketBase
{
    MarketModel GetMarketModel();
    /// <summary>
    /// Устанавливает объект ui товара, который будет отображаться в магазине
    /// </summary>
    /// <param name="itemMarket">Объект UI</param>
    void SetShopItem(GameObject itemMarket);

    /// <summary>
    /// Создает объекты UI товаров в заданной области контента.
    /// </summary>
    /// <param name="contentZone">Область контента товаров магазина</param>
    /// <param name="marketManager">Менеджер магазинов, нужен для взаимодействия кнопки с магазином</param>
    /// <param name="typeMarketOpen">Тип открытия магазина</param>
    /// <returns>Коллекция созданных товаров</returns>
    List<GameObject> InstanceItemsInContentZone(Transform contentZone, MarketManager marketManager, EnumTypeMarketOpen typeMarketOpen);
    
    /// <summary>
    /// Попытка покупки товара
    /// </summary>
    /// <param name="itemMarket">Покупаемый товар</param>
    /// <returns>Вернет итог поптки в виде объекта перечисления</returns>
    EnumActionMarketItem TryToBuy(IItem itemMarket);
    
    /// <summary>
    /// Метод покупки товара
    /// </summary>
    /// <param name="itemMarket">Покупаемый товар</param>
    /// <returns>true:если получилось внести запись в файл сохранения</returns>
    bool BuyItem(IItem itemMarket);

}
