using Assets.Scripts.MainGame.Models;
using Assets.Scripts.MainGame.Player;
using Assets.Scripts.SGEngine.DataBase.Models;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text playerLayerWorldText;
    [SerializeField]
    private Animation playerLayerWorldAnimation;

    [SerializeField]
    private TMP_Text playerSpeedText;
    [SerializeField]
    private TMP_Text playerCoinText;
    [SerializeField]
    private TMP_Text playerSpecialCoinText;
    [SerializeField]
    private TMP_Text playerDistanceText;

    [SerializeField]
    private RectTransform playerSpeedLineTransform;
    [SerializeField]
    private TMP_Text playerMaxSpeedText;

    [SerializeField]
    private TMP_Text NotifyPlayerLayerHasChangeTextTMP;
    [SerializeField]
    private TMP_Text playerNotifyEndYPosText;

    [SerializeField]
    private Animator playerCoinAnimator;
    [SerializeField]
    private Animator playerSpecialCoinAnimator;
    [SerializeField]
    private PlayerItemsController playerItemsController;
    [SerializeField]
    private GameObject pickItemsInfoNotifyContent;
    [SerializeField]
    private PickItemInfoNotifyItemModel pickItemsInfoNotifyItem;


    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;
    private float maxPlayerSpeedWithUpdates => GlobalPlayerInfo.playerInfoModel.GetMaxPlayerSpeedWithUpdates();
    private Dictionary<string, UIItem> uiItems => ProjectContext.instance.DataBaseRepository.UITranslatorRepos.allItems;


    void Start()
    {
        GlobalPlayerInfo.playerInfoModel.UserCoinsChange += UserCoinsChange;
        GlobalPlayerInfo.playerInfoModel.UserSpecialCoinsChange += UserSpecialCoinsChange;
        ProjectContext.instance.PlayerController.OnPlayerPositionYChange += PlayerPositionYChange;
        playerItemsController.IsItemPickUp += UserItemPickUp;
        playerMaxSpeedText.text = maxPlayerSpeedWithUpdates.ToString();
        playerCoinText.text = "0";
        playerSpecialCoinText.text = "0";
    }

    void Update()
    {
        if(isPaused)
        {
            return;
        }

        playerSpeedText.text = GlobalPlayerInfo.playerInfoModel.FinalSpeed.ToString("0");
        playerDistanceText.text = GlobalPlayerInfo.playerInfoModel.PlayerDistance.ToString("0") + "/" + ProjectContext.WORLD_DISTANCE;

        //Спидометр
        /*float normalizedSpeed = Mathf.Clamp(GlobalPlayerInfo.playerInfoModel.FinalSpeed / maxPlayerSpeedWithUpdates, 0f, 1f);
        float angle = Mathf.Lerp(90, -90, normalizedSpeed);
        playerSpeedLineTransform.rotation = Quaternion.Euler(0f, 0f, angle);*/
    }

    private void PlayerPositionYChange(float newPositionY)
    {
        if(newPositionY < ProjectContext.MIN_POS_Y + ProjectContext.STEP_TO_END_POS_Y || newPositionY > ProjectContext.MAX_POS_Y - ProjectContext.STEP_TO_END_POS_Y)
        {
            if (newPositionY < ProjectContext.MIN_POS_Y + ProjectContext.STEP_TO_END_POS_Y)
            {
                playerNotifyEndYPosText.text = uiItems["LowNotifyPlayerText"].Description;
                playerNotifyEndYPosText.alignment = TextAlignmentOptions.Bottom;
            } 
            else
            {
                playerNotifyEndYPosText.text = uiItems["HighNotifyPlayerText"].Description;
                playerNotifyEndYPosText.alignment = TextAlignmentOptions.Top;
            }
            playerNotifyEndYPosText.gameObject.SetActive(true);
        }
        else
        {
            playerNotifyEndYPosText.gameObject.SetActive(false);
        }
        NotifyPlayerLayerHasChangeTextTMP.gameObject.SetActive(false);
    }

    private void UserCoinsChange(int userCoins)
    {
        playerCoinText.text = userCoins.ToString();
        playerCoinAnimator.Play("UIScale", -1, 0f);
    }
    
    private void UserSpecialCoinsChange(int userSpecialCoins)
    {
        playerSpecialCoinText.text = userSpecialCoins.ToString();
        playerSpecialCoinAnimator.Play("UIScale", -1, 0f);
    }
    
    private void UserItemPickUp(ItemData item)
    {
        var obj = Instantiate(pickItemsInfoNotifyItem, Vector3.zero, Quaternion.identity, pickItemsInfoNotifyContent.transform);
        obj.SetItemData(item);
        Destroy(obj.gameObject, 0.8f);
    }

    public void ActiveZoneIsChanged(LayerWorldModel layerWorldModel)
    {
        if(uiItems.TryGetValue(layerWorldModel.LayerName.ToString(),out UIItem uiItem))
        {
            playerLayerWorldText.text = uiItem.Description;
            playerLayerWorldAnimation?.Play();
        }
    }
    
    public void ApproachedToNewActiveZone(LayerWorldModel layerWorldModel, bool isHigh)
    {
        if(uiItems.TryGetValue(layerWorldModel.LayerName.ToString(),out UIItem uiItem))
        {
            NotifyPlayerLayerHasChangeTextTMP.gameObject.SetActive(true);
            NotifyPlayerLayerHasChangeTextTMP.text = uiItem.Description;
            NotifyPlayerLayerHasChangeTextTMP.alignment = (isHigh) ? TextAlignmentOptions.Top : TextAlignmentOptions.Bottom;
        }
    }

    private void OnDestroy()
    {
        ProjectContext.instance.PlayerController.OnPlayerPositionYChange -= PlayerPositionYChange;
        GlobalPlayerInfo.playerInfoModel.UserCoinsChange -= UserCoinsChange;
        GlobalPlayerInfo.playerInfoModel.UserSpecialCoinsChange -= UserSpecialCoinsChange;
        playerItemsController.IsItemPickUp -= UserItemPickUp;
    }
}
