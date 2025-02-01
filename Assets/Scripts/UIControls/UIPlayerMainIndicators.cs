using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerMainIndicators : MonoBehaviour {

    [SerializeField]
    private TMP_Text playerMoney;
    [SerializeField]
    private TMP_Text playerSecondMoney;
    [SerializeField]
    private Scrollbar playerExpScroll;
    [SerializeField]
    private TMP_Text playerLvl;
    [SerializeField]
    private TMP_Text playerExpText;

    PlayerFeaturesRepository playerFeaturesWorker;
    

    void Start()
    {
        playerFeaturesWorker = DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos;
        playerFeaturesWorker.isRepositoryChange += UpdatePlayerIndicators;
        UpdatePlayerIndicators();
    }

    private void OnDestroy()
    {
        playerFeaturesWorker.isRepositoryChange -= UpdatePlayerIndicators;
    }

    private void UpdatePlayerIndicators()
    {
        if (playerMoney != null)
            playerMoney.text = playerFeaturesWorker.GetPlayerMoney().ToString();
        if (playerSecondMoney != null)
            playerSecondMoney.text = playerFeaturesWorker.GetPlayerSpecialMoney().ToString();
        if (playerExpScroll != null)
        {
            var playerExp = playerFeaturesWorker.GetPlayerExperience();
            var playerExpInLvlAll = playerFeaturesWorker.GetPlayerExpInLevel();
            var playerPrevExpInLvlAll = playerFeaturesWorker.GetPlayerPreExpInLevel();
            var needExpInActialLvl = playerExpInLvlAll - playerPrevExpInLvlAll;
            var lvlPersent = playerExp - playerPrevExpInLvlAll;
            float expPercentage = (float)lvlPersent / needExpInActialLvl;
            expPercentage = Mathf.Clamp(expPercentage, 0f, 1f);

            playerExpScroll.size = expPercentage;
            playerExpText.text = lvlPersent
                + "/" + needExpInActialLvl.ToString();
        }

        playerLvl.text = playerFeaturesWorker.GetPlayerLevel().ToString();
    }
}

