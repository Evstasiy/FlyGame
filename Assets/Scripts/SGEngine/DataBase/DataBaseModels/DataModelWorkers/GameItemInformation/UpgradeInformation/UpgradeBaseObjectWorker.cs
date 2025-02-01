using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Базовый абстрактный класс апгрейдов
/// </summary>
public abstract class UpgradeBaseObjectWorker {

    /// <summary>
    /// Метод конвертации информации из базы в базовые игровые элементы 
    /// </summary>
    /// <param name="upgradeItems">Объекты из базы данных</param>
    /// <param name="descriptionItems">Объекты локализации</param>
    /// <returns></returns>
    public virtual List<UpgradeBaseObject> ConvertXmlToObjectList(List<UpgradeGameItemModel> upgradeItems, List<DescriptionItem> descriptionItems) {
        List<UpgradeBaseObject> allUpgradeBaseObjects = new List<UpgradeBaseObject>();
        upgradeItems = upgradeItems.Where(x => descriptionItems.Any(y => y.Id == x.Id)).ToList();

        foreach (var paramNewItem in upgradeItems) {
            try {
                var descriptionItem = descriptionItems.FirstOrDefault(x => x.Id == paramNewItem.Id);
                UpgradeBaseObject upgradeBaseObject = new UpgradeBaseObject() {
                    Id = paramNewItem.Id,
                    LvlToUnlock = paramNewItem.LvlToUnlock,
                    Price = paramNewItem.Price,
                    PriceSpecialMoney = paramNewItem.PriceSpecialMoney,
                    TimeToSearch = paramNewItem.TimeToSearch,
                    Name = descriptionItem.MainDescription,
                    Description = descriptionItem.SecondaryDescription
                };
                allUpgradeBaseObjects.Add(upgradeBaseObject);
            } catch (Exception ex) {
                Debug.LogError(ex.Message);
                continue;
            }
        }
        return allUpgradeBaseObjects;

    }
    
}
