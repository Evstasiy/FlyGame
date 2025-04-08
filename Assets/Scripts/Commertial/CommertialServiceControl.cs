using System;
using System.Collections.Generic;
using YG;

/// <summary>
/// Сервис по интеграции с дополнительными сервисами
/// </summary>
public static class CommertialServiceControl
{
    public static ICommertialService commertialService = new YDControl();

    public static void SavePlayerInfo(string jsonPlayerInfo)
    {
        commertialService.SavePlayerInfo(jsonPlayerInfo);
    }

    public static string GetPlayerInfo()
    {
        return commertialService.GetPlayerInfo();
    }
    
    public static void CleanSaves()
    {
        commertialService.CleanSaves();
    }
    
    public static void SetRecord(string leaderboardName, int record)
    {
        commertialService.SetRecord(leaderboardName, record);
    }

    #region ADS
    public static bool IsADSReady()
    {
        return commertialService.IsADSReady();
    }
    
    public static void ViewRewardADS(string rewardId)
    {
        commertialService.ViewRewardADS(rewardId);
    }

    public static void ViewADS()
    {
        commertialService.ViewADS();
    }
    
    public static void SetActionOnOpenAds(Action onOpenAdsMethod)
    {
        commertialService.SetActionOnOpenAds(onOpenAdsMethod);    
    }

    public static void SetActionOnCloseAds(Action onCloseAdsMethod)
    {
        commertialService.SetActionOnCloseAds(onCloseAdsMethod);
    }

    public static void SetActionOnRewardAds(Action onOpenRewardAdsMethod, string rewardId)
    {
        commertialService.SetActionOnRewardAds(onOpenRewardAdsMethod, rewardId);
    }

    public static void ClearActionsOnRewardAds()
    {
        commertialService.ClearActionsOnRewardAds();
    }
    #endregion ADS

    public static void SendMetrica(string eventName, Dictionary<string, object> metricaData)
    {
        commertialService.SendMetrica(eventName, metricaData);
    }
    
    public static PlayerCommertialInformation GetPlayerInformation()
    {
        return commertialService.GetPlayerInformation();
    }
}