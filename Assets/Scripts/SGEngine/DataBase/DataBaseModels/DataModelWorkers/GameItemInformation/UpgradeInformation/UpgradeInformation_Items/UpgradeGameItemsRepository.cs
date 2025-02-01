using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class UpgradeGameItemsRepository : UpgradeBaseObjectWorker, IDataBaseRepository
{
    private SaveGameInformationModel saveGameInformation;


    public List<UpgradeGameItemModel> allUpgradeGameItems { get; private set; }
    public List<UpgradeGameItemModel> saveUpgradeGameItems { get; private set; }

    public event Action isRepositoryChange;

    public UpgradeGameItemsRepository(UpgradesModel upgrades, SaveGameInformationModel saveGameInformation)
    {
        this.saveGameInformation = saveGameInformation;
        this.allUpgradeGameItems = upgrades.UpgradeGameItems;
    }

    public void ReloadObjectsLanguage(GameLocalizationModel gameLocalization)
    {
        try
        {
            allUpgradeGameItems = ConnectLanguageToItems(gameLocalization);
            saveUpgradeGameItems = ConvertSaveToGameItemList(saveGameInformation.SaveUpgrades.SaveUpgradeGameItem);
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
    private List<UpgradeGameItemModel> ConnectLanguageToItems(GameLocalizationModel gameLocalization)
    {
        var localiz = gameLocalization.UpdateLocalization.UpdateGameItemsLocalization;

        //var badIds = CheckAndGetBadIds(localiz.Select(x => x.Id).ToList(), allGameItems.Select(x => x.Id).ToList(), "ConvertXmlToItemsList");
        allUpgradeGameItems = allUpgradeGameItems.Where(x => localiz.Any(y => y.Id == x.Id)).ToList();

        foreach (var paramsItem in allUpgradeGameItems)
        {
            try
            {
                var itemInfo = localiz.FirstOrDefault(x => x.Id == paramsItem.Id);
                paramsItem.Description = itemInfo.MainDescription;
                paramsItem.Name = itemInfo.SecondaryDescription;
            } 
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                continue;
            }
        }
        return allUpgradeGameItems;
    }

    /// <summary>
    /// Соединяет информацию из файла сохранения с базовыми игровыми показателями 
    /// </summary>
    /// <returns>Объект с полной информацией и локализацией</returns>
    private List<UpgradeGameItemModel> ConvertSaveToGameItemList(List<SaveUpgradeGameItemModel> saveItems)
    {
        //var badIds = CheckAndGetBadIds(saveItems.Select(x => x.Id).ToList(), allGameItems.Select(x => x.Id).ToList(), MethodBase.GetCurrentMethod().Name);
        saveItems = saveItems.Where(x => allUpgradeGameItems.Any(y => y.Id == x.Id)).ToList();

        List<UpgradeGameItemModel> items = new List<UpgradeGameItemModel>();
        foreach (var paramsItem in saveItems)
        {
            try
            {
                var idItem = Convert.ToInt32(paramsItem.Id);
                var gameItemInAll = allUpgradeGameItems.FirstOrDefault(x => x.Id == paramsItem.Id);
                UpgradeGameItemModel gameItem = new UpgradeGameItemModel()
                {
                    Id = idItem,
                    Description = gameItemInAll.Description,
                    Name = gameItemInAll.Name,
                    Price = gameItemInAll.Price,
                    PriceSpecialMoney = gameItemInAll.PriceSpecialMoney,
                    TimeToSearch = gameItemInAll.TimeToSearch,
                    LvlToUnlock = gameItemInAll.LvlToUnlock
                };
                items.Add(gameItem);
            } catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                continue;
            }
        }
        return items;
    }

    public void SaveChanges()
    {
        try
        {
            DataBaseRepository.dataBaseRepository.SaveChanges(saveGameInformation);
            isRepositoryChange?.Invoke();
        } 
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: {MethodInfo.GetCurrentMethod().Name}");
        }
    }

    public void Add<T>(T item)
    {
        try
        {
            var newitem = item as UpgradeGameItemModel;
            saveUpgradeGameItems.Add(newitem);
            saveGameInformation.SaveUpgrades.SaveUpgradeGameItem.Add(newitem);
            SaveChanges();
        } 
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: {MethodInfo.GetCurrentMethod().Name}");
        }
    }
}
