using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers.GameItemInformation;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers.GameItemInformation.GameWorker;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.Interfaces;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SGEngine.DataBase.DataBaseModels
{
    public class DataBaseRepository : MonoBehaviour
    {
        public static DataBaseRepository dataBaseRepository = null;
        private IDataBase dataBase;

        private SaveGameInformationModel saveGameInformation;
        private GameParametersModel gameParameters;

        #region Repositories
        List<IDataBaseRepository> dataBaseWorkers = new List<IDataBaseRepository>();
        public GameItemRepository GameItemRepos { get; private set; }
        public UpgradeGameItemsRepository UpgradeGameItemsRepos { get; private set; }
        public UpgradeBoostItemsRepository UpgradBoostObjectsRepos { get; private set; }
        public BoostPlayerItemsRepository BoostObjectsRepos { get; private set; }
        public AchievementRepository AchievementRepos { get; private set; }
        public SkinsRepository SkinsRepo { get; private set; }
        public UITranslatorRepository UITranslatorRepos { get; private set; }
        #endregion

        #region PlayerFeatures
        public PlayerFeaturesRepository PlayerFeaturesRepos { get; private set; }
        #endregion

        public event ReloadLocalizationEvent IsReloadLocalizationEvent;
        public delegate void ReloadLocalizationEvent();

        private void Awake()
        {
            InitDataBase();
        }

        public void InitDataBase() 
        {
            if (dataBaseRepository == null)
            {
                dataBaseRepository = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (dataBaseRepository == this ||(dataBaseRepository != null && dataBaseRepository != this))
            {
                //Debug.LogWarning("Попытка создать второй экземпляр Singleton. Уничтожаем объект: " + gameObject.name);
                Destroy(gameObject); 
                return;
            }
            /*
#if UNITY_EDITOR 
            var PATH_SAVE_FILE = Application.persistentDataPath + @"/Save/UserSaveFile";
            Debug.Log(PATH_SAVE_FILE);
            dataBase = new DataBaseComponentXML();
#endif
#if !UNITY_EDITOR && UNITY_WEBGL____
            dataBase = new DataBaseComponentXMLYandex();
#endif
            */

            dataBase = new DataBaseComponentXMLYandex();

            SetupSettings();
        }
         
        /// <summary>
        /// Начальная настройка менеджера, где запрашивается вся информация из базы
        /// </summary>
        private void SetupSettings()
        {
            dataBaseWorkers = new List<IDataBaseRepository>();
            saveGameInformation = dataBase.LoadSaveInfo();
            gameParameters = dataBase.LoadGameParameters();

            PlayerFeaturesRepos = new PlayerFeaturesRepository(saveGameInformation);
            GameItemRepos = new GameItemRepository(gameParameters.WorldObjects.Items, saveGameInformation);
            UpgradeGameItemsRepos = new UpgradeGameItemsRepository(gameParameters.Upgrades, saveGameInformation);
            UpgradBoostObjectsRepos = new UpgradeBoostItemsRepository(gameParameters.Upgrades, saveGameInformation);
            BoostObjectsRepos = new BoostPlayerItemsRepository(gameParameters.Upgrades, saveGameInformation);
            AchievementRepos = new AchievementRepository(gameParameters.WorldObjects.AchievementsItemsModel, saveGameInformation);
            SkinsRepo = new SkinsRepository(gameParameters.WorldObjects.SkinsModel, saveGameInformation);
            UITranslatorRepos = new UITranslatorRepository(gameParameters.UiItemsModel);

            dataBaseWorkers.Add(GameItemRepos);
            dataBaseWorkers.Add(UpgradeGameItemsRepos);
            dataBaseWorkers.Add(UpgradBoostObjectsRepos);
            dataBaseWorkers.Add(PlayerFeaturesRepos);
            dataBaseWorkers.Add(BoostObjectsRepos);
            dataBaseWorkers.Add(AchievementRepos);
            dataBaseWorkers.Add(SkinsRepo);
            dataBaseWorkers.Add(UITranslatorRepos);

            ReloadLocalization(PlayerFeaturesRepos.GetPlayerLanguage());
        }

        /// <summary>
        /// Перезагружает локализацию в игровые объекты воркеров
        /// </summary>
        /// <param name="localizationRegion"></param>
        public void ReloadLocalization(LocalizationOption.LocalizationRegion localizationRegion)
        {
            GameLocalizationModel gameLocalization = dataBase.GetGlobalLocalizationFile(localizationRegion);
            foreach (var dataBaseWorker in dataBaseWorkers)
            {
                dataBaseWorker.ReloadObjectsLanguage(gameLocalization);
            }
            IsReloadLocalizationEvent?.Invoke();
        }

        /// <summary>
        /// Сохраняет изменения в БД
        /// </summary>
        /// <param name="saveGameInformation">Полная информация с изменениями</param>
        /// <returns>Вернет true, если операция успешна</returns>
        public bool SaveChanges(SaveGameInformationModel saveGameInformation) 
        {
            try 
            {
                dataBase.SaveChanges(saveGameInformation);

                //isChangeSaveFile(saveGameInformation);
                return true;
            }
            catch (Exception ex) {
                Debug.LogError(ex.Message + "  " + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Сброс сохранений
        /// </summary>
        public void ResetSaves()
        {
            dataBase.CleanSaves();
            SetupSettings();
        }

    }

}
