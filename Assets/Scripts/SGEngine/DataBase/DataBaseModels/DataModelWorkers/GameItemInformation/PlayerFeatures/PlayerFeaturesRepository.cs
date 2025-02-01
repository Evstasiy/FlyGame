using Assets.Scripts.SGEngine.DataBase;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.Interfaces;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Diagnostics;
using System.Reflection;
using static Assets.Scripts.SGEngine.DataBase.LocalizationOption;

public class PlayerFeaturesRepository : IDataBaseRepository
{
    private LevelManager levelManager = new LevelManager();
    private SaveGameInformationModel saveGameInformation;

    public event Action isRepositoryChange;

    public PlayerFeaturesRepository(SaveGameInformationModel saveGameInformation) 
    {
        this.saveGameInformation = saveGameInformation;
    }

    public int GetPlayerLevel()
    {
        return levelManager.GetLevelInExperience(GetPlayerExperience());
    }
    
    public int GetPlayerExpInLevel()
    {
        return levelManager.GetExperienceInLvl(GetPlayerLevel());
    }
    public int GetPlayerPreExpInLevel()
    {
        return levelManager.GetPrevExperienceInLvl(GetPlayerLevel());
    }

    public int GetPlayerMoney() 
    {
        return saveGameInformation.PlayerInformation.PlayerFeature.MainMoney;
    }
    public int GetPlayerSpecialMoney() 
    {
        return saveGameInformation.PlayerInformation.PlayerFeature.SpecialMoney;
    }

    public int GetPlayerExperience() 
    {
        return saveGameInformation.PlayerInformation.PlayerFeature.Experience;
    }

    public int GetPlayerSelectedSkinId()
    {
        return saveGameInformation.PlayerInformation.PlayerFeature.SelectedSkinId;
    }
    
    public int GetPlayerDistanceRecord()
    {
        return saveGameInformation.PlayerInformation.PlayerFeature.PlayerDistanceRecord;
    }
    
    
    #region Options
    public LocalizationRegion GetPlayerLanguage() 
    {
        return (LocalizationRegion)this.saveGameInformation.PlayerInformation.PlayerOptions.Language;
    }
    
    public bool GetPlayerIsMusic() 
    {
        return this.saveGameInformation.PlayerInformation.PlayerOptions.IsMusic;
    }

    public bool GetIsFinishedTutorial()
    {
        return saveGameInformation.PlayerInformation.PlayerOptions.IsFinishedTutorial;
    }

    public bool SetPlayerLanguage(LocalizationRegion lang)
    {
        try
        {
            this.saveGameInformation.PlayerInformation.PlayerOptions.Language = (int)lang;
            SaveChanges();
            DataBaseRepository.dataBaseRepository.ReloadLocalization(GetPlayerLanguage());
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Method: {0}, Error: {1}",MethodInfo.GetCurrentMethod().Name,ex);
            return false;
        }
    }
    public bool SetPlayerPlayerIsMusic(bool isMusic)
    {
        saveGameInformation.PlayerInformation.PlayerOptions.IsMusic = isMusic;
        SaveChanges();
        return true;
    }
    
    public bool SetPlayerIsFinishedTutorial(bool isFinishedTutorial)
    {
        saveGameInformation.PlayerInformation.PlayerOptions.IsFinishedTutorial = isFinishedTutorial;
        SaveChanges();
        return true;
    }

    #endregion Options

    public bool AddPlayerMoney(int playerMoney)
    {
        return SetPlayerMoney(GetPlayerMoney() + playerMoney);
    }

    public bool AddPlayerSpecialMoney(int playerSpecialMoney)
    {
        return SetPlayerSpecialMoney(GetPlayerSpecialMoney() + playerSpecialMoney);
    }

    public bool AddPlayerExperience(int playerExperience)
    {
        return SetPlayerExperience(GetPlayerExperience() + playerExperience);
    }

    public bool SetPlayerSelectedSkinId(int playerSelectedSkinId)
    {
        var selectedSkinIdSave = saveGameInformation.PlayerInformation.PlayerFeature.SelectedSkinId;
        if (selectedSkinIdSave != playerSelectedSkinId)
        {
            saveGameInformation.PlayerInformation.PlayerFeature.SelectedSkinId = playerSelectedSkinId;
            SaveChanges();
        }
        return true;
    }

    public bool SetPlayerDistanceRecord(int playerDistanceRecord)
    {
        var playerDistanceRecordSave = saveGameInformation.PlayerInformation.PlayerFeature.PlayerDistanceRecord;
        if (playerDistanceRecordSave != playerDistanceRecord)
        {
            saveGameInformation.PlayerInformation.PlayerFeature.PlayerDistanceRecord = playerDistanceRecord;
            SaveChanges();
        }
        return true;
    }

    private bool SetPlayerMoney(int playerMoney) 
    {
        var playerMoneySave = saveGameInformation.PlayerInformation.PlayerFeature.MainMoney;
        if (playerMoneySave != playerMoney) 
        {
            saveGameInformation.PlayerInformation.PlayerFeature.MainMoney = playerMoney;
            SaveChanges();
        }
        return true;
    }

    private bool SetPlayerSpecialMoney(int playerSpecialMoney) 
    {
        var playerSpecialMoneySave = saveGameInformation.PlayerInformation.PlayerFeature.SpecialMoney;
        if (playerSpecialMoneySave != playerSpecialMoney) 
        {
            saveGameInformation.PlayerInformation.PlayerFeature.SpecialMoney = playerSpecialMoney;
            SaveChanges();
        }
        return true;
    }

    private bool SetPlayerExperience(int playerExperience)
    {
        var playerExperienceSave = saveGameInformation.PlayerInformation.PlayerFeature.Experience;
        if (playerExperienceSave != playerExperience) 
        {
            saveGameInformation.PlayerInformation.PlayerFeature.Experience = playerExperience;
            SaveChanges();
        }
        return true;
    }
    
    public void SaveChanges()
    {
        DataBaseRepository.dataBaseRepository.SaveChanges(saveGameInformation);
        isRepositoryChange?.Invoke();
    }

    public void ReloadObjectsLanguage(GameLocalizationModel gameLocalization)
    {
    }

    public void Add<T>(T item)
    {
        throw new NotImplementedException();
    }
}
