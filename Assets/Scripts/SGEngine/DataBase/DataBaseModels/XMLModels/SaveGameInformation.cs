using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot(ElementName = "SaveItem")]
public class SaveItemXML
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    [XmlAttribute(AttributeName = "isLock")]
    public bool isLock { get; set; }

}

[XmlRoot(ElementName = "SaveItems")]
public class Save_Items
{
    [XmlElement(ElementName = "SaveItem")]
    public List<SaveItemXML> SaveItemList { get; set; }
}

[XmlRoot(ElementName = "Save_Achivments")]
public class Save_Achivments
{
    [XmlElement(ElementName = "SaveAchivmentItem")]
    public List<SaveItemXML> SaveAchivmentItem { get; set; }
}

[XmlRoot(ElementName = "Save_Skins")]
public class Save_Skins
{
    [XmlElement(ElementName = "SaveSkinItem")]
    public List<SaveItemXML> SaveSkinItem { get; set; }
}

[XmlRoot(ElementName = "Save_WorldObjects")]
public class Save_WorldObjects
{
    [XmlElement(ElementName = "Save_Items")]
    public Save_Items Save_Items { get; set; }

    [XmlElement(ElementName = "Save_Achivments")]
    public Save_Achivments Save_Achivments { get; set; }

    [XmlElement(ElementName = "Save_Skins")]
    public Save_Skins Save_Skins { get; set; }
}

#region Update
[XmlRoot(ElementName = "Save_UpdateNewObject")]
public class Save_UpdateNewObject
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }
}

[XmlRoot(ElementName = "Save_BoostObject")]
public class Save_BoostObject
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    [XmlAttribute(AttributeName = "userCount")]
    public int UserCount { get; set; }
}

[XmlRoot(ElementName = "Save_UpdateBoostObject")]
public class Save_UpdateBoostObject 
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }
}

[XmlRoot(ElementName = "Save_New_Items")]
public class Save_New_Items
{
    [XmlElement(ElementName = "Save_UpdateNewObject")]
    public List<Save_UpdateNewObject> Save_UpdateNewOjectItem { get; set; }
}

[XmlRoot(ElementName = "Save_Boost_Items")]
public class Save_Boost_Items
{
    [XmlElement(ElementName = "Save_BoostObject")]
    public List<Save_BoostObject> Save_BoostObjectItem { get; set; }
}

[XmlRoot(ElementName = "Save_Upgrade_Items")]
public class Save_Upgrade_Items
{
    [XmlElement(ElementName = "Save_New_Items")]
    public Save_New_Items Save_New_Items { get; set; }
    
    [XmlElement(ElementName = "Save_Boost_Items")]
    public Save_Boost_Items Save_Boost_Items { get; set; }

    [XmlElement(ElementName = "Save_UpdateBoost_Items")]
    public Save_UpdateBoost_Items Save_UpdateBoost_Items { get; set; }
}

[XmlRoot(ElementName = "Save_UpdateBoost_Items")]
public class Save_UpdateBoost_Items
{
    [XmlElement(ElementName = "Save_UpdateBoostObject")]
    public List<Save_UpdateBoostObject> Save_UpdateBoostObject { get; set; }
}

[XmlRoot(ElementName = "Save_Upgrades")]
public class Save_Upgrades
{
    [XmlElement(ElementName = "Save_Upgrade_Items")]
    public Save_Upgrade_Items Save_Upgrade_Items { get; set; }
}
#endregion Update

[XmlRoot(ElementName = "Save_PlayerFeatureBase")]
public class Save_PlayerFeatureBase {
    [XmlAttribute(AttributeName = "mainMoney")]
    public int mainMoney { get; set; }

    [XmlAttribute(AttributeName = "specialMoney")]
    public int specialMoney { get; set; }
    
    [XmlAttribute(AttributeName = "experience")]
    public int experience { get; set; }

    [XmlAttribute(AttributeName = "selectedSkinId")]
    public int selectedSkinId { get; set; }

    [XmlAttribute(AttributeName = "playerDistanceRecord")]
    public int playerDistanceRecord { get; set; }

}
[XmlRoot(ElementName = "Save_PlayerFeatureOptions")]
public class Save_PlayerFeatureOptions {
    [XmlAttribute(AttributeName = "language")]
    public int language { get; set; }

    [XmlAttribute(AttributeName = "isMusic")]
    public bool isMusic { get; set; }
    
    [XmlAttribute(AttributeName = "isAd")]
    public bool isAd { get; set; }
    
    [XmlAttribute(AttributeName = "isFinishedTutorial")]
    public bool isFinishedTutorial { get; set; }

}

[XmlRoot(ElementName = "Save_PlayerFeatures")]
public class Save_PlayerFeatures {
    [XmlElement(ElementName = "Save_PlayerFeatureBase")]
    public Save_PlayerFeatureBase Save_PlayerFeatureBase { get; set; }
    [XmlElement(ElementName = "Save_PlayerFeatureOptions")]
    public Save_PlayerFeatureOptions Save_PlayerFeatureOptions { get; set; }
}

[XmlRoot(ElementName = "SaveGameInformation")]
public class SaveGameInformation
{
    [XmlElement(ElementName = "Save_WorldObjects")]
    public Save_WorldObjects Save_WorldObjects { get; set; }
    
    [XmlElement(ElementName = "Save_Upgrades")]
    public Save_Upgrades Save_Upgrades { get; set; }
    
    [XmlElement(ElementName = "Save_PlayerFeatures")]
    public Save_PlayerFeatures Save_PlayerFeature { get; set; }
}