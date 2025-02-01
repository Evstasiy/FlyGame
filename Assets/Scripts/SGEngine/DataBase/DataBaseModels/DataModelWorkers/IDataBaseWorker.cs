using Assets.Scripts.SGEngine.DataBase.Models;
using System;

namespace Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers
{
    interface IDataBaseRepository
    {
        public event Action isRepositoryChange;

        /// <summary>
        /// Метод перезаписи игровых элементов с новой локализацией
        /// </summary>
        /// <param name="gameLocalization">Файл локализации</param>
        /// <returns></returns>
        void ReloadObjectsLanguage(GameLocalizationModel gameLocalization);

        void SaveChanges();

        /// <summary>
        /// Добавляет запись в базу данных сохранения
        /// </summary>
        /// <param name="item">Объект, который будут сохранен в БД</param>
        /// <returns></returns>
        void Add<T>(T item);


        /// <summary>
        /// Обновлает объекты из файла сохранения
        /// </summary>
        /// <param name="saveGameInformation">Файл сохранения</param>
        //void UpdateSaveObjects(SaveGameInformationModel saveGameInformation);
    }
}
