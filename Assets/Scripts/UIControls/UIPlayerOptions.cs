using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.Extensions;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.SGEngine.DataBase.LocalizationOption;

public class UIPlayerOptions : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown PlayerLanguageDropDown;

    [SerializeField]
    private Image PlayerMusicIcon;
    [SerializeField]
    private Sprite PlayerMusicIconOn;
    [SerializeField]
    private Sprite PlayerMusicIconOff;

    [SerializeField]
    private GameObject MainMenuUIObj;
    [SerializeField]
    private GameObject MainMenuDialogUI;
    [SerializeField]
    private GameObject FAQUIObj;
    [SerializeField]
    private GameObject TryResetSavesDialog;

    PlayerFeaturesRepository PlayerFeaturesRepo;

    void Start()
    {
        PlayerFeaturesRepo = DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos;
        PlayerFeaturesRepo.isRepositoryChange += UpdatePlayerOptions;
        UpdatePlayerOptions();

        PlayerLanguageDropDown.options.Clear();
        var localizationList = Enum.GetValues(typeof(LocalizationRegion)).Cast<LocalizationRegion>();
        PlayerLanguageDropDown.AddOptions(localizationList.Select(x => new TMP_Dropdown.OptionData(x.ToEnumMember())).ToList());
        FAQUIObj.SetActive(false);
    }

    private void UpdatePlayerOptions()
    {
        if (PlayerLanguageDropDown != null)
            PlayerLanguageDropDown.value = (int)PlayerFeaturesRepo.GetPlayerLanguage();
        if (PlayerMusicIcon != null)
            PlayerMusicIcon.sprite = (PlayerFeaturesRepo.GetPlayerIsMusic()) ? PlayerMusicIconOn : PlayerMusicIconOff;
    }

    public void OnLanguageChange(int language)
    {
        AudioController.Instance.PlayClip("Click");
        var languageName = (LocalizationRegion)language;
        PlayerFeaturesRepo.SetPlayerLanguage(languageName);
        UpdatePlayerOptions();
    }
    
    public void OnMusicChange()
    {
        var isMusic = PlayerFeaturesRepo.GetPlayerIsMusic();
        PlayerFeaturesRepo.SetPlayerPlayerIsMusic(!isMusic);
        UpdatePlayerOptions();
        AudioController.Instance.PlayClip("Click");
    }
    
    public void SetMenuActive(bool isActive)
    {
        AudioController.Instance.PlayClip("Click");
        MainMenuUIObj.SetActive(isActive);
    }
    
    public void SetFAQActive(bool isActive)
    {
        AudioController.Instance.PlayClip("Click");
        FAQUIObj.SetActive(isActive);
        MainMenuDialogUI.SetActive(!isActive);
    }
    
    public void TryResetSaves(bool isActive)
    {
        AudioController.Instance.PlayClip("Click");
        TryResetSavesDialog.SetActive(isActive);
        MainMenuDialogUI.SetActive(!isActive);
    }

}
