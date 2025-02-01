using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AchievementView : MonoBehaviour
{
    [SerializeField]
    private GameObject achievementUI;

    [SerializeField]
    private GameObject contentObject;

    [SerializeField]
    private AchievementUIItem contentObjectItem;

    [SerializeField]
    private GameObject scrollAchievementUI;
    [SerializeField]
    private GameObject leaderboardUI;

    [SerializeField]
    private Button achievementBtn;
    [SerializeField]
    private Button playersRecordBtn;

    private List<AchievementUIItem> initAchievements = new List<AchievementUIItem>();

    private AchievementRepository achievementRepository => DataBaseRepository.dataBaseRepository.AchievementRepos;

    public void OpenAchivementUI(bool isActive)
    {
        AudioController.Instance.PlayClip("Click");
        achievementUI.SetActive(isActive); 
        SetActiveAchivementUI(true);
    }

    public void SetActiveAchivementUI(bool isActive)
    {
        AudioController.Instance.PlayClip("Click");
        achievementBtn.interactable = false;
        playersRecordBtn.interactable = true;
        scrollAchievementUI.SetActive(isActive);
        leaderboardUI.SetActive(!isActive);
        if (isActive)
        {
            ReloadContent();
        }
    }

    public void SetActiveLeaderboardUI(bool isActive)
    {
        AudioController.Instance.PlayClip("Click");
        playersRecordBtn.interactable = false;
        achievementBtn.interactable = true;
        scrollAchievementUI.SetActive(!isActive);
        leaderboardUI.SetActive(isActive);
        if (isActive)
        {
            ReloadContent();
        }
    }

    private void ReloadContent()
    {
        bool isFirstOpen = initAchievements == null || initAchievements?.Count == 0;
        foreach (var item in achievementRepository.allAchievementItems)
        {
            var saveItem = achievementRepository.saveAchievementItems.FirstOrDefault(x => x.Id == item.Id);
            if (isFirstOpen)
            {
                var itemUI = Instantiate(contentObjectItem, Vector3.zero, Quaternion.identity, contentObject.transform);
                itemUI.SetModel(item);
                if (saveItem != null)
                {
                    itemUI.SetLock(false);
                }
                initAchievements.Add(itemUI);
            } 
            else
            {
                var itemUI = initAchievements.FirstOrDefault(x => x.AchievementId == item.Id);
                itemUI.SetModel(item);
                if (saveItem != null)
                {
                    itemUI.SetLock(false);
                }
            }
        }
    }
}
