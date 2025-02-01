using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.UserContent.AchievementFolder;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class WorldEventManager : MonoBehaviour
{
    public static WorldEventManager worldManager = null;

    public AchievementManager AchievementManager { get; private set; }

    [SerializeField]
    private GameObject playerProfileUI;

    [SerializeField]
    private TutorialController tutorialController;

    [SerializeField]
    private SkinsView skinsView;

    private void Awake()
    {
        if (worldManager == null)
            worldManager = this;
        else if (worldManager == this)
            Destroy(gameObject);

        AchievementManager = new AchievementManager();

        skinsView.StartupSkins();
        tutorialController.CheckAndPlayTutorial();
    }

    private void Start()
    {
        playerProfileUI.SetActive(YG2.player.auth);
    }

    public void ResetSaves()
    {
        DataBaseRepository.dataBaseRepository.ResetSaves();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
