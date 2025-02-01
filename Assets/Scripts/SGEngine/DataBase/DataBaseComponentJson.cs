using Assets.Scripts.SGEngine.DataBase;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.Interfaces;
using Assets.Scripts.SGEngine.DataBase.Extensions;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class DataBaseComponentJson : IDataBase
{
    string PATH_SAVE_FILE;
    public DataBaseComponentJson()
    {
        PATH_SAVE_FILE = Application.persistentDataPath + @"Save/playerInfo.json";
    }
    public bool SaveChanges(SaveGameInformationModel saveInformation)
    {
        throw new NotImplementedException();
    }

    public GameParametersModel LoadGameParameters()
    {
        throw new NotImplementedException();
    }

    public SaveGameInformationModel LoadSaveInfo()
    {
        throw new NotImplementedException();
    }

    public GameLocalizationModel GetGlobalLocalizationFile(LocalizationOption.LocalizationRegion region)
    {
        throw new NotImplementedException();
    }

    public void CleanSaves()
    {
        throw new NotImplementedException();
    }
}


