using Assets.Scripts.MainGame.Player;
using Assets.Scripts.SGEngine.UserContent.AchievementFolder;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private GameObject exitMainQuestionDialogUI;
    [SerializeField]
    private GameObject mainMenuDialogUI;
    [SerializeField]
    private Animator animatorMenuUI;

    [SerializeField]
    private Image songUI;
    [SerializeField]
    private Sprite songOff;
    [SerializeField]
    private Sprite songOn;
    private bool isActiveSongUI;

    [SerializeField]
    private GameRulesController gameRulesController;

    [SerializeField]
    private GameObject gameOverMenuUIBtn;
    [SerializeField]
    private GameObject gameOverMenuUI;
    [SerializeField]
    private Animator gameOverMenuUIAnimator;
    [SerializeField]
    private TMP_Text gameOverCoinsText;
    [SerializeField]
    private TMP_Text gameOverSpecialCoinsText;
    [SerializeField]
    private TMP_Text gameOverDistanceText;
    [SerializeField]
    private TMP_Text gameOverExpText;
    [SerializeField]
    private TMP_Text gameOverExpRecordText;
    [SerializeField]
    private Image gameOverRecordNewImg;
    [SerializeField]
    private TMP_Text gameOverInfoText;

    void Start()
    {
        isActiveSongUI = ProjectContext.instance.DataBaseRepository.PlayerFeaturesRepos.GetPlayerIsMusic();
        SetSongActive(isActiveSongUI);
        exitMainQuestionDialogUI.SetActive(false);
        gameRulesController.IsGameOver += IsGameOver;
    }

    public void SetActiveMenuUI(bool isActiveMenuUI)
    {
        ProjectContext.instance.PauseManager.SetPause(isActiveMenuUI);

        if (isActiveMenuUI)
        {
            menuUI.SetActive(isActiveMenuUI);
        }
        animatorMenuUI.SetBool("isPause", isActiveMenuUI);
        AudioController.Instance.PlayClip("Click");
    }
    
    public void TryExitInMainScene()
    {
        mainMenuDialogUI.SetActive(false);
        exitMainQuestionDialogUI.SetActive(true);
    }
    
    public void FailExitInMainMenu()
    {
        mainMenuDialogUI.SetActive(true);
        exitMainQuestionDialogUI.SetActive(false);
    }

    public void SetSongUIActive()
    {
        isActiveSongUI = !isActiveSongUI;
        SetSongActive(isActiveSongUI);
        AudioController.Instance.PlayClip("Click");
    }

    public void ExitInMainScene()
    {
        AudioController.Instance.PlayClip("Click");
        if (Random.Range(1, 10) > 3 && CommertialServiceControl.IsADSReady())
        {
            CommertialServiceControl.SetActionOnCloseAds(() => { SceneManager.LoadScene(0); });
            CommertialServiceControl.ViewADS();
        } 
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    private void SetSongActive(bool isActiveSong)
    {
        songUI.sprite = (isActiveSongUI) ? songOn : songOff;
        ProjectContext.instance.DataBaseRepository.PlayerFeaturesRepos.SetPlayerPlayerIsMusic(isActiveSongUI);
    }

    private void IsGameOver()
    {
        var playerFinalDistance = (int)GlobalPlayerInfo.playerInfoModel.PlayerDistance;
        float gameOverExpRecord = 0;
        gameOverMenuUI.SetActive(true);
        gameOverRecordNewImg.gameObject.SetActive(false);
        if (GlobalPlayerInfo.playerInfoModel.GetPlayerDistanceRecord() < playerFinalDistance)
        {
            IsNewRecord();
            gameOverExpRecord = playerFinalDistance;
        } 
        else
        {
            gameOverExpRecord = GlobalPlayerInfo.playerInfoModel.GetPlayerDistanceRecord();
        }

        AudioController.Instance.PlayClip("GameOver");
        gameOverMenuUIAnimator.SetBool("isGameOver", true);
        Dictionary<TMP_Text, float> displayItems = new Dictionary<TMP_Text, float>()
        {
            { gameOverDistanceText, GlobalPlayerInfo.playerInfoModel.PlayerDistance},
            { gameOverExpRecordText, gameOverExpRecord },
            { gameOverCoinsText, GlobalPlayerInfo.playerInfoModel.PlayerCoins},
            { gameOverSpecialCoinsText, GlobalPlayerInfo.playerInfoModel.PlayerSpecialCoins},
            { gameOverExpText, GlobalPlayerInfo.playerInfoModel.GetFinalResultExp()}
        };
        StartCoroutine(DisplayScores(displayItems));
        //CommertialServiceControl.SetActionOnOpenAds(()=> { SetVisibleButtonGameOver(false); });
        //CommertialServiceControl.SetActionOnCloseAds(() => { SetVisibleButtonGameOver(true); });
        CheckAndGetGameOverInfoForPlayer();
    }

    private IEnumerator DisplayScores(Dictionary<TMP_Text, float> displayItems)
    {
        float duration = 1f;// Время анимации для каждого параметра
        int currentIndex = 0;
        // Обрабатываем каждый параметр по очереди
        foreach (var field in displayItems)
        {
            float currentValue = 0f;
            float targetValue = field.Value;
            float timeElapsed = 0f;
            if(targetValue == 0)
            {
                currentIndex++;
                continue;
            }

            while (timeElapsed < duration)
            {
                currentValue = Mathf.Lerp(0f, targetValue, timeElapsed / duration);
                field.Key.text = Mathf.RoundToInt(currentValue).ToString();
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            field.Key.text = Mathf.RoundToInt(targetValue).ToString();

            currentIndex++;

            if (currentIndex >= displayItems.Count)
                break;
        }
    }

    private void IsNewRecord()
    {
        gameOverRecordNewImg.gameObject.SetActive(true);
    }
    
    private void SetVisibleButtonGameOver(bool isVisible)
    {
        gameOverMenuUIBtn.SetActive(isVisible);
    }

    private void CheckAndGetGameOverInfoForPlayer()
    {
        gameOverInfoText.gameObject.SetActive(false);
        if (!ProjectContext.instance.AchievementManager.IsAchievementUnlock(GlobalAchievements.FLY_IN_CLOUD))
        {
            if (ProjectContext.instance.DataBaseRepository.PlayerFeaturesRepos.GetPlayerLevel() > 6 
                && Random.Range(0, 10) > 4)
            {
                gameOverInfoText.gameObject.SetActive(true);
                gameOverInfoText.text = ProjectContext.instance.DataBaseRepository.UITranslatorRepos.allItems["GameOverInfo_PlayerCanFlyInCloud"].Description;
            }
        }
    }
}
