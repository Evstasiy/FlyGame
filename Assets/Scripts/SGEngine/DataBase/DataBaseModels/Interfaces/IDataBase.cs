using Assets.Scripts.SGEngine.DataBase.Models;

namespace Assets.Scripts.SGEngine.DataBase.DataBaseModels.Interfaces
{
    public interface IDataBase
    {
        /// <summary>
        /// Записывает в файл сохранения измения из объекта с данными сохранения
        /// </summary>
        /// <param name="saveInformation">Объект с данными сохранения</param>
        /// <returns></returns>
        public bool SaveChanges(SaveGameInformationModel saveInformation);

        public GameParametersModel LoadGameParameters();
        public SaveGameInformationModel LoadSaveInfo();
        public GameLocalizationModel GetGlobalLocalizationFile(LocalizationOption.LocalizationRegion region);
        public void CleanSaves();
    }
}
