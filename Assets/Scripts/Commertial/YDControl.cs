using System;
using System.Collections.Generic;
using YG;

public class YDControl : ICommertialService
{
    Dictionary<string, Action> _actions = new Dictionary<string, Action>();
    public PlayerCommertialInformation GetPlayerInformation()
    {
        var playerInfo = new PlayerCommertialInformation()
        {
            SystemLanguageCode = YG2.envir.language.ToLowerInvariant()
        };
        return playerInfo;
    }

    public void SavePlayerInfo(string jsonPlayerInfo)
    {
        YG2.saves.PlayerInfoJson = jsonPlayerInfo;
        YG2.SaveProgress();
    }

    public string GetPlayerInfo()
    {
        string playerInfoJson = null;
        
        playerInfoJson = YG2.saves?.PlayerInfoJson;

        return playerInfoJson;
    }
    
    public void CleanSaves()
    {
        try
        {   
            YG2.SetDefaultSaves();
        } 
        catch
        {
        }
    }
    public void SetRecord(string leaderboardName, int record)
    {
        try
        {
            YG2.SetLeaderboard("FlyDistance", record);

        } catch
        {
        }
    }


    public bool IsADSReady()
    {
        return YG2.isTimerAdvCompleted;
    }

    public void ViewADS()
    {
        YG2.InterstitialAdvShow();
    }

    public void SetActionOnOpenAds(Action onOpenAdsMethod)
    {
        if(onOpenAdsMethod != null && !_actions.ContainsKey("openADS"))
        {
            _actions.Add("openADS", onOpenAdsMethod);
            YG2.onOpenAnyAdv += onOpenAdsMethod;
        }
    }

    public void SetActionOnCloseAds(Action onCloseAdsMethod)
    {
        if (onCloseAdsMethod != null && !_actions.ContainsKey("closeADS"))
        {
            _actions.Add("closeADS", onCloseAdsMethod);
            YG2.onCloseAnyAdv += onCloseAdsMethod;
            YG2.onCloseAnyAdv += OnCloseAds;
        }
    }
    
    void OnCloseAds()
    {
        if (_actions.TryGetValue("openADS",out Action actionOpen))
        {
            YG2.onOpenAnyAdv -= actionOpen;
        }
        if (_actions.TryGetValue("closeADS", out Action actionClose))
        {
            YG2.onCloseAnyAdv -= actionClose;
            YG2.onCloseAnyAdv -= OnCloseAds;
        }
    }

    public void SendMetrica(string eventName, Dictionary<string, object> metricaData)
    {
        //YG2.MetricaSend(eventName, metricaData);
    }
}
namespace YG
{
    public partial class SavesYG
    {
        public string PlayerInfoJson;
    }
}