using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AchievementRepository : IDataBaseRepository
{
    public List<AchievementModel> allAchievementItems { get; private set; }
    public List<AchievementModel> saveAchievementItems { get; private set; }

    private SaveGameInformationModel saveGameInformation;

    public event Action isRepositoryChange;

    public AchievementRepository(AchievementsItemsModel achievementsItemsModel, SaveGameInformationModel saveGameInformation) 
    {
        this.allAchievementItems = achievementsItemsModel.AchievementItems;
        this.saveGameInformation = saveGameInformation;
    }

    public void ReloadObjectsLanguage(GameLocalizationModel gameLocalization)
    {
        try
        {
            allAchievementItems = ConnectLanguageToItems(gameLocalization);
            saveAchievementItems = ConvertSaveToGameItemList(saveGameInformation.SaveWorldObjects.SaveAchievementsModel.SaveAchievements);
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
    private List<AchievementModel> ConnectLanguageToItems(GameLocalizationModel gameLocalization)
    {
        var localiz = gameLocalization.WorldObjectsLocalization.AchivmentsLocalization.DescriptionItems;

        allAchievementItems = allAchievementItems.Where(x => localiz.Any(y => y.Id == x.Id)).ToList();

        foreach (var paramsItem in allAchievementItems)
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
        return allAchievementItems;
    }

    /// <summary>
    /// Соединяет информацию из файла сохранения с базовыми игровыми показателями 
    /// </summary>
    /// <returns>Объект с полной информацией и локализацией</returns>
    private List<AchievementModel> ConvertSaveToGameItemList(List<SaveAchievementModel> saveItems)
    {
        //var badIds = CheckAndGetBadIds(saveItems.Select(x => x.Id).ToList(), allGameItems.Select(x => x.Id).ToList(), MethodBase.GetCurrentMethod().Name);
        saveItems = saveItems.Where(x => allAchievementItems.Any(y => y.Id == x.Id)).ToList();

        List<AchievementModel> items = new List<AchievementModel>();
        foreach (var paramsItem in saveItems)
        {
            try
            {
                var idItem = Convert.ToInt32(paramsItem.Id);
                var gameItemInAll = allAchievementItems.FirstOrDefault(x => x.Id == paramsItem.Id);
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
        isRepositoryChange?.Invoke();
    }

    public void Add<T>(T item)
    {
        try
        {
            var newitem = item as AchievementModel;

            var itemInSaveFile = saveGameInformation.SaveWorldObjects.SaveAchievementsModel.SaveAchievements.FirstOrDefault(x => x.Id == newitem.Id);
            if (itemInSaveFile == null)
            {
                saveGameInformation.SaveWorldObjects.SaveAchievementsModel.SaveAchievements.Add(newitem);
                saveAchievementItems.Add(newitem);
                SaveChanges();
            }
        } catch (Exception ex)
        {
            Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: AddItemInSaveFile()");
        }
    }
}

