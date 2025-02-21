using System;
using System.Collections.Generic;

public interface ICommertialService 
{
    PlayerCommertialInformation GetPlayerInformation();
    void SavePlayerInfo(string jsonPlayerInfo);
    string GetPlayerInfo();
    void CleanSaves();
    void SetRecord(string leaderboardName, int record);

    #region ADS
    bool IsADSReady();
    void ViewRewardADS(string rewardId);
    void ViewADS();
    void SetActionOnOpenAds(Action onOpenAdsMethod);
    void SetActionOnCloseAds(Action onCloseAdsMethod);
    void SetActionOnRewardAds(Action onOpenRewardAdsMethod, string rewardId);
    void ClearActionsOnRewardAds();
    #endregion ADS

    #region Metrics
    void SendMetrica(string eventName, Dictionary<string, object> metricaData);
    #endregion Metrics

}
