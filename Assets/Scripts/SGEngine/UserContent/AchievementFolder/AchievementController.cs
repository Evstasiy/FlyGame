using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.UserContent.AchievementFolder;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementController : MonoBehaviour
{
    [SerializeField]
    private PlayerEffectController playerEffectController;
    [SerializeField]
    private PlayerItemsController playerItemsController;
    [SerializeField]
    private LayerController layerController;
    [SerializeField]
    private TutorialController tutorialController;

    private string activeSceneName;


    private PlayerFeaturesRepository playerFeaturesWorker;

    void Start()
    {
        activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName == "FlyGame")
        {
            ProjectContext.instance.PlayerController.OnPlayerPositionYChange += PlayerPositionYChange;
            ProjectContext.instance.PlayerController.OnPlayerDistanceChange += PlayerDistanceChange;
            playerEffectController.IsActiveEffectAdd += ActiveEffectAdd;
            layerController.OnActiveZoneChanged += ActiveZoneIsChanged;
            tutorialController.IsTutorialDone += IsTutorialDone;
            playerItemsController.IsObjectCollideToPlayer += ObjectCollideToPlayer;
        }
        else if (activeSceneName == "SampleScene")
        {
            playerFeaturesWorker = DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos;
            playerFeaturesWorker.isRepositoryChange += AchievementEventCheck;
        }
    }
    #region MainMenu

    private void AchievementEventCheck()
    {
        var userMoney = playerFeaturesWorker.GetPlayerMoney();
        if (userMoney >= 100)
        {
            WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.GET_100_GOLD);
        }
        if (userMoney >= 1000)
        {
            WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.GET_1000_GOLD);
        }
        if (userMoney >= 20000)
        {
            WorldEventManager.worldManager.AchievementManager.UnlockAchievement(GlobalAchievements.GET_20000_GOLD);
        }

    }
    #endregion MainMenu
    #region FlyGame
    private void ObjectCollideToPlayer(InteractiveObjectModel interactiveObjectModel)
    {
        if (interactiveObjectModel.ObjectType == InteractiveObjectEnum.BigCristal)
        {
            ProjectContext.instance.AchievementManager.UnlockAchievement(GlobalAchievements.FIND_CRISTAL);
        }
    }
    private void IsTutorialDone()
    {
        ProjectContext.instance.AchievementManager.UnlockAchievement(GlobalAchievements.FINISHED_TUTORIAL);
    }
    private void ActiveZoneIsChanged(LayerWorldModel activeLayer)
    {
        if (activeLayer.LayerName == Assets.Scripts.MainGame.Models.LayerEnum.Clouds)
        {
            ProjectContext.instance.AchievementManager.UnlockAchievement(GlobalAchievements.FLY_IN_CLOUD);
        }
        if (activeLayer.LayerName == Assets.Scripts.MainGame.Models.LayerEnum.Space)
        {
            ProjectContext.instance.AchievementManager.UnlockAchievement(GlobalAchievements.FLY_IN_SPACE);
        }
    }
    private void ActiveEffectAdd(EffectObjectController effectObjectController)
    {
        var allActiveEffectTypes = playerEffectController.GetActiveEffectObjectModels()?.Select(x => x.EffectType);
        if(allActiveEffectTypes.Contains(EffectEnum.Shield) && 
            allActiveEffectTypes.Contains(EffectEnum.Magnet) &&
            allActiveEffectTypes.Contains(EffectEnum.Jetpack) &&
            allActiveEffectTypes.Contains(EffectEnum.PlayerSpeed))
        {
            ProjectContext.instance.AchievementManager.UnlockAchievement(GlobalAchievements.GET_ALL_BUFFS);
        }
    }

    private void PlayerDistanceChange(float newDistance)
    {
        if (newDistance > 1000 && newDistance < 1200)
        {
            ProjectContext.instance.AchievementManager.UnlockAchievement(GlobalAchievements.DISTANCE_1000KM_EARTH);
        } 
        else if (newDistance > 5000 && newDistance < 5200)
        {
            ProjectContext.instance.AchievementManager.UnlockAchievement(GlobalAchievements.DISTANCE_5000KM_EARTH);
        } 
        else if (newDistance > 20000 && newDistance < 20200)
        {
            ProjectContext.instance.AchievementManager.UnlockAchievement(GlobalAchievements.DISTANCE_HALF_KM_EARTH);
        } 
        else if (newDistance > 40000 && newDistance < 40200)
        {
            ProjectContext.instance.AchievementManager.UnlockAchievement(GlobalAchievements.DISTANCE_ALL_EARTH);
        }
    }
    private void PlayerPositionYChange(float newPositionY)
    {
        if (newPositionY >= ProjectContext.MAX_POS_Y)
        {
            ProjectContext.instance.AchievementManager.UnlockAchievement(GlobalAchievements.FLY_OUT_SPACE);
        }
    }
    private void OnDestroy()
    {
        if (activeSceneName == "FlyGame")
        {
            ProjectContext.instance.PlayerController.OnPlayerPositionYChange -= PlayerPositionYChange;
            ProjectContext.instance.PlayerController.OnPlayerDistanceChange -= PlayerDistanceChange;
            playerEffectController.IsActiveEffectAdd -= ActiveEffectAdd;
            layerController.OnActiveZoneChanged -= ActiveZoneIsChanged;
            tutorialController.IsTutorialDone -= IsTutorialDone;
            playerItemsController.IsObjectCollideToPlayer -= ObjectCollideToPlayer;
        }
        else if (activeSceneName == "SampleScene")
        {
            playerFeaturesWorker.isRepositoryChange -= AchievementEventCheck;
        }
    }
    #endregion FlyGame
}
