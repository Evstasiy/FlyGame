using Assets.Scripts.MainGame.Player;
using Assets.Scripts.MainGame.World;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers;
using Assets.Scripts.SGEngine.DataBase.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MarketManager;

/*FIX!!!
 Весь класс переписать
*/
public class TutorialController : MonoBehaviour
{
    public event TutorialDone IsTutorialDone;
    public delegate void TutorialDone();

    [SerializeField]
    private Image tutorialArrow;
    [SerializeField]
    private GameObject[] uiElementsNeedToDisabled;
    [SerializeField]
    private GameObject[] uiElements;
    [SerializeField]
    private TMP_Text tutorialText;
    [SerializeField]
    private GameObject tutorialBackImg;
    [SerializeField]
    private Button OkButton;
    [SerializeField]
    private Button tutorialButtonInMainMeny;
    [SerializeField]
    private TMP_Text OkButtonText;
    [SerializeField]
    private GameObject playerControllObject;

    [SerializeField]
    private MarketManager marketManagerUpdate;


    [SerializeField]
    private RotateObject rotatePlayerInMainMenu;
    private float lastRotateSpeed;

    [SerializeField]
    private InteractiveLayerController interactiveLayerController;
    [SerializeField]
    private LayerController layerController;
    [SerializeField]
    private GameObject playerFlyZone;
    [SerializeField]
    private GameObject playerFlyZoneTop;
    [SerializeField]
    private PlayerItemsController playerItemsController;
    [SerializeField]
    private PlayerEffectController playerEffectController;
    private DataBaseRepository dataBaseRepository => DataBaseRepository.dataBaseRepository;

    private int counter = 0;
    private Coroutine activeCoroutineForViewPlayer = null;
    private Coroutine activeCoroutineForFrizePlayer = null;
    private Coroutine activeCoroutineForGetPlayerSpawn = null;
    /// <summary>
    /// Костыль чтобы показываь последний объект из корутины с эффектом мерцания
    /// </summary>
    private GameObject lastGameObjectInShowCoroutine = null;

    private readonly int minPlayerFlyZone = 100;
    private readonly int maxPlayerFlyZone = 190;

    private readonly int minPlayerFlyZoneCloud = 270;
    private readonly int maxPlayerFlyZoneCloud = 370;
    
    public void StartTutorialAgain()
    {
        DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.SetPlayerIsFinishedTutorial(false);
        DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.SetPlayerTutorialPoint(0);
        StartTutorialInMainMeny();
    }

    public void CheckAndPlayTutorial()
    {
        if (!DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetIsFinishedTutorial())
        {
            var point = DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetPlayerTutorialPoint();
            switch (point)
            {
                case 0:
                    counter = 0;
                    break;
                case 1:
                    counter = 50;
                    break;
                case 2:
                    counter = 3;
                    break;
                case 3:
                    counter = 65;
                    break;
            }

            if (SceneManager.GetActiveScene().name == "FlyGame")
            {
                StartTutorialInGame();
            } 
            else
            {
                StartTutorialInMainMeny();
            }
        }
    }

    public void StartTutorialInMainMeny()
    {
        this.gameObject.SetActive(true);
        if (DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetIsFinishedTutorial()) 
        { 
            DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.SetPlayerIsFinishedTutorial(false);
        }
        lastRotateSpeed = rotatePlayerInMainMenu.rotationSpeed;
        rotatePlayerInMainMenu.rotationSpeed = 0;

        uiElements.FirstOrDefault(x => x.name == "MenuUI")?.SetActive(false);
        foreach (var element in uiElementsNeedToDisabled)
        {
            element.SetActive(false);
        }
        tutorialText.text = string.Empty;
        NextTutorialInMainMeny();
    }
    
    private void EndTutorialInMainMeny()
    {
        rotatePlayerInMainMenu.rotationSpeed = lastRotateSpeed;
        counter = -1;
        foreach (var element in uiElementsNeedToDisabled)
        {
            element.SetActive(true);
            if(element.TryGetComponent(out Button button))
            {
                button.interactable = true;
            }
        }
        tutorialButtonInMainMeny.interactable = false;
        this.gameObject.SetActive(false);
    }

    public void NextTutorialInMainMeny()
    {
        OkButton.interactable = false;
        StartCoroutine(DisableOkButton());
        dataBaseRepository.UITranslatorRepos.allItems.TryGetValue("Tutorial_" + counter, out UIItem uIItem);
        string nextText = (uIItem == null) ? string.Empty : uIItem.Description;
        tutorialText.text = nextText;
        if (counter != 0)
        {
            AudioController.Instance?.PlayClip("Click");
        }
        if (activeCoroutineForViewPlayer != null)
        {
            StopCoroutine(activeCoroutineForViewPlayer);
        }

        var playBtn = uiElements.FirstOrDefault(x => x.name == "PlayBtn");
        var playerUpdateBtn = uiElements.FirstOrDefault(x => x.name == "PlayerUpdatesBtn");
        var boostBtn = uiElements.FirstOrDefault(x => x.name == "BoostsBtn");


        switch (counter)
        {
            case 0:
                break;
            case 1:
                playBtn.SetActive(true);
                OkButton.gameObject.SetActive(false);
                DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.SetPlayerTutorialPoint(1);
                break;
            case 3:
                marketManagerUpdate.OnMarketOpen += OnMarketOpen;
                marketManagerUpdate.OnMarketClose += OnMarketClose;
                marketManagerUpdate.OnItemBuy += OnItemBuy;
                var mainUI = uiElements.FirstOrDefault(x => x.name == "MainUI");
                mainUI.SetActive(true);
                OkButton.gameObject.SetActive(false);
                playBtn.SetActive(true);
                playBtn.GetComponent<Button>().interactable = false;
                playerUpdateBtn.SetActive(true);
                var playerUpdate = DataBaseRepository.dataBaseRepository.BoostObjectsRepos.saveUpgradeBoostPlayerItems.FirstOrDefault(x => x.Id == 3);
                if (playerUpdate?.UserCount == 0 || playerUpdate == null)
                {
                    playerUpdateBtn.GetComponent<Button>().interactable = true;
                    var costPlayerBoost = DataBaseRepository.dataBaseRepository.BoostObjectsRepos.allUpgradeBoostPlayerItems.FirstOrDefault(x => x.Id == 3).BasePrice;
                    var needPlayerMoney = costPlayerBoost - DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetPlayerMoney();
                    if (needPlayerMoney >= 0)
                    {
                        DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.AddPlayerMoney(Math.Abs(needPlayerMoney));
                    }
                }
                else
                {
                    playerUpdateBtn.GetComponent<Button>().interactable = false;
                    counter = 5;
                    NextTutorialInMainMeny();
                }
                return;
            case 4:
                
                break;
            case 5:
                tutorialText.gameObject.SetActive(true);
                playerUpdateBtn.GetComponent<Button>().interactable = false;
                boostBtn.SetActive(true);
                if (!DataBaseRepository.dataBaseRepository.UpgradBoostObjectsRepos.saveUpgradeBoostItems.Any(x => x.Id == 20))
                {
                    if (DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetPlayerMoney() < 40)
                    {
                        DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.AddPlayerMoney(40);
                    }
                    boostBtn.GetComponent<Button>().interactable = true;
                }
                else
                {
                    boostBtn.GetComponent<Button>().interactable = false;
                    counter = 7;
                    activeCoroutineForGetPlayerSpawn = StartCoroutine(DoActionWithTime(1.5f,()=>NextTutorialInMainMeny()));
                }
                return;
            case 6:
                break;
            case 7:
                if(activeCoroutineForGetPlayerSpawn != null)
                {
                    StopCoroutine(activeCoroutineForGetPlayerSpawn);
                }
                tutorialText.gameObject.SetActive(true);
                OkButton.gameObject.SetActive(false);
                boostBtn.GetComponent<Button>().interactable = false;
                playBtn.GetComponent<Button>().interactable = true;
                DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.SetPlayerTutorialPoint(3);
                break;
            default:
                EndTutorialInMainMeny();
                break;
        }
        counter++;
    }
    
    public void StartTutorialInGame()
    {
        gameObject.SetActive(true);
        var playerPos = ProjectContext.instance.PlayerController.gameObject.transform.position;
        ProjectContext.instance.PlayerController.gameObject.transform.position = new Vector3(playerPos.x, minPlayerFlyZone + ((maxPlayerFlyZone - minPlayerFlyZone)/2f), playerPos.z);

        playerFlyZone.SetActive(true);
        playerControllObject.SetActive(false);

        interactiveLayerController.IsAutoSpawnObjects = false;
        playerItemsController.IsItemPickUp += PlayerPickUpItem;

        if(activeCoroutineForFrizePlayer != null)
        {
            StopCoroutine(activeCoroutineForFrizePlayer);
        }
        activeCoroutineForFrizePlayer = StartCoroutine(FrizePlayerInZone(maxPlayerFlyZone, minPlayerFlyZone));
        StartCoroutine(SetPauseGameAfterTime(0.5f));

        tutorialBackImg.SetActive(false);
        tutorialText.gameObject.SetActive(false);
        OkButton.gameObject.SetActive(false);

        uiElements.FirstOrDefault(x => x.name == "EffectIconsPanel").GetComponent<Image>().enabled = true;

        uiElements.FirstOrDefault(x => x.name == "MenuUI")?.SetActive(false);
        foreach (var element in uiElementsNeedToDisabled)
        {
            element.SetActive(false);
        }
        tutorialText.text = string.Empty;
        NextTutorialInGame();
    }

    public void NextTutorialInGame()
    {
        OkButton.interactable = false;
        StartCoroutine(DisableOkButton());
        dataBaseRepository.UITranslatorRepos.allItems.TryGetValue("Tutorial_" + counter, out UIItem uIItem);
        string nextText = (uIItem == null) ? string.Empty : uIItem.Description;
        tutorialText.text = nextText;
        if (activeCoroutineForViewPlayer != null)
        {
            if(lastGameObjectInShowCoroutine != null)
            {
                lastGameObjectInShowCoroutine.SetActive(true);
                lastGameObjectInShowCoroutine = null;
            }
            StopCoroutine(activeCoroutineForViewPlayer);
        }
        switch (counter)
        {
            case 50:
                uiElements.FirstOrDefault(x => x.name == "MenuUIButton").GetComponent<Button>().interactable = false;
                break;
            case 51:
                playerControllObject.SetActive(true);
                interactiveLayerController.SpawnObjectByObjectTypeAndPosition(InteractiveObjectEnum.MassNormalBirds, new Vector3(100, ProjectContext.instance.PlayerController.gameObject.transform.position.y, ProjectContext.instance.PlayerController.gameObject.transform.position.z));
                if(playerEffectController.GetActiveEffectObjectModels().Any(x=>x.EffectType == EffectEnum.PlayerMobility))
                {
                    playerEffectController.RemoveEffect(playerEffectController.GetActiveEffectObjectModels().FirstOrDefault(x => x.EffectType == EffectEnum.PlayerMobility));
                }
                break;
            case 52:
                GlobalPlayerInfo.playerInfoModel.SetPlayerDistance(0);
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                tutorialBackImg.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                StartCoroutine(ContinueTutorialAfterTime(5f));
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(100f);
                counter--;
                return;
            case 53:
                playerControllObject.SetActive(false);
                var playerInfoUI = uiElements.FirstOrDefault(x => x.name == "PlayerInfoUI");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(playerInfoUI));
                break;
            case 54:
                interactiveLayerController.SpawnObjectByObjectTypeAndPosition(InteractiveObjectEnum.MassCoins, new Vector3(70, ProjectContext.instance.PlayerController.gameObject.transform.position.y, ProjectContext.instance.PlayerController.gameObject.transform.position.z));
                break;
            case 55:
                GlobalPlayerInfo.playerInfoModel.SetPlayerDistance(0);
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                tutorialBackImg.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                StartCoroutine(ContinueTutorialAfterTime(4f));
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(100f);
                counter--;
                return;
            case 56:
                //Это скорость. Чем она ниже, тем сложнее лететь вверх.
                var speedUI = uiElements.FirstOrDefault(x => x.name == "SpeedPlayerPanel");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(speedUI));
                break;
            case 57:
                interactiveLayerController.SpawnObjectByObjectTypeAndPosition(InteractiveObjectEnum.MassBoostSpeed, new Vector3(70, ProjectContext.instance.PlayerController.gameObject.transform.position.y, ProjectContext.instance.PlayerController.gameObject.transform.position.z));
                break;
            case 58:
                GlobalPlayerInfo.playerInfoModel.SetPlayerDistance(0);
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                tutorialBackImg.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                StartCoroutine(ContinueTutorialAfterTime(3f));
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(100f);
                counter--;
                return;
            case 59:
                //свободно полетай
                break;
            case 60:
                DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.SetPlayerTutorialPoint(2);
                StopTutorialInGame();
                break;

            case 65:
                var speedPlayerUI = uiElements.FirstOrDefault(x => x.name == "SpeedPlayerPanel");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(speedPlayerUI));
                OkButton.gameObject.SetActive(true);
                break;
            case 66:
                tutorialText.gameObject.SetActive(true);
                tutorialBackImg.SetActive(true);
                var objBoostSpeed = interactiveLayerController.SpawnObjectByObjectTypeAndPosition(InteractiveObjectEnum.BoostSpeed, new Vector3(50, ProjectContext.instance.PlayerController.gameObject.transform.position.y, ProjectContext.instance.PlayerController.gameObject.transform.position.z));
                objBoostSpeed.GetComponent<IInteractiveObjectBase>().SetTargetToMove(ProjectContext.instance.PlayerController.gameObject.transform);
                break;
            case 67:
                uiElements.FirstOrDefault(x => x.name == "MenuUIButton").GetComponent<Button>().interactable = false;
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                tutorialBackImg.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                StartCoroutine(ContinueTutorialAfterTime(3f));
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(200f);
                GlobalPlayerInfo.playerInfoModel.SetPlayerDistance(0);
                counter++;
                return;
            case 68:
                var effectIconsPanel = uiElements.FirstOrDefault(x => x.name == "EffectIconsPanel");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(effectIconsPanel));
                break;
            case 69:
                OkButton.gameObject.SetActive(true);
                StopCoroutine(activeCoroutineForFrizePlayer);
                activeCoroutineForFrizePlayer = StartCoroutine(FrizePlayerInZone(null, minPlayerFlyZone));
                playerFlyZoneTop.SetActive(false);
                tutorialArrow.gameObject.SetActive(true);
                //Взлететь до облаков
                break;
            case 70:
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                tutorialBackImg.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                activeCoroutineForGetPlayerSpawn = StartCoroutine(DoActionWithTime(5f, () => {
                    GlobalPlayerInfo.playerInfoModel.SetPlayerDistance(0);
                    GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(200f);
                }));
                layerController.OnActiveZoneChanged += ActiveZoneChanged;
                return;
            
            case 71:
                //Тут надо поставить мин и макс высоты на облаках
                ProjectContext.instance.PauseManager.SetPause(true);
                tutorialText.gameObject.SetActive(true);
                tutorialBackImg.SetActive(true);
                OkButton.gameObject.SetActive(true);
                tutorialArrow.gameObject.SetActive(false); 
                playerFlyZoneTop.SetActive(true);
                StopCoroutine(activeCoroutineForFrizePlayer); 
                activeCoroutineForFrizePlayer = StartCoroutine(FrizePlayerInZone(maxPlayerFlyZoneCloud, minPlayerFlyZoneCloud));
                GlobalPlayerInfo.playerInfoModel.SetPlayerDistance(0);
                break;
            case 72:
                interactiveLayerController.SpawnObjectByObjectTypeAndPosition(InteractiveObjectEnum.BigCoin, new Vector3(70, ProjectContext.instance.PlayerController.gameObject.transform.position.y, ProjectContext.instance.PlayerController.gameObject.transform.position.z));
                break;
            case 73:
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                tutorialBackImg.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                StartCoroutine(ContinueTutorialAfterTime(6f));
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(100f);
                counter--;
                return;
            case 74:
                //показать индикатор пользователя
                interactiveLayerController.SpawnObjectByObjectTypeAndPosition(InteractiveObjectEnum.NormalBirds, new Vector3(150, ProjectContext.instance.PlayerController.gameObject.transform.position.y, ProjectContext.instance.PlayerController.gameObject.transform.position.z));
                break;
            case 75:
                StopCoroutine(activeCoroutineForGetPlayerSpawn); 
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                tutorialBackImg.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(100f);
                StartCoroutine(ContinueTutorialAfterTime(6f));
                counter++;
                return;
            case 76:

                break;
            case 77:
                StopCoroutine(activeCoroutineForFrizePlayer);
                StopTutorialInGame();
                IsTutorialDone?.Invoke();
                DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.SetPlayerIsFinishedTutorial(true);
                break;
            default:
                break;
        }
        counter++;
    }
    private void StopTutorialInGame()
    {
        ProjectContext.instance.PauseManager.SetPause(false);
        SetActiveInMainGame(true);
        tutorialText.gameObject.SetActive(true);
        tutorialBackImg.SetActive(true);
        OkButton.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        playerFlyZone.SetActive(false);
        interactiveLayerController.IsAutoSpawnObjects = true;
        GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(100f);
    }
    
    private void SetActiveInMainGame(bool isActive,bool withInteractableButtons = true)
    {
        foreach (var element in uiElementsNeedToDisabled)
        {
            element.SetActive(isActive);
            if (element.TryGetComponent(out Button button) && withInteractableButtons)
            {
                button.interactable = isActive;
            }
        }
        /*FIX!!!*/
        uiElements.FirstOrDefault(x => x.name == "EffectIconsPanel").GetComponent<Image>().enabled = false;
    }

    private IEnumerator DisableOkButton()
    {
        OkButtonText.text = "...";
        yield return new WaitForSeconds(1.5f);
        OkButton.interactable = true;
        OkButtonText.text = "Ok";
    }
    
    private IEnumerator FrizePlayerInZone(int? maxPlayerFlyZone, int? minPlayerFlyZone)
    {
        while (true)
        {
            var playerPos = ProjectContext.instance.PlayerController.gameObject.transform.position;
            if (maxPlayerFlyZone.HasValue && playerPos.y > maxPlayerFlyZone)
            {
                ProjectContext.instance.PlayerController.gameObject.transform.position = new Vector3(playerPos.x, maxPlayerFlyZone.Value, playerPos.z);
            }
            if (minPlayerFlyZone.HasValue && playerPos.y < minPlayerFlyZone)
            {
                ProjectContext.instance.PlayerController.gameObject.transform.position = new Vector3(playerPos.x, minPlayerFlyZone.Value, playerPos.z);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    private IEnumerator ContinueTutorialAfterTime(float timeToSetPause)
    {
        yield return new WaitForSeconds(timeToSetPause);
        tutorialText.gameObject.SetActive(true);
        tutorialBackImg.SetActive(true);
        OkButton.gameObject.SetActive(true);
        ProjectContext.instance.PauseManager.SetPause(true);
        NextTutorialInGame();
    }
    
    private IEnumerator SetPauseGameAfterTime(float timeToSetPause)
    {
        yield return new WaitForSeconds(timeToSetPause);
        ProjectContext.instance.PauseManager.SetPause(true);
        tutorialText.gameObject.SetActive(true);
        OkButton.gameObject.SetActive(true);
        tutorialBackImg.SetActive(true);
    }
    
    private IEnumerator DoActionWithTime(float timeToDo, Action action)
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToDo);
            action?.Invoke();
        }
    }

    private IEnumerator ShowPlayerObject(GameObject obj)
    {
        lastGameObjectInShowCoroutine = obj;
        while (true)
        {
            obj.SetActive(!obj.activeSelf);
            yield return new WaitForSeconds(0.4f);
        }
    }

    private void PlayerPickUpItem(ItemData item)
    {
        if ((counter == 51 && item.Value > 0 && item.Key == ItemDataEnum.Coin)
                ||
                (counter == 54 && item.Value > 0 && item.Key == ItemDataEnum.Coin)
                ||
                (counter == 57 && item.Value > 0 && item.Key == ItemDataEnum.Speed)
                ||
                (counter == 72 && item.Value > 0 && item.Key == ItemDataEnum.Coin))
        {
            counter += 2;
        }
    } 

    private void ActiveZoneChanged(LayerWorldModel activeZone)
    {
        layerController.OnActiveZoneChanged -= ActiveZoneChanged;
        counter++;
        NextTutorialInGame();
    }

    private void OnItemBuy(IItem marketItem)
    {
        if(counter == 3 || marketItem.GetId() == 3)
        {
            counter = 4;
        }
        else if(counter == 5 || marketItem.GetId() == 20)
        {
            counter = 6;
        }
    }

    private void OnMarketClose(IMarketBase currentMarket, IList<GameObject> marketItems)
    {
        if (currentMarket.GetMarketModel().MarketType == EnumMarkets.BoostPlayerItemsMarket && counter == 4)
        {
            counter = 5;
            NextTutorialInMainMeny();
        } 
        else if (currentMarket.GetMarketModel().MarketType == EnumMarkets.UpdateBoostItemsMarket && counter == 6)
        {
            counter = 7;
            NextTutorialInMainMeny();
        } 
        else
        {
            tutorialText.gameObject.SetActive(true);
        }
    }

    private void OnMarketOpen(IMarketBase currentMarket, IList<GameObject> marketItems)
    {
        if (currentMarket.GetMarketModel().MarketType == EnumMarkets.BoostPlayerItemsMarket && counter == 3)
        {
            tutorialText.gameObject.SetActive(false);
            for (int i = 0; i < marketItems.Count; i++)
            {
                var item = marketItems[i].GetComponent<BoostPlayerItemMarketUI>();
                if (item.itemModel.Id != 3)
                {
                    item.SetItemUIState(Assets.Scripts.SGEngine.MarketFolder.EnumsMarket.EnumStatesItemMarket.Lock);
                }
            }

        }
        else if(currentMarket.GetMarketModel().MarketType == EnumMarkets.UpdateBoostItemsMarket && counter == 5)
        {
            tutorialText.gameObject.SetActive(false);
            for (int i = 0; i < marketItems.Count; i++)
            {
                var item = marketItems[i].GetComponent<UpgradeBoostGameItemUI>();
                if (item.gameItem.Id != 20)
                {
                    item.SetItemUIState(Assets.Scripts.SGEngine.MarketFolder.EnumsMarket.EnumStatesItemMarket.Lock);
                }
            }
        }
    }

    private void OnDisable()
    {
        if(playerItemsController != null)
        {
            playerItemsController.IsItemPickUp -= PlayerPickUpItem;
        }
        if (layerController != null)
        {
            layerController.OnActiveZoneChanged -= ActiveZoneChanged;
        }
        if(activeCoroutineForViewPlayer != null)
        {
            StopCoroutine(activeCoroutineForViewPlayer);
            activeCoroutineForViewPlayer = null;
        }
        if(activeCoroutineForFrizePlayer != null)
        {
            StopCoroutine(activeCoroutineForFrizePlayer);
            activeCoroutineForFrizePlayer = null;
        }
        if(activeCoroutineForGetPlayerSpawn != null)
        {
            StopCoroutine(activeCoroutineForGetPlayerSpawn);
            activeCoroutineForGetPlayerSpawn = null;
        }

        if (marketManagerUpdate != null)
        {
            marketManagerUpdate.OnMarketOpen -= OnMarketOpen;
            marketManagerUpdate.OnItemBuy -= OnItemBuy;
            marketManagerUpdate.OnMarketClose -= OnMarketClose;
        }
    }
    
}
