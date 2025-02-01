using Assets.Scripts.MainGame.Player;
using Assets.Scripts.MainGame.World;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.Models;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*FIX!!!
 Весь класс переписать
*/
public class TutorialController : MonoBehaviour
{
    public event TutorialDone IsTutorialDone;
    public delegate void TutorialDone();

    [SerializeField]
    private GameObject[] uiElementsNeedToDisabled;
    [SerializeField]
    private GameObject[] uiElements;
    [SerializeField]
    private TMP_Text tutorialText;
    [SerializeField]
    private Button OkButton;
    [SerializeField]
    private Button tutorialButtonInMainMeny;
    [SerializeField]
    private TMP_Text OkButtonText;

    [SerializeField]
    private RotateObject rotatePlayerInMainMenu;
    private float lastRotateSpeed;

    [SerializeField]
    private InteractiveLayerController interactiveLayerController;
    [SerializeField]
    private GameObject playerFlyZone;
    private DataBaseRepository dataBaseRepository => DataBaseRepository.dataBaseRepository;

    private int counter = 0;
    private Coroutine activeCoroutineForViewPlayer = null;
    private Coroutine activeCoroutineForFrizePlayer = null;
    /// <summary>
    /// Костыль чтобы показываь последний объект из корутины с эффектом мерцания
    /// </summary>
    private GameObject lastGameObjectInShowCoroutine = null;

    private readonly int minPlayerFlyZone = 100;
    private readonly int maxPlayerFlyZone = 190;
    
    void Start()
    {
    }

    public void CheckAndPlayTutorial()
    {
        if (!DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.GetIsFinishedTutorial())
        {
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
        if(counter != 0)
        {
            AudioController.Instance.PlayClip("Click");
        }
        if (activeCoroutineForViewPlayer != null)
        {
            lastGameObjectInShowCoroutine.SetActive(true);
            lastGameObjectInShowCoroutine = null;
            StopCoroutine(activeCoroutineForViewPlayer);
        }
        switch (counter)
        {
            case 0:
                break;
            case 1:
                var lvlPanel = uiElements.FirstOrDefault(x => x.name == "LevelPanel");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(lvlPanel));
                break;
            case 2:
                var coinsPanel = uiElements.FirstOrDefault(x => x.name == "MainMoneyBackground");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(coinsPanel));
                break;
            case 3:
                var specialCoinsPanel = uiElements.FirstOrDefault(x => x.name == "SpecialMoneyBackground");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(specialCoinsPanel));
                break;
            case 4:
                var openMenuUIBtn = uiElements.FirstOrDefault(x => x.name == "OpenMenuUIBtn");
                openMenuUIBtn.GetComponent<Button>().interactable = false;
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(openMenuUIBtn));
                break;
            case 5:
                var updateBoostBtn = uiElements.FirstOrDefault(x => x.name == "UpdateBoostBtn");
                updateBoostBtn.GetComponent<Button>().interactable = false;
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(updateBoostBtn));
                break;
            case 6:
                var shopBtn = uiElements.FirstOrDefault(x => x.name == "ShopBtn");
                shopBtn.GetComponent<Button>().interactable = false;
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(shopBtn));
                break;
            case 7:
                var achivmentsBtn = uiElements.FirstOrDefault(x => x.name == "AchivmentsBtn");
                achivmentsBtn.GetComponent<Button>().interactable = false;
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(achivmentsBtn));
                break;
            case 8:
                var skinShopBtn = uiElements.FirstOrDefault(x => x.name == "SkinShopBtn");
                skinShopBtn.GetComponent<Button>().interactable = false;
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(skinShopBtn));
                break;
            case 9:
                var playBtn = uiElements.FirstOrDefault(x => x.name == "PlayBtn");
                playBtn.GetComponent<Button>().interactable = false;
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(playBtn));
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

        interactiveLayerController.IsAutoSpawnObjects = false;

        counter = 50;

        if(activeCoroutineForFrizePlayer != null)
        {
            StopCoroutine(activeCoroutineForFrizePlayer);
        }
        activeCoroutineForFrizePlayer = StartCoroutine(FrizePlayerInZone());
        StartCoroutine(SetPauseGameAfterTime(2f));
        
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
        int[] notClickCounters = new int[] { 50, 58, 60, 62, 65, 68 };
        if (!notClickCounters.Any(x=>x == counter))
        {
            AudioController.Instance.PlayClip("Click");
        }
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
                break;
            case 51:
                var menuUIButton = uiElements.FirstOrDefault(x => x.name == "MenuUIButton");
                menuUIButton.GetComponent<Button>().interactable = false;
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(menuUIButton));
                break;
            case 52:
                var playerLayerWorldText = uiElements.FirstOrDefault(x => x.name == "PlayerLayerWorldText");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(playerLayerWorldText));
                break;
            case 53:
                var tracePanel = uiElements.FirstOrDefault(x => x.name == "TracePanel");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(tracePanel));
                break;
            case 54:
                var playerInfoUI = uiElements.FirstOrDefault(x => x.name == "PlayerInfoUI");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(playerInfoUI));
                break;
            case 55:
                var speedPlayerPanel = uiElements.FirstOrDefault(x => x.name == "SpeedPlayerPanel");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(speedPlayerPanel));
                break;
            case 56:
                var effectIconsPanel = uiElements.FirstOrDefault(x => x.name == "EffectIconsPanel");
                activeCoroutineForViewPlayer = StartCoroutine(ShowPlayerObject(effectIconsPanel));

                break;
            case 57:
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                counter++;
                StartCoroutine(ContinueTutorialAfterTime(5f));
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(100f);
                return;

            case 58:
                interactiveLayerController.SpawnObjectByObjectTypeAndPosition(InteractiveObjectEnum.MassNormalBirds, new Vector3(50, ProjectContext.instance.PlayerController.gameObject.transform.position.y, ProjectContext.instance.PlayerController.gameObject.transform.position.z));
                break;
            case 59:
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                counter++;
                StartCoroutine(ContinueTutorialAfterTime(5f));
                return;
            case 60:
                var objBoostSpeed = interactiveLayerController.SpawnObjectByObjectTypeAndPosition(InteractiveObjectEnum.BoostSpeed, new Vector3(50, ProjectContext.instance.PlayerController.gameObject.transform.position.y, ProjectContext.instance.PlayerController.gameObject.transform.position.z));
                ShowPlayerObject(uiElements.FirstOrDefault(x => x.name == "SpeedPlayerPanel"));
                objBoostSpeed.GetComponent<IInteractiveObjectBase>().SetTargetToMove(ProjectContext.instance.PlayerController.gameObject.transform);
                break;
            case 61:
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                counter++;
                StartCoroutine(ContinueTutorialAfterTime(4f));
                return;
            case 62:
                break;
            case 63:
                interactiveLayerController.SpawnObjectByObjectTypeAndPosition(InteractiveObjectEnum.NormalBirds, new Vector3(70, ProjectContext.instance.PlayerController.gameObject.transform.position.y, ProjectContext.instance.PlayerController.gameObject.transform.position.z));
                break;
            case 64:
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                counter++;
                StartCoroutine(ContinueTutorialAfterTime(4f));
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(100f);
                return;
            case 65:
                break;
            case 66:
                interactiveLayerController.SpawnObjectByObjectTypeAndPosition(InteractiveObjectEnum.MassCoins, new Vector3(70, ProjectContext.instance.PlayerController.gameObject.transform.position.y, ProjectContext.instance.PlayerController.gameObject.transform.position.z));
                break;
            case 67:
                ProjectContext.instance.PauseManager.SetPause(false);
                tutorialText.gameObject.SetActive(false);
                OkButton.gameObject.SetActive(false);
                SetActiveInMainGame(true, false);
                StartCoroutine(ContinueTutorialAfterTime(4f));
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(100f);
                break;
            case 68:

                break;
            default:
                EndTutorialInGame();
                break;
        }
        counter++;
    }
    private void EndTutorialInGame()
    {
        ProjectContext.instance.PauseManager.SetPause(false);
        counter = 49;
        SetActiveInMainGame(true);
        tutorialText.gameObject.SetActive(true);
        OkButton.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        playerFlyZone.SetActive(false);
        interactiveLayerController.IsAutoSpawnObjects = true;
        DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos.SetPlayerIsFinishedTutorial(true);
        IsTutorialDone?.Invoke();
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
        yield return new WaitForSeconds(0.1f);
        OkButton.interactable = true;
        OkButtonText.text = "Ok";
    }
    
    private IEnumerator FrizePlayerInZone()
    {
        while (true)
        {
            var playerPos = ProjectContext.instance.PlayerController.gameObject.transform.position;
            if (playerPos.y > maxPlayerFlyZone)
            {
                ProjectContext.instance.PlayerController.gameObject.transform.position = new Vector3(playerPos.x, maxPlayerFlyZone, playerPos.z);
            }
            if (playerPos.y < minPlayerFlyZone)
            {
                ProjectContext.instance.PlayerController.gameObject.transform.position = new Vector3(playerPos.x, minPlayerFlyZone, playerPos.z);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    private IEnumerator ContinueTutorialAfterTime(float timeToSetPause)
    {
        yield return new WaitForSeconds(timeToSetPause);
        tutorialText.gameObject.SetActive(true);
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

}
