using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using UnityEngine;

namespace Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers.GameItemInformation.GameWorker
{
    public class SkinsRepository : IDataBaseRepository
    {
        public List<SkinModel> allSkins { get; private set; }
        public List<SkinModel> saveSkins { get; private set; }

        private SaveGameInformationModel saveGameInformation;

        public event Action isRepositoryChange;

        public SkinsRepository(SkinsModel skinsModel, SaveGameInformationModel saveGameInformation)
        {
            this.allSkins = skinsModel.SkinsItems;
            this.saveGameInformation = saveGameInformation;
        }

        public void ReloadObjectsLanguage(GameLocalizationModel gameLocalization)
        {
            try
            {
                allSkins = ConnectLanguageToItems(gameLocalization);
                saveSkins = ConvertSaveToGameItemList(saveGameInformation.SaveWorldObjects.SaveSkinsModel.SaveSkins);
            } catch (Exception ex)
            {
                Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: {MethodInfo.GetCurrentMethod().Name}");
            }
        }

        /// <summary>
        /// Соединяет информацию из файла локализации с базовыми игровыми показателями 
        /// </summary>
        /// <returns>Объект с полной информацией и локализацией</returns>
        private List<SkinModel> ConnectLanguageToItems(GameLocalizationModel gameLocalization)
        {
            var localiz = gameLocalization.WorldObjectsLocalization.SkinsLocalization.DescriptionItems;

            allSkins = allSkins.Where(x => localiz.Any(y => y.Id == x.Id)).ToList();

            foreach (var paramsItem in allSkins)
            {
                try
                {
                    var itemInfo = localiz.FirstOrDefault(x => x.Id == paramsItem.Id);
                    paramsItem.Description = itemInfo.SecondaryDescription;
                    paramsItem.Name = itemInfo.MainDescription;
                } catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                    continue;
                }
            }
            return allSkins;
        }

        /// <summary>
        /// Соединяет информацию из файла сохранения с базовыми игровыми показателями 
        /// </summary>
        /// <returns>Объект с полной информацией и локализацией</returns>
        private List<SkinModel> ConvertSaveToGameItemList(List<SaveSkinModel> saveItems)
        {
            //var badIds = CheckAndGetBadIds(saveItems.Select(x => x.Id).ToList(), allGameItems.Select(x => x.Id).ToList(), MethodBase.GetCurrentMethod().Name);
            saveItems = saveItems.Where(x => allSkins.Any(y => y.Id == x.Id)).ToList();

            List<SkinModel> items = new List<SkinModel>();
            foreach (var paramsItem in saveItems)
            {
                try
                {
                    var idItem = Convert.ToInt32(paramsItem.Id);
                    var gameItemInAll = allSkins.FirstOrDefault(x => x.Id == paramsItem.Id);
                    var gameItem = gameItemInAll;

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
            DataBaseRepository.dataBaseRepository.SaveChanges(saveGameInformation);
            isRepositoryChange?.Invoke();
        }

        public void Add<T>(T item)
        {
            try
            {
                var newitem = item as SkinModel;

                var itemInSaveFile = saveGameInformation.SaveWorldObjects.SaveSkinsModel.SaveSkins.FirstOrDefault(x => x.Id == newitem.Id);
                if (itemInSaveFile == null)
                {
                    saveGameInformation.SaveWorldObjects.SaveSkinsModel.SaveSkins.Add(newitem);
                    saveSkins.Add(newitem);
                    SaveChanges();
                }
            } catch (Exception ex)
            {
                Debug.LogError($"{ex.Message} \n {ex.StackTrace} \n MethodName: {MethodInfo.GetCurrentMethod().Name}");
            }
        }
    }
}
