using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

#region Items
[XmlRoot(ElementName = "item")]
public class GameItemXml
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    [XmlAttribute(AttributeName = "price")]
    public int Price { get; set; }

}

[XmlRoot(ElementName = "achivment")]
public class AchivmentXml
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    [XmlAttribute(AttributeName = "iconPath")]
    public string IconPath { get; set; }

}

[XmlRoot(ElementName = "skin")]
public class SkinXML
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }
    
    [XmlAttribute(AttributeName = "price")]
    public int Price { get; set; }

    [XmlAttribute(AttributeName = "priceSpecialMoney")]
    public int PriceSpecialMoney { get; set; }

    [XmlAttribute(AttributeName = "lvlToUnlock")]
    public int LvlToUnlock { get; set; }

    [XmlAttribute(AttributeName = "pathToSkin")]
    public string PathToSkin { get; set; }

    [XmlAttribute(AttributeName = "iconPath")]
    public string IconPath { get; set; }

}

[XmlRoot(ElementName = "Items")]
public class Items
{
    [XmlElement(ElementName = "item")]
    public List<GameItemXml> TestItems { get; set; }
}

[XmlRoot(ElementName = "Achivments")]
public class AchivmentsXML
{
    [XmlElement(ElementName = "achivment")]
    public List<AchivmentXml> AchivmentItems { get; set; }
}

[XmlRoot(ElementName = "Skins")]
public class SkinsXML
{
    [XmlElement(ElementName = "skin")]
    public List<SkinXML> SkinsItemsXML { get; set; }
}


[XmlRoot(ElementName = "boostUserItem")]
public class BoostUserItemXML
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    [XmlAttribute(AttributeName = "internalName")]
    public string InternalName { get; set; }

    [XmlAttribute(AttributeName = "basePrice")]
    public int BasePrice { get; set; }

    [XmlAttribute(AttributeName = "multiplyStepPriceByCount")]
    public float MultiplyStepPriceByCount { get; set; }

    [XmlAttribute(AttributeName = "baseEffectCount")]
    public int BaseEffectCount { get; set; }

    [XmlAttribute(AttributeName = "maxBuyCount")]
    public int MaxBuyCount { get; set; }

    [XmlAttribute(AttributeName = "iconPath")]
    public string IconPath { get; set; }
}

[XmlRoot(ElementName = "updateBoostItem")]
public class UpdateBoostItemXML
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    [XmlAttribute(AttributeName = "lvlToUnlock")]
    public int LvlToUnlock { get; set; }

    [XmlAttribute(AttributeName = "idNeedUpgradeForOpen")]
    public int IdToUnlock { get; set; }

    [XmlAttribute(AttributeName = "price")]
    public int Price { get; set; }

    [XmlAttribute(AttributeName = "priceSpecialMoney")]
    public int PriceSpecialMoney { get; set; }

    [XmlAttribute(AttributeName = "countEffect")]
    public int CountEffect { get; set; }

    [XmlAttribute(AttributeName = "timeEffectSecond")]
    public int TimeEffectSecond { get; set; }

    [XmlAttribute(AttributeName = "iconPath")]
    public string IconPath { get; set; }

    [XmlAttribute(AttributeName = "internalName")]
    public string InternalName { get; set; }
}
#endregion Items;

#region Upgrades
#region Upgrade_Items

#region New_Items
[XmlRoot(ElementName = "newAssignment_Item")]
public class newAssignment_Item
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }
    
    [XmlAttribute(AttributeName = "idUpgrade")]
    public int IdUpgrade { get; set; }

    [XmlAttribute(AttributeName = "idNew")]
    public int IdNew { get; set; }
}

[XmlRoot(ElementName = "UpgradeNewAssignment_Items")]
public class UpgradeNewAssignment_Items
{
    [XmlElement(ElementName = "newAssignment_Item")]
    public List<newAssignment_Item> newAssignment_Items { get; set; }
}

[XmlRoot(ElementName = "updateObject")]
public class updateObject
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }
    
    [XmlAttribute(AttributeName = "lvlToUnlock")]
    public int LvlToUnlock { get; set; }

    [XmlAttribute(AttributeName = "price")]
    public int Price { get; set; }

    [XmlAttribute(AttributeName = "priceSpecialMoney")]
    public int PriceSpecialMoney { get; set; }

    [XmlAttribute(AttributeName = "timeToSearch")]
    public int TimeToSearch { get; set; }
}

[XmlRoot(ElementName = "UpgradeNew_Items")]
public class UpgradeNew_Items
{
    [XmlElement(ElementName = "updateObject")]
    public List<updateObject> updateObject { get; set; }
}

[XmlRoot(ElementName = "New_Items")]
public class New_Items
{
    [XmlElement(ElementName = "UpgradeNew_Items")]
    public UpgradeNew_Items UpgradeNew_Items { get; set; }

    [XmlElement(ElementName = "UpgradeNewAssignment_Items")]
    public UpgradeNewAssignment_Items UpgradeNewAssignment_Items { get; set; }
}
#endregion New_Items

#region Boost_Items
[XmlRoot(ElementName = "boostAssignment_Item")]
public class boostAssignment_Item
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    [XmlAttribute(AttributeName = "idUpgrade")]
    public int IdUpgrade { get; set; }

    [XmlAttribute(AttributeName = "idBoost_Item")]
    public int idBoost_Item { get; set; }

    [XmlAttribute(AttributeName = "percentBoost")]
    public int PercentBoost { get; set; }
}

[XmlRoot(ElementName = "UpgradeBoostAssignment_Items")]
public class UpgradeBoostAssignment_Items
{
    [XmlElement(ElementName = "boostAssignment_Item")]
    public List<boostAssignment_Item> boostAssignment_Item { get; set; }
}

[XmlRoot(ElementName = "UpgradeBoost_Items")]
public class UpgradeBoost_Items
{
    [XmlElement(ElementName = "updateBoostItem")]
    public List<UpdateBoostItemXML> updateBoostObject { get; set; }
}

[XmlRoot(ElementName = "Counter_Boost_Player")]
public class Counter_Boost_Player
{
    [XmlElement(ElementName = "boostUserItem")]
    public List<BoostUserItemXML> Counter_Boost_Player_Items { get; set; }
}

[XmlRoot(ElementName = "Boost_Items")]
public class Boost_Items
{
    [XmlElement(ElementName = "Counter_Boost_Player")]
    public Counter_Boost_Player Counter_Boost_Items { get; set; }

    [XmlElement(ElementName = "UpgradeBoost_Items")]
    public UpgradeBoost_Items UpgradeBoost_Items { get; set; }

    [XmlElement(ElementName = "UpgradeBoostAssignment_Items")]
    public UpgradeBoostAssignment_Items UpgradeBoostAssignment_Items { get; set; }
}
#endregion Boost_Items

[XmlRoot(ElementName = "Upgrade_Items")]
public class Upgrade_Items
{
    [XmlElement(ElementName = "New_Items")]
    public New_Items New_Items { get; set; }

    [XmlElement(ElementName = "Boost_Items")]
    public Boost_Items Boost_Items { get; set; }
}

#endregion Upgrade_Items

[XmlRoot(ElementName = "Upgrades")]
public class Upgrades
{
    [XmlElement(ElementName = "Upgrade_Items")]
    public Upgrade_Items Upgrade_Items { get; set; }
    
    //[XmlElement(ElementName = "Upgrade_Cars")]
    //public Upgrade_Cars Upgrade_Cars { get; set; }
}
#endregion Upgrades;


[XmlRoot(ElementName = "item")]
public class UIItemXML
{
    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    [XmlAttribute(AttributeName = "internalName")]
    public string InternalName { get; set; }
}

[XmlRoot(ElementName = "UI")]
public class UIItems
{
    [XmlElement(ElementName = "item")]
    public List<UIItemXML> UIItem { get; set; }
}

[XmlRoot(ElementName = "WorldObjects")]
public class WorldObjects
{
    [XmlElement(ElementName = "Items")]
    public Items Items { get; set; }

    [XmlElement(ElementName = "Achivments")]
    public AchivmentsXML AchivmentsItems { get; set; }

    [XmlElement(ElementName = "Skins")]
    public SkinsXML Skins { get; set; }
}

[XmlRoot(ElementName = "GameParameters")]
public class GameParameters
{
    [XmlElement(ElementName = "UI")]
    public UIItems UIItems { get; set; }

    [XmlElement(ElementName = "WorldObjects")]
    public WorldObjects WorldObjects { get; set; }

    [XmlElement(ElementName = "Upgrades")]
    public Upgrades Upgrades { get; set; }
}
