using Assets.Scripts.MainGame.Models;
using Assets.Scripts.MainGame.Player;
using System.Collections;
using UnityEngine;

public class GameRulesController : MonoBehaviour
{
    public event GameOverEvent IsGameOver;
    public delegate void GameOverEvent();

    private PlayerFeaturesRepository playerFeaturesRepository => ProjectContext.instance.DataBaseRepository.PlayerFeaturesRepos;

    [SerializeField]
    private TutorialController tutorialController;

    private Coroutine gameOverCoroutine;

    void Start()
    {
        ProjectContext.instance.PlayerController.OnPlayerPositionYChange += PlayerPositionYChange;
        tutorialController.CheckAndPlayTutorial();
    }

    private void PlayerPositionYChange(float newPositionY)
    {
        if (GlobalPlayerInfo.playerInfoModel.FinalSpeed == PlayerInfoModel.MIN_SPEED)
        {
            if(gameOverCoroutine == null)
            {
                gameOverCoroutine = StartCoroutine(WaitAndStartGameOver());
            }
        }
        if((newPositionY <= ProjectContext.MIN_POS_Y || newPositionY >= ProjectContext.MAX_POS_Y))
        {
            if (gameOverCoroutine != null)
            {
                StopCoroutine(gameOverCoroutine);
            }
            GameOver();
        }
    }
    private void OnDestroy()
    {
        ProjectContext.instance.PlayerController.OnPlayerPositionYChange -= PlayerPositionYChange;
    }

    private void GameOver()
    {
        IsGameOver?.Invoke();
        var playerFinalDistance = (int)GlobalPlayerInfo.playerInfoModel.PlayerDistance;

        ProjectContext.instance.PauseManager.SetPause(true);
        playerFeaturesRepository.AddPlayerMoney(GlobalPlayerInfo.playerInfoModel.PlayerCoins);
        playerFeaturesRepository.AddPlayerSpecialMoney(GlobalPlayerInfo.playerInfoModel.PlayerSpecialCoins);
        if (GlobalPlayerInfo.playerInfoModel.GetPlayerDistanceRecord() < playerFinalDistance)
        {
            CommertialServiceControl.SetRecord("FlyDistance", playerFinalDistance);
            playerFeaturesRepository.SetPlayerDistanceRecord(playerFinalDistance);
        }
        playerFeaturesRepository.AddPlayerExperience(GlobalPlayerInfo.playerInfoModel.GetFinalResultExp());
    }

    private IEnumerator WaitAndStartGameOver()
    {
        yield return new WaitForSeconds(2f);
        while (ProjectContext.instance.PauseManager.IsPause)
        {
            yield return new WaitForSeconds(0.2f);
        }
        GameOver();
    }
}
