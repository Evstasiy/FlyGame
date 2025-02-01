using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers.GameItemInformation
{
    public class GameItemRepository : IDataBaseRepository
    {
        public List<GameItemModel> allGameItems { get; private set; }
        public List<GameItemModel> saveGameItems { get; private set; }

        private SaveGameInformationModel saveGameInformation;

        public event Action isRepositoryChange;

        public GameItemRepository(ItemsModel itemsModel, SaveGameInformationModel saveGameInformation) 
        {
            allGameItems = itemsModel.GameItems;
            this.saveGameInformation = saveGameInformation;
            this.saveGameItems = new List<GameItemModel>();
        }

        public void ReloadObjectsLanguage(GameLocalizationModel gameLocalization)
        {
            try
            {
                allGameItems = ConnectLanguageToItems(gameLocalization);
                saveGameItems = ConvertSaveToGameItemList(saveGameInformation.SaveWorldObjects.SaveItems.SaveItems);
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
        private List<GameItemModel> ConnectLanguageToItems(GameLocalizationModel gameLocalization) {
            var localiz = gameLocalization.WorldObjectsLocalization.ItemsLocalization.DescriptionItems;

            var badIds = CheckAndGetBadIds(localiz.Select(x => x.Id).ToList(), allGameItems.Select(x => x.Id).ToList(), "ConvertXmlToItemsList");
            allGameItems = allGameItems.Where(x => localiz.Any(y => y.Id == x.Id)).ToList();

            foreach (var paramsItem in allGameItems) {
                try {
                    var itemInfo = localiz.FirstOrDefault(x => x.Id == paramsItem.Id);
                    paramsItem.Description = itemInfo.MainDescription;
                    paramsItem.Name = itemInfo.SecondaryDescription;
                }
                catch (Exception ex) {
                    Debug.LogError(ex.Message);
                    continue;
                }
            }
            return allGameItems;
        }

        /// <summary>
        /// Соединяет информацию из файла сохранения с базовыми игровыми показателями 
        /// </summary>
        /// <returns>Объект с полной информацией и локализацией</returns>
        private List<GameItemModel> ConvertSaveToGameItemList(List<SaveGameItemModel> saveItems)
        {
            var badIds = CheckAndGetBadIds(saveItems.Select(x => x.Id).ToList(), allGameItems.Select(x => x.Id).ToList(), MethodBase.GetCurrentMethod().Name);
            saveItems = saveItems.Where(x => allGameItems.Any(y => y.Id == x.Id)).ToList();
            allGameItems.ForEach(x =>
            {
                x.IsUserHas = saveItems.Any(y => y.Id == x.Id);
            });

            List<GameItemModel> gameItems = new List<GameItemModel>();
            foreach (var paramsItem in saveItems) {
                try {
                    var idItem = Convert.ToInt32(paramsItem.Id);
                    var gameItemInAll = allGameItems.FirstOrDefault(x => x.Id == paramsItem.Id);
                    GameItemModel gameItem = new GameItemModel() {
                        Id = idItem,
                        Description = gameItemInAll.Description,
                        Name = gameItemInAll.Name,
                        IsLock = paramsItem.IsLock
                    };
                    gameItems.Add(gameItem);
                }
                catch (Exception ex) {
                    Debug.LogError(ex.Message);
                    continue;
                }
            }
            return gameItems;
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

        /// <summary>
        /// Проверяет полученные объекты для дальнейшего соханения в базу данных
        /// </summary>
        /// <param name="saveChangeItems">Объекты, которые будут сохранены в БД</param>
        /// <returns></returns>
        /*private bool SaveChanges(List<GameItemModel> saveChangeItems) 
        {
            if (saveChangeItems.Count == 0) return true;
            try 
            {
                var allSaveItems = saveGameInformation.SaveWorldObjects.SaveItems.SaveItems;

                var newObjects = saveChangeItems.Where(x => !allSaveItems.Any(y => y.Id == x.Id));

                var saveChangeItemsWithNotNew = saveChangeItems.Except(newObjects);
                var changeObjects = GetChangeSaveItems(saveChangeItemsWithNotNew.ToList(), allSaveItems);
                allSaveItems = allSaveItems.Except(changeObjects).ToList();
                allSaveItems = allSaveItems.Concat(changeObjects).ToList();

                foreach (var newObject in newObjects) {
                    allSaveItems.Add(new SaveGameItemModel() { Id = newObject.Id, IsLock = newObject.IsLock });
                }

                var deleteObjects = allSaveItems.Where(x => !saveChangeItems.Any(y => y.Id == x.Id));
                allSaveItems = allSaveItems.Except(deleteObjects).ToList();

                saveGameInformation.SaveWorldObjects.SaveItems.SaveItems = allSaveItems;
                DataBaseRepository.dataBaseRepository.SaveChanges(saveGameInformation);
            }
            catch (Exception ex) {
                Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: SaveChangesInSaveFile()");
            }
            return true;
        }*/


        /// <summary>
        /// Проверяет на измение имеющихся в БД объектов сохранения и пришедших
        /// </summary>
        /// <param name="saveChangeItems">Объекты, нуждающиеся в проверке</param>
        /// <param name="allSaveItems">Объекты из базы данных</param>
        /// <returns></returns>
        /*private List<SaveGameItemModel> GetChangeSaveItems(List<GameItemModel> saveChangeItems, List<SaveGameItemModel> allSaveItems)
        {
            List<SaveGameItemModel> needToSaveItems = new List<SaveGameItemModel>();
            foreach (var saveChangeItem in saveChangeItems) {
                try {
                    var itemInSaveFile = allSaveItems.FirstOrDefault(x => x.Id == saveChangeItem.Id);
                    if (itemInSaveFile.IsLock != saveChangeItem.IsLock) {
                        itemInSaveFile.IsLock = saveChangeItem.IsLock;
                        needToSaveItems.Add(itemInSaveFile);
                        continue;
                    }
                }
                catch (Exception ex) {
                    Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: GetChangeSaveItems()");
                }
            }
            return needToSaveItems;
        }*/

        /*public void UpdateSaveObjects(SaveGameInformationModel saveGameInformation) {
            this.saveGameInformation = saveGameInformation;
            saveGameItems = ConvertSaveToGameItemList(saveGameInformation.SaveWorldObjects.SaveItems.SaveItems);
        }*/


        public void SaveChanges()
        {
            try
            {
                DataBaseRepository.dataBaseRepository.SaveChanges(saveGameInformation);
                isRepositoryChange?.Invoke();
            } catch (Exception ex)
            {
                Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: {MethodInfo.GetCurrentMethod().Name}");
            }
            /*
            SaveChanges(saveGameItems);*/
            isRepositoryChange?.Invoke();
        }

        public void Add<T>(T item)
        {
            try
            {
                GameItemModel gameItem = item as GameItemModel;
                allGameItems.FirstOrDefault(x => x.Id == gameItem.Id).IsUserHas = true;
                this.saveGameItems.Add(gameItem);
                saveGameInformation.SaveWorldObjects.SaveItems.SaveItems.Add(gameItem);
                SaveChanges();
            } catch (Exception ex)
            {
                Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: AddItemInSaveFile()");
            }
        }
    }
}
