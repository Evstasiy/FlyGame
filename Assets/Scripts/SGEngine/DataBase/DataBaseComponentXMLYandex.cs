using Assets.Scripts.SGEngine.DataBase;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.Interfaces;
using Assets.Scripts.SGEngine.DataBase.Extensions;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class DataBaseComponentXMLYandex : IDataBase
{
    public DataBaseComponentXMLYandex() 
    {
        CheckBaseFiles();
    }

    public GameParametersModel LoadGameParameters()
    {
        var gameParams = GetXMLGameParameters();
        return gameParams.ConvertXMLToModel();
    }

    public bool SaveChanges(SaveGameInformationModel saveInformation)
    {
        try
        {
            var jsonPlayer = JsonUtility.ToJson(saveInformation);
            CommertialServiceControl.SavePlayerInfo(jsonPlayer);
            return true;
        } catch (Exception ex)
        {
            return false;
        }
    }
    public SaveGameInformationModel LoadSaveInfo()
    {
        var saveInfo = GetSaveGameInformation();
        return saveInfo;
    }

    /// <summary>
    /// Получает файл локализации в зависимости от параметра 
    /// </summary>
    /// <param name="nameLocalization">Необходимый язык</param>
    /// <returns></returns>
    public GameLocalizationModel GetGlobalLocalizationFile(LocalizationOption.LocalizationRegion nameLocalization)
    {
        XmlDocument xml = new XmlDocument();
        TextAsset notSerializeXml = Resources.Load<TextAsset>("Date/DataBase/Localization/" + nameLocalization.ToString());
        xml.LoadXml(notSerializeXml.text);
        XmlNodeReader nod1 = new XmlNodeReader(xml);

        XmlRootAttribute root = new XmlRootAttribute();
        root.ElementName = "GameLocalization";
        root.IsNullable = true;

        XmlSerializer ser = new XmlSerializer(typeof(GameLocalization), root);
        var XMLInformation = (GameLocalization)ser.Deserialize(nod1);

        return XMLInformation.ConvertToModel();
    }

    /// <summary>
    /// Проверяет целостность всех начальных файлов, испоьзуемых в работе 
    /// </summary>
    /// <returns></returns>
    private bool CheckBaseFiles()
    {
        try
        {
            //CheckAndCreateSaveFile();
            return true;
        }
        catch (Exception ex) {
            return false;
        }
    }

    /// <summary>
    /// Возвращает пакет с root и nod для дальнейшей десериализации 
    /// </summary>
    /// <param name="pathFile">Путь к файлу</param>
    /// <param name="elementName">Начальный элемент файла</param>
    /// <param name="isResourceFile">Брать ли файл из папки ресурсов</param>
    /// <returns></returns>
    private SerializerPacket GetSerializerPacket(string pathFile, string elementName, bool isResourceFile = true) {
        XmlDocument xml = new XmlDocument();
        string fileText = "";
        if (isResourceFile) {
            fileText = Resources.Load<TextAsset>(pathFile).text;
        }
        else {
            using (StreamReader sr = new StreamReader(pathFile + ".xml")) {
                fileText = sr.ReadToEnd();
            }
        }
        xml.LoadXml(fileText);
        XmlNodeReader nod1 = new XmlNodeReader(xml);

        XmlRootAttribute root = new XmlRootAttribute();
        root.ElementName = elementName;
        root.IsNullable = true;

        return new SerializerPacket(root, nod1);
    }

    /// <summary>
    /// Сериализует данные из файла сохранения PATH_SAVE_FILE в SaveGameInformation элементы
    /// </summary>
    /// <returns></returns>
    public SaveGameInformationModel GetSaveGameInformation() 
    {
        var jsonPlayer = CommertialServiceControl.GetPlayerInfo();
        SaveGameInformationModel saveModel = new SaveGameInformationModel();
        if(String.IsNullOrEmpty(jsonPlayer)) 
        {
            var environmentDatas = CommertialServiceControl.GetPlayerInformation();
            saveModel.SaveWorldObjects.SaveSkinsModel.SaveSkins.Add(new SaveSkinModel() { Id = 1 });
            saveModel.PlayerInformation.PlayerFeature.SelectedSkinId = 1; 
            saveModel.PlayerInformation.PlayerOptions.Language = (int)EnumLocalizationRegionExtention.ToEnumLanguage(environmentDatas.SystemLanguageCode);
        } 
        else
        {
            saveModel = JsonUtility.FromJson<SaveGameInformationModel>(jsonPlayer);
        }

        return saveModel;
    }

    /// <summary>
    /// Сериализует информацию из Date/DataBase/GameInfo.xml в GameParameters элементы
    /// </summary>
    /// <returns></returns>
    public GameParameters GetXMLGameParameters() {
        string path = "Date/DataBase/GameInfo";
        string elementName = "GameParameters";
        var serializerPacket = GetSerializerPacket(path, elementName);
        XmlSerializer ser = new XmlSerializer(typeof(GameParameters), serializerPacket.root);
        var XMLInformation = (GameParameters)ser.Deserialize(serializerPacket.node);

        return XMLInformation;
    }

    public void CleanSaves()
    {
        CommertialServiceControl.CleanSaves();
    }
}
