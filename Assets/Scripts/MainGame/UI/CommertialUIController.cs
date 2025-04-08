using Assets.Scripts.MainGame.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommertialUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerBoostRewardInput;
    [SerializeField]
    private Button gameOverRewardBtn;
    [SerializeField]
    private GameObject adsMenuUI;
    [SerializeField]
    private TMP_Text gameOverCoinsText;

    [SerializeField]
    private PlayerEffectController playerEffectController;
    [SerializeField]
    private EffectObjectModel adsEffect;

    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;
    private bool isTutorialFinish => ProjectContext.instance.DataBaseRepository.PlayerFeaturesRepos.GetIsFinishedTutorial();
    private PlayerFeaturesRepository playerFeaturesRepository => ProjectContext.instance.DataBaseRepository.PlayerFeaturesRepos;

    /// <summary>
    /// Кулдаун на кнопку буста скорости
    /// </summary>
    private bool lookBoostBtn = true;

    /// <summary>
    /// Минимальная скорость, при которой будет доступно ускорение 
    /// </summary>
    private readonly int minPlayerSpeedForBoost = 10;
    /// <summary>
    /// Максимальная скорость, при которой будет доступно ускорение 
    /// </summary>
    private readonly int maxPlayerSpeedForBoost = 50;

    void Start()
    {
        lookBoostBtn = false;
    }

    void Update()
    {
        if (isPaused || !isTutorialFinish)
        {
            return;
        }
        var playerSpeed = GlobalPlayerInfo.playerInfoModel.FinalSpeed;

        if (playerSpeed >= minPlayerSpeedForBoost && playerSpeed <= maxPlayerSpeedForBoost && !playerBoostRewardInput.activeSelf && !lookBoostBtn)
        {
            playerBoostRewardInput.SetActive(true);
        } 
        else if ((playerSpeed > maxPlayerSpeedForBoost || playerSpeed < minPlayerSpeedForBoost) && playerBoostRewardInput.activeSelf)
        {
            playerBoostRewardInput.SetActive(false);
        }

    }
    public void SetActiveAdvMenuUI(bool isActiveUI)
    {
        ProjectContext.instance.PauseManager.SetPause(isActiveUI);
        adsMenuUI.SetActive(isActiveUI);
        //animatorMenuUI.SetBool("isPause", isActiveUI);
        AudioController.Instance.PlayClip("Click");
        if (!isActiveUI)
        {
            StartCoroutine(LockBoost(10)); 
            playerBoostRewardInput.SetActive(false);
        }
    }

    public void ViewADSForReward(string rewardId)
    {
        CommertialServiceControl.SetActionOnRewardAds(RewardADSView(rewardId), rewardId);
        CommertialServiceControl.ViewRewardADS(rewardId);
    }

    private Action RewardADSView(string rewardID)
    {
        if (rewardID == ADSInfo.REWARD_ID_GameOver)
        {
            gameOverRewardBtn.gameObject.SetActive(false);
            return () =>
            {
                var oldPlayerCoins = GlobalPlayerInfo.playerInfoModel.PlayerCoins;
                GlobalPlayerInfo.playerInfoModel.AddPlayerCoins(oldPlayerCoins);
                playerFeaturesRepository.AddPlayerMoney(oldPlayerCoins);
                StartCoroutine(UIHelper.DisplayScores(new Dictionary<TMP_Text, float>()
                {
                    { gameOverCoinsText, GlobalPlayerInfo.playerInfoModel.PlayerCoins }
                }, oldPlayerCoins));
            };
        } else
        {
            adsMenuUI.SetActive(false);
            ProjectContext.instance.PauseManager.SetPause(false);
            return () =>
            {
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(30);
                playerEffectController.AddEffect(adsEffect);
                StartCoroutine(LockBoost(100));
                playerBoostRewardInput.SetActive(false);
            };
        }
    }

    public IEnumerator LockBoost(int waitMinToUnlockBoost)
    {
        lookBoostBtn = true;
        yield return new WaitForSeconds(waitMinToUnlockBoost);
        lookBoostBtn = false;
    }
}
