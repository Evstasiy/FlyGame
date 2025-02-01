using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels.DataModelWorkers.GameItemInformation.GameWorker;
using Assets.Scripts.SGEngine.DataBase.Models;
using Assets.Scripts.SGEngine.Utilits;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinsView : MonoBehaviour
{
    [SerializeField]
    private GameObject mainUI;

    [SerializeField]
    private GameObject skinUI;

    [SerializeField]
    private GameObject worldObject;

    [SerializeField]
    private Button selectedOrBuySkinBtn;
    [SerializeField]
    private TMP_Text actionSelectSkinText;

    [SerializeField]
    private TMP_Text nameSkinText;
    [SerializeField]
    private TMP_Text descriptionSkinText;
    [SerializeField]
    private TMP_Text mainPriceSkinText;
    [SerializeField]
    private TMP_Text secondPriceSkinText;

    /*Positions*/
    [SerializeField]
    private Transform positionNext;
    [SerializeField]
    private Transform positionBefore;
    [SerializeField]
    private Transform positionTarget;


    private CircularQueue<SkinModelObject> allSkins;

    private SkinModelObject targetSkinObj;
    private SkinModelObject nextSkinObj;
    private SkinModelObject previousSkinObj;

    private bool isUserCanEquipTargetSkin = false;

    private PlayerFeaturesRepository playerRepo => DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos;
    private SkinsRepository skinsRepo => DataBaseRepository.dataBaseRepository.SkinsRepo;
    private Dictionary<string, UIItem> uiItems => DataBaseRepository.dataBaseRepository.UITranslatorRepos.allItems;

    public void StartupSkins()
    {
        var skinModels = skinsRepo.allSkins;
        if (allSkins != null && allSkins?.Count() != 0)
        {
            SetTargetSkin();
            return;
        }
        allSkins = new CircularQueue<SkinModelObject>();
        foreach (var skinModel in skinModels)
        {
            var loadObj = Resources.Load<GameObject>(skinModel.PathToSkin);
            var skinObj = Instantiate(loadObj, Vector3.zero, new Quaternion(0, 180, 0, 0));
            var skinModelObject = skinObj.AddComponent<SkinModelObject>();
            skinModelObject.Model = skinModel;
            skinModelObject.gameObject.SetActive(false);
            allSkins.Enqueue(skinModelObject);
        }
        allSkins.Close();
        var playerSelectedSkinId = playerRepo.GetPlayerSelectedSkinId();
        SetSkinById(playerSelectedSkinId);
    }

    public void ViewNextSkin(bool isRight)
    {
        AudioController.Instance.PlayClip("Click");
        previousSkinObj = allSkins.GetPrevious();
        previousSkinObj.gameObject.SetActive(false);
        nextSkinObj = allSkins.GetNext();
        nextSkinObj.gameObject.SetActive(false);

        if (isRight)
        {

            allSkins.MoveNext();
        }
        else
        {
            allSkins.MovePrevious();
        }

        SetTargetSkin();
        nameSkinText.text = targetSkinObj.Model.Name;
        descriptionSkinText.text = targetSkinObj.Model.Description;
        SetStateSelectedButtonForTargeSkin(targetSkinObj.Model);
    }

    public void SelectOrBuyTargetSkin()
    {
        if (isUserCanEquipTargetSkin)
        {
            playerRepo.SetPlayerSelectedSkinId(targetSkinObj.Model.Id);
            AudioController.Instance.PlayClip("Click");
        } 
        else
        {
            playerRepo.AddPlayerMoney(targetSkinObj.Model.Price * -1);
            playerRepo.AddPlayerSpecialMoney(targetSkinObj.Model.PriceSpecialMoney * -1);
            skinsRepo.Add(targetSkinObj.Model);
            AudioController.Instance.PlayClip("Buy");
        }
        SetStateSelectedButtonForTargeSkin(targetSkinObj.Model);
    }
    private void SetTargetSkin()
    {
        targetSkinObj = allSkins.Current();
        targetSkinObj.gameObject.SetActive(true);
        targetSkinObj.transform.SetParent(positionTarget);
        targetSkinObj.transform.localPosition = new Vector3();

        previousSkinObj = allSkins.GetPrevious();
        previousSkinObj.transform.SetParent(positionBefore);
        previousSkinObj.gameObject.SetActive(true);
        previousSkinObj.transform.localPosition = new Vector3();

        nextSkinObj = allSkins.GetNext();
        nextSkinObj.transform.SetParent(positionNext);
        nextSkinObj.gameObject.SetActive(true);
        nextSkinObj.transform.localPosition = new Vector3();

        nameSkinText.text = targetSkinObj.Model.Name;
        descriptionSkinText.text = targetSkinObj.Model.Description;
        mainPriceSkinText.text = targetSkinObj.Model.Price.ToString();
        secondPriceSkinText.text = targetSkinObj.Model.PriceSpecialMoney.ToString();
        SetStateSelectedButtonForTargeSkin(targetSkinObj.Model);
    }

    private void SetSkinById(int skinId)
    {
        targetSkinObj?.gameObject.SetActive(false);
        for (int i = 0; i < allSkins.Count(); i++)
        {
            if (allSkins.Current().Model.Id == skinId)
            {
                break;
            } else
            {
                allSkins.MoveNext();
            }
        }
        SetTargetSkin();
        previousSkinObj?.gameObject.SetActive(false);
        nextSkinObj?.gameObject.SetActive(false);
    }

    #region UI
    public void SetActiveUI(bool isActive)
    {
        AudioController.Instance.PlayClip("Click");
        mainUI.SetActive(!isActive);
        skinUI.SetActive(isActive);
        worldObject.SetActive(!isActive);

        previousSkinObj?.gameObject.SetActive(isActive);
        nextSkinObj?.gameObject.SetActive(isActive);
        if (isActive)
        {
            nameSkinText.text = targetSkinObj.Model.Name;
            descriptionSkinText.text = targetSkinObj.Model.Description;
            SetStateSelectedButtonForTargeSkin(targetSkinObj.Model);
        } 
        else
        {
            var playerSelectedSkinId = playerRepo.GetPlayerSelectedSkinId();
            if(targetSkinObj.Model.Id != playerSelectedSkinId)
            {
                SetSkinById(playerSelectedSkinId);
            }
        }
    }

    private void SetStateSelectedButtonForTargeSkin(SkinModel skinModel)
    {
        if(skinModel.Id == playerRepo.GetPlayerSelectedSkinId())
        {
            selectedOrBuySkinBtn.interactable = false;
            actionSelectSkinText.text = uiItems["EqipmentSkinText"].Description;
        }
        else if(DataBaseRepository.dataBaseRepository.SkinsRepo.saveSkins.Any(x => x.Id == skinModel.Id))
        {
            selectedOrBuySkinBtn.interactable = true;
            actionSelectSkinText.text = uiItems["SelectSkinText"].Description;
            isUserCanEquipTargetSkin = true;
        }
        else if (skinModel.Price <= playerRepo.GetPlayerMoney() &&
            skinModel.PriceSpecialMoney <= playerRepo.GetPlayerSpecialMoney() &&
            skinModel.LvlToUnlock <= playerRepo.GetPlayerLevel())
        {
            selectedOrBuySkinBtn.interactable = true;
            actionSelectSkinText.text = uiItems["BuySkinText"].Description;
            isUserCanEquipTargetSkin = false;
        } 
        else
        {
            selectedOrBuySkinBtn.interactable = false;
            actionSelectSkinText.text = uiItems["BuySkinText"].Description;
        }
    }
    #endregion UI
}
