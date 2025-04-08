using System;
using System.Collections.Generic;
using System.Linq;
using YG;

public class YDControl : ICommertialService
{
    Dictionary<string, List<Action>> _actions = new Dictionary<string, List<Action>>();
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

    public void ViewRewardADS(string rewardId)
    {
        YG2.RewardedAdvShow(rewardId);
    }

    public void ViewADS()
    {
        YG2.InterstitialAdvShow();
    }

    public void SetActionOnOpenAds(Action onOpenAdsMethod)
    {
        if(onOpenAdsMethod != null)
        {
            if (_actions.ContainsKey("openADS"))
            {
                _actions["openADS"].Add(onOpenAdsMethod);
            }
            else
            {

                _actions.Add("openADS", new List<Action>(){ onOpenAdsMethod });
            }
            YG2.onOpenAnyAdv += onOpenAdsMethod;
        }
    }

    public void SetActionOnCloseAds(Action onCloseAdsMethod)
    {
        if (onCloseAdsMethod != null)
        {
            if (_actions.ContainsKey("closeADS"))
            {
                _actions["closeADS"].Add(onCloseAdsMethod);
            }
            else
            {

                _actions.Add("closeADS", new List<Action>() { onCloseAdsMethod });
            }
            YG2.onCloseAnyAdv += onCloseAdsMethod;
            YG2.onCloseAnyAdv += OnCloseAds;
        }
    }
    public void SetActionOnRewardAds(Action onOpenRewardAdsMethod, string rewardId)
    {
        if (onOpenRewardAdsMethod != null)
        {
            if (_actions.ContainsKey(rewardId))
            {
                _actions[rewardId].Add(onOpenRewardAdsMethod);
            }
            else
            {

                _actions.Add(rewardId, new List<Action>() { onOpenRewardAdsMethod });
            }
            YG2.onRewardAdv += (string rewardId) => { onOpenRewardAdsMethod(); };
            //YG2.onRewardAdv += OnRewardADS;// Для пропуска межстранички
        }
    }
    
    public void ClearActionsOnRewardAds()
    {
        var rewardsActionsIds = _actions.Where(x => x.Key.Contains("rewardID_")).Select(x=>x.Key).ToList();
        foreach (var rewardActionId in rewardsActionsIds)
        {
            _actions.Remove(rewardActionId);
        }
        YG2.onRewardAdv = null;
    }

    void OnCloseAds()
    {
        if (_actions.TryGetValue("openADS",out List<Action> actionsOpen))
        {
            foreach (var action in actionsOpen)
            {
                YG2.onOpenAnyAdv -= action;
            }
        }
        if (_actions.TryGetValue("closeADS", out List<Action> actionsClose))
        {
            foreach (var action in actionsClose)
            {
                YG2.onCloseAnyAdv -= action;
                YG2.onCloseAnyAdv -= OnCloseAds;
            }
        }
    }
    
    void OnRewardADS()
    {
        YG2.SkipNextInterAdCall();
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