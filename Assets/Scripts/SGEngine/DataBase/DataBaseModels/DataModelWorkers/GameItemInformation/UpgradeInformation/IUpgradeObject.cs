using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeObject {
    int GetId();
    int GetLvlToUnlock();
    int GetPrice();
    int GetPriceSpecialMoney();
    int GetTimeToSearch();
    int[] GetOpenIds();
    string GetName();
    string GetDescription();
}
