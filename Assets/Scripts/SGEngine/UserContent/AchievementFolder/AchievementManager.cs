using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using System.Linq;
using UnityEngine;

public class AchievementManager
{
    public event IsNewAchievementUnlock NewAchievementUnlock;
    public delegate void IsNewAchievementUnlock(AchievementModel model);

    private AchievementRepository achievementRepository => DataBaseRepository.dataBaseRepository.AchievementRepos;

    public bool IsAchievementUnlock(int idUnlockAchievement)
    {
        return achievementRepository.saveAchievementItems.Any(x => x.Id == idUnlockAchievement);
    }

    public void UnlockAchievement(int idUnlockAchievement)
    {
        var isPlayerHasAchievement = achievementRepository.saveAchievementItems.Any(x=>x.Id == idUnlockAchievement);
        if (isPlayerHasAchievement)
        {
            //Debug.LogWarning("Player has achievementId: " + idUnlockAchievement);
            return;
        }
        var unlockAchievementModel = achievementRepository.allAchievementItems.FirstOrDefault(x=>x.Id == idUnlockAchievement);
        achievementRepository.Add(unlockAchievementModel);
        NewAchievementUnlock?.Invoke(unlockAchievementModel);
    }
}

