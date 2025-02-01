using System.Collections;
using UnityEngine;

public class AchievementUnlockView : MonoBehaviour
{
    [SerializeField]
    private Transform achievementSpawnPlace;
    [SerializeField]
    private AchievementUIItem achievementUiNotifyItem;

    private AchievementManager achievementManager;

    void Start()
    {
        achievementManager = (ProjectContext.instance == null) ? WorldEventManager.worldManager.AchievementManager : ProjectContext.instance.AchievementManager;

        achievementManager.NewAchievementUnlock += NewAchievementUnlock;
    }

    private void OnDestroy()
    {
        achievementManager.NewAchievementUnlock -= NewAchievementUnlock;
    }

    void Update()
    {
        
    }

    void NewAchievementUnlock(AchievementModel model)
    {
        var newAchivementUi = Instantiate(achievementUiNotifyItem, achievementSpawnPlace);
        newAchivementUi.SetModel(model);
        StartCoroutine(DestroyUIAchievement(newAchivementUi, 4f));
        AudioController.Instance?.PlayClip("GetAchievement");
    }
    private IEnumerator DestroyUIAchievement(AchievementUIItem achievementUI, float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);
        achievementUI.GetComponent<Animation>().Play("UIAchievementClose");
        Destroy(achievementUI.gameObject, 0.5f);
    }
}
