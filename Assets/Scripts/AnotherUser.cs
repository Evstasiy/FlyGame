using Assets.Scripts.MainGame.Player;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnotherUser : MonoBehaviour
{
    public TMP_Text TimeToFly;
    public TMP_Text InfoInFlyGame;
    private float timeSec = 0;
    public float mobility = 0;
    public float baseDebuffSpeed = 0;
    public bool setMaxSpeedFlyMode = false;

    public AudioSource audioSource;

    public AudioClip music;
    public int ExpForAdd;
    public DataBaseRepository dataBase => DataBaseRepository.dataBaseRepository;
    private PlayerFeaturesRepository playerRepo => DataBaseRepository.dataBaseRepository.PlayerFeaturesRepos;
    void Start() 
    {
        if (ProjectContext.instance != null)
        {
            mobility = GlobalPlayerInfo.playerInfoModel.mobility;
        }
        /*var g = dataBaseRepository.GameItemRepository;
        var all = g.allGameItems;
        var allSave = g.allGameItemsSave;
        GameItem gam = new GameItem() { Id = 99, Name = "Kek" };
        GameItem gam1 = new GameItem() { Id = 7, Name = "Kektr5", IsLock = true };
        GameItem gam3 = new GameItem() { Id = 9, Name = "Kek5", IsLock = true };
        GameItem gam4 = new GameItem() { Id = 4, Name = "Ke98k5", IsLock = false };
        allSave.Add(gam);
        allSave.Add(gam1);
        allSave.Remove(allSave.First(x=>x.Id == 4));
        allSave.Add(gam4);

        g.SaveChangesInSaveFile(allSave);*/
    }

    private void Update()
    {
        if(ProjectContext.instance != null)
        {
            if(InfoInFlyGame!= null)
            {
                InfoInFlyGame.text = "Mobility : " + GlobalPlayerInfo.playerInfoModel.mobility +
                    " DebuffSpeed: " +
                    " MaxSpeed: " + GlobalPlayerInfo.playerInfoModel.GetMaxPlayerSpeedWithUpdates();
            }
            if (setMaxSpeedFlyMode)
            {
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(1000);
            }
            //GlobalPlayerInfo.playerInfoModel.mobility = mobility;
            //GlobalPlayerInfo.playerInfoModel.baseDebuffSpeed = baseDebuffSpeed;
            if (TimeToFly != null && !ProjectContext.instance.PauseManager.IsPause)
            {
                timeSec += Time.deltaTime;
                TimeToFly.text = (timeSec).ToString();
            }
        }
    }


    private void IsPlayerOptionsChange()
    {
        /*if(DataBaseRepository.dataBaseRepository.playerFeaturesRepository.GetPlayerIsMusic())
        {
            audioSource.Play();
        } 
        else
        {
            audioSource.Stop();
        }*/
    }

    public void AnotherClick() 
    {
        playerRepo.AddPlayerMoney(50);
    }
    
    public void AddSpecMoney()
    {
        playerRepo.AddPlayerSpecialMoney(10);
    }
    
    public void AddLvl()
    {
        playerRepo.AddPlayerExperience(ExpForAdd);
    }
    
    public void RandomAch() 
    {
        var achsToUnlock = dataBase.AchievementRepos.allAchievementItems.Select(x => x.Id).Except(dataBase.AchievementRepos.saveAchievementItems.Select(x => x.Id));
        var idRand = UnityEngine.Random.Range(0, achsToUnlock.Count());
        WorldEventManager.worldManager.AchievementManager.UnlockAchievement(achsToUnlock.ToArray()[idRand]);
    }
    
    public void RestartFly()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void AddPlayerMobility(float value)
    {
        mobility += value;
    }
    public void AddMaxPlayerSpeed(int value)
    {
        GlobalPlayerInfo.playerInfoModel.AddMaxPlayerSpeed(value);
    }

    public void CleanAll() 
    {
        
    }
}
