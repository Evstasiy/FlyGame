using Assets.Scripts.MainGame.Models;
using Assets.Scripts.MainGame.Player;
using Assets.Scripts.MainGame.World;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectContext : MonoBehaviour
{
    public const float MAX_POS_Y = 700;
    public const float MIN_POS_Y = 30;
    /// <summary>
    /// Расстояние от конца карты, чтобы кинуть игроку предупреждение
    /// </summary>
    public const float STEP_TO_END_POS_Y = 30;
    public const float WORLD_DISTANCE = 40000;

    public PauseManager PauseManager { get; private set; }

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private InteractiveLayerController interactiveLayerController;

    public PlayerController PlayerController { get; private set; }
    public AchievementManager AchievementManager { get; private set; }

    //ДЛЯ ДЕБАГА
    //public DataBaseRepository DataBaseRepository;
    public DataBaseRepository DataBaseRepository => DataBaseRepository.dataBaseRepository;

    public static ProjectContext instance;

    private void Awake()
    {
        instance = this;
        Initialize();
    }
    

    public void Initialize()
    {
        AchievementManager = new AchievementManager();
        PauseManager = new PauseManager();
        PlayerController = playerController;
        var playerInfoModel = new PlayerInfoModel();
        GlobalPlayerInfo.SetPlayerInfoModel(playerInfoModel);
        SetupPlayerUpdates();
        GlobalPlayerInfo.playerInfoModel.SetPlayerDistanceRecord(DataBaseRepository.PlayerFeaturesRepos.GetPlayerDistanceRecord());
    }

    private void SetupPlayerUpdates()
    {
        /*FIX!*/
        var shieldUpdates = DataBaseRepository.UpgradBoostObjectsRepos.saveUpgradeBoostItems.Where(x => x.InternalName == "shield");
        SetUserUpdateByType(shieldUpdates, InteractiveObjectEnum.BoostShield, EffectEnum.Shield, true);

        var jetpackUpdates = DataBaseRepository.UpgradBoostObjectsRepos.saveUpgradeBoostItems.Where(x => x.InternalName == "jetpack");
        SetUserUpdateByType(jetpackUpdates, InteractiveObjectEnum.BoostJetpack, EffectEnum.Jetpack, false);

        var speedBoostUpdates = DataBaseRepository.UpgradBoostObjectsRepos.saveUpgradeBoostItems.Where(x => x.InternalName == "speedBoost");
        SetUserUpdateByType(speedBoostUpdates, InteractiveObjectEnum.BoostSpeed, EffectEnum.PlayerSpeed, false);

        var magnetUpdates = DataBaseRepository.UpgradBoostObjectsRepos.saveUpgradeBoostItems.Where(x => x.InternalName == "magnet");
        SetUserUpdateByType(magnetUpdates, InteractiveObjectEnum.BoostMagnet, EffectEnum.Magnet, false);

        var playerMobitity = DataBaseRepository.BoostObjectsRepos.saveUpgradeBoostPlayerItems.FirstOrDefault(x => x.InternalName == "playerMobitity");
        SetUserBoostByType(playerMobitity, BasePlayerEffectType.PlayerMobitity);
        
        var playerDebuffSpeed = DataBaseRepository.BoostObjectsRepos.saveUpgradeBoostPlayerItems.FirstOrDefault(x => x.InternalName == "playerDebuffSpeed");
        SetUserBoostByType(playerDebuffSpeed, BasePlayerEffectType.PlayerDebuffSpeed);
        
        var playerMaxSpeed = DataBaseRepository.BoostObjectsRepos.saveUpgradeBoostPlayerItems.FirstOrDefault(x => x.InternalName == "playerMaxSpeed");
        SetUserBoostByType(playerMaxSpeed, BasePlayerEffectType.PlayerMaxSpeed);

        var skinId = DataBaseRepository.PlayerFeaturesRepos.GetPlayerSelectedSkinId();
        PlayerController.SetPlayerSkinById(skinId);
    }

    private void SetUserBoostByType(BoostPlayerItemModel boostPlayer, BasePlayerEffectType baseEffectType) 
    {
        if(boostPlayer == null)
        {
            return;
        }
        var finalEffect = boostPlayer.UserCount * boostPlayer.BaseEffectCount;
        if(baseEffectType == BasePlayerEffectType.PlayerMobitity)
        {
            GlobalPlayerInfo.playerInfoModel.AddForBasePlayerMobility(finalEffect);
        }
        else if(baseEffectType == BasePlayerEffectType.PlayerDebuffSpeed)
        {
            float finalEffectDebuff = (finalEffect / 10000f);
            GlobalPlayerInfo.playerInfoModel.AddPlayerBaseDebuffSpeed(finalEffectDebuff);
        }
        else if(baseEffectType == BasePlayerEffectType.PlayerMaxSpeed)
        {
            GlobalPlayerInfo.playerInfoModel.AddMaxPlayerSpeed(finalEffect);
        }
    }
    
    private void SetUserUpdateByType(IEnumerable<UpgradeBoostItemModel> updates, InteractiveObjectEnum interactiveObjectType, EffectEnum effectType, bool isSetCount) 
    {
        if (updates?.Count() == 0)
        {
            interactiveLayerController.ExcludeTypeInSpawnObjects(interactiveObjectType);
            return;
        }
        var interactiveObj = interactiveLayerController.InteractiveObjectModels.FirstOrDefault(x => x.ObjectType == interactiveObjectType);
        var interactiveObjEffect = interactiveObj.Effects.FirstOrDefault(x => x.EffectType == effectType);

        foreach (var update in updates)
        {
            if (isSetCount)
            {
                interactiveObjEffect.AddEffectCountToDisabled(update.CountEffect);
            } 
            else
            {
                interactiveObjEffect.AddUpdateEffectLifeTimeSec(update.TimeEffectSecond);
            }
        }
    }


}
