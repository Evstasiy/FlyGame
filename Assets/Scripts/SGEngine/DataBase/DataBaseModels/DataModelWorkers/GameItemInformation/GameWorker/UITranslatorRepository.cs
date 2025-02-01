using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UITranslatorRepository : IDataBaseRepository
{
    public Dictionary<string, UIItem> allItems { get; private set; }
    public List<UIItem> uiItems { get; set; }

    public event Action isRepositoryChange;

    public UITranslatorRepository(UiItemsModel itemsModel)
    {
        uiItems = itemsModel.UIItems;
    }

    public void ReloadObjectsLanguage(GameLocalizationModel gameLocalization)
    {
        try
        {
            allItems = ConnectLanguageToItems(gameLocalization);
        } 
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: ReloadItemsLanguage()");
        }
    }

    /// <summary>
    /// Соединяет информацию из файла локализации с базовыми игровыми показателями 
    /// </summary>
    /// <returns>Объект с полной информацией и локализацией</returns>
    private Dictionary<string, UIItem> ConnectLanguageToItems(GameLocalizationModel gameLocalization)
    {
        var localiz = gameLocalization.UIItemsLocalizationModel.DescriptionItems;

        var badIds = CheckAndGetBadIds(localiz.Select(x => x.Id).ToList(), uiItems.Select(x => x.Id).ToList(), "ConvertXmlToItemsList");
        uiItems = uiItems.Where(x => localiz.Any(y => y.Id == x.Id)).ToList();
        var allItems = new Dictionary<string, UIItem>();

        foreach (var paramsItem in uiItems)
        {
            try
            {
                var itemInfo = localiz.FirstOrDefault(x => x.Id == paramsItem.Id);
                paramsItem.Description = itemInfo.MainDescription;
                paramsItem.Name = itemInfo.SecondaryDescription;
                allItems.Add(paramsItem.InternalName, paramsItem);
            } 
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                continue;
            }
        }
        return allItems;
    }
    /// <summary>
    /// Сравнивает две коллекции, пишет в дебаг о всех объектах, которые не удалось найти в коллекции A из B
    /// </summary>
    /// <param name="listIdsA">Коллекция, в которой надо искать</param>
    /// <param name="listIdsB">Колеекция с элементами для поиска</param>
    /// <param name="methodName">Метод, где запущен поиск, нужен для инфы в дебаге</param>
    /// <returns>Коллекция не найденных элементов в коллекции A</returns>
    public List<int> CheckAndGetBadIds(List<int> listIdsA, List<int> listIdsB, string methodName = "CheckAndGetBadIds")
    {
        var notFoundIds = listIdsA.Except(listIdsB).ToList();
        string badId = "Bad localize ids count: " + notFoundIds.Count;
        notFoundIds.ForEach(x => badId += "\nId:" + x);
        if (notFoundIds.Count > 0)
            Debug.LogWarning($"{methodName} - {badId}");
        return notFoundIds;
    }

    public void SaveChanges()
    {
    }

    public void Add<T>(T item)
    {
    }
}
