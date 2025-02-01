using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class UpgradeBoostItemsRepository : IDataBaseRepository
{
    public List<UpgradeBoostItemModel> allUpgradeBoostItems { get; private set; }
    public List<UpgradeBoostItemModel> saveUpgradeBoostItems { get; private set; }

    private SaveGameInformationModel saveGameInformation;

    public event Action isRepositoryChange;

    public UpgradeBoostItemsRepository(UpgradesModel upgrades, SaveGameInformationModel saveGameInformation) 
    {
        this.allUpgradeBoostItems = upgrades.UpdateBoostItems;
        this.saveGameInformation = saveGameInformation;
        //saveUpgrades = saveGameInformation.Save_Upgrades;
    }

    public void ReloadObjectsLanguage(GameLocalizationModel gameLocalization)
    {
        try
        {
            allUpgradeBoostItems = ConnectLanguageToItems(gameLocalization);
            saveUpgradeBoostItems = ConvertSaveToGameItemList(saveGameInformation.SaveUpgrades.SaveUpgradeBoostItems);
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
    private List<UpgradeBoostItemModel> ConnectLanguageToItems(GameLocalizationModel gameLocalization)
    {
        var localiz = gameLocalization.UpdateLocalization.UpdateBoostItemsLocalization;

        //var badIds = CheckAndGetBadIds(localiz.Select(x => x.Id).ToList(), allGameItems.Select(x => x.Id).ToList(), "ConvertXmlToItemsList");
        allUpgradeBoostItems = allUpgradeBoostItems.Where(x => localiz.Any(y => y.Id == x.Id)).ToList();

        foreach (var paramsItem in allUpgradeBoostItems)
        {
            try
            {
                var itemInfo = localiz.FirstOrDefault(x => x.Id == paramsItem.Id);
                paramsItem.Description = itemInfo.SecondaryDescription;
                paramsItem.Name = itemInfo.MainDescription;
            } 
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                continue;
            }
        }
        return allUpgradeBoostItems;
    }

    /// <summary>
    /// Соединяет информацию из файла сохранения с базовыми игровыми показателями 
    /// </summary>
    /// <returns>Объект с полной информацией и локализацией</returns>
    private List<UpgradeBoostItemModel> ConvertSaveToGameItemList(List<SaveUpgradeBoostItemModel> saveItems)
    {
        //var badIds = CheckAndGetBadIds(saveItems.Select(x => x.Id).ToList(), allGameItems.Select(x => x.Id).ToList(), MethodBase.GetCurrentMethod().Name);
        saveItems = saveItems.Where(x => allUpgradeBoostItems.Any(y => y.Id == x.Id)).ToList();

        List<UpgradeBoostItemModel> items = new List<UpgradeBoostItemModel>();
        foreach (var paramsItem in saveItems)
        {
            try
            {
                var idItem = Convert.ToInt32(paramsItem.Id);
                var gameItemInAll = allUpgradeBoostItems.FirstOrDefault(x => x.Id == paramsItem.Id);
                var gameItem = gameItemInAll;

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
        saveUpgradeBoostItems = ConvertSaveToGameItemList(saveGameInformation.SaveUpgrades.SaveUpgradeBoostItems);
        isRepositoryChange?.Invoke();
    }

    public void Add<T>(T item)
    {
        try
        {
            UpgradeBoostItemModel newitem = item as UpgradeBoostItemModel;
            var itemInSaveFile = saveGameInformation.SaveUpgrades?.SaveUpgradeBoostItems?.FirstOrDefault(x => x.Id == newitem.Id);
            if (itemInSaveFile == null)
            {
                saveGameInformation.SaveUpgrades.SaveUpgradeBoostItems.Add(newitem);
                SaveChanges();
            }
        } catch (Exception ex)
        {
            Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: AddItemInSaveFile()");
        }
    }
}

