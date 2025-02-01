using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class BoostPlayerItemsRepository : IDataBaseRepository
{
    public List<BoostPlayerItemModel> allUpgradeBoostPlayerItems { get; private set; }
    public List<BoostPlayerItemModel> saveUpgradeBoostPlayerItems { get; private set; }

    private SaveGameInformationModel saveGameInformation;

    public event Action isRepositoryChange;

    public BoostPlayerItemsRepository(UpgradesModel upgrades, SaveGameInformationModel saveGameInformation) 
    {
        this.allUpgradeBoostPlayerItems = upgrades.BoostPlayerItems;
        this.saveGameInformation = saveGameInformation;
        //saveUpgrades = saveGameInformation.Save_Upgrades;
    }

    public void ReloadObjectsLanguage(GameLocalizationModel gameLocalization)
    {
        try
        {
            allUpgradeBoostPlayerItems = ConnectLanguageToItems(gameLocalization);
            saveUpgradeBoostPlayerItems = ConvertSaveToGameItemList(saveGameInformation.SaveUpgrades.SaveBoostItems);
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: {MethodInfo.GetCurrentMethod().Name}");
        }
    }

    /// <summary>
    /// Соединяет информацию из файла локализации с базовыми игровыми показателями 
    /// </summary>
    /// <returns>Объект с полной информацией и локализацией</returns>
    private List<BoostPlayerItemModel> ConnectLanguageToItems(GameLocalizationModel gameLocalization)
    {
        var localiz = gameLocalization.UpdateLocalization.BoostItemsLocalization;

        //var badIds = CheckAndGetBadIds(localiz.Select(x => x.Id).ToList(), allGameItems.Select(x => x.Id).ToList(), "ConvertXmlToItemsList");
        allUpgradeBoostPlayerItems = allUpgradeBoostPlayerItems.Where(x => localiz.Any(y => y.Id == x.Id)).ToList();

        foreach (var paramsItem in allUpgradeBoostPlayerItems)
        {
            try
            {
                var itemInfo = localiz.FirstOrDefault(x => x.Id == paramsItem.Id);
                paramsItem.Name = itemInfo.MainDescription;
                paramsItem.Description = itemInfo.SecondaryDescription;
            } 
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                continue;
            }
        }
        return allUpgradeBoostPlayerItems;
    }

    /// <summary>
    /// Соединяет информацию из файла сохранения с базовыми игровыми показателями 
    /// </summary>
    /// <returns>Объект с полной информацией и локализацией</returns>
    private List<BoostPlayerItemModel> ConvertSaveToGameItemList(List<SaveBoostItemModel> saveItems)
    {
        //var badIds = CheckAndGetBadIds(saveItems.Select(x => x.Id).ToList(), allGameItems.Select(x => x.Id).ToList(), MethodBase.GetCurrentMethod().Name);
        saveItems = saveItems.Where(x => allUpgradeBoostPlayerItems.Any(y => y.Id == x.Id)).ToList();

        List<BoostPlayerItemModel> items = new List<BoostPlayerItemModel>();
        foreach (var paramsItem in saveItems)
        {
            try
            {
                var idItem = Convert.ToInt32(paramsItem.Id);
                var gameItemInAll = allUpgradeBoostPlayerItems.FirstOrDefault(x => x.Id == paramsItem.Id);
                var gameItem = gameItemInAll;
                gameItem.UserCount = paramsItem.UserCount;

                items.Add(gameItem);
            } 
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                continue;
            }
        }
        return items;
    }

    public void SaveChanges()
    {
        DataBaseRepository.dataBaseRepository.SaveChanges(saveGameInformation);
        isRepositoryChange?.Invoke();
    }

    public void Add<T>(T item)
    {
        try
        {
            var newitem = item as BoostPlayerItemModel;
            var itemInSaveFile = saveGameInformation.SaveUpgrades?.SaveBoostItems?.FirstOrDefault(x => x.Id == newitem.Id);
            if (itemInSaveFile != null)
            {
                itemInSaveFile.UserCount++;
                newitem.UserCount = itemInSaveFile.UserCount;
            } else
            {
                newitem.UserCount = 1;
                saveUpgradeBoostPlayerItems.Add(newitem);
                saveGameInformation.SaveUpgrades.SaveBoostItems.Add(newitem);
            }
            SaveChanges();
        } 
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: AddItemInSaveFile()");
        }
    }
}

