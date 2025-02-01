using Assets.Scripts.MainGame.Models;
using Assets.Scripts.MainGame.Player;
using Assets.Scripts.SGEngine.DataBase.DataBaseModels;
using Assets.Scripts.SGEngine.DataBase.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerModel;

    [SerializeField]
    private ParticleSystem speedEffect;

    [SerializeField]
    private FogEffect fogEffect;

    public Gradient backgroundColorGradient;

    public AnimationCurve GravityCurve;

    public bool useGravityForPlayer = true;
    public float playerSpeedVertical = 10f;

    //public AnimationCurve liftCurve; // Кривая для управления подъемом и падением
    public float acceleration = 5f; // Скорость изменения скорости
    public float targetSpeed = 0f;

    public event PlayerPositionYChange OnPlayerPositionYChange;
    public delegate void PlayerPositionYChange(float newPositionY);

    public event PlayerDistanceChange OnPlayerDistanceChange;
    public delegate void PlayerDistanceChange(float newPlayerDistance);

    private float stepByEventChangeDistance = 10f;
    private float lastPlayerDistance = 0;

    /*public float anq = 140f;
    public float minAngle = 140f;
    public float maxAngle = 40f;*/
    private float stepByEventChangePositionY = 20f;
    private float lastPlayerPositionY = 0;

    private float verticalMovement;
    private float gravityNow;

    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    public void SetPlayerSkinById(int skinId)
    {
        var skinModel = DataBaseRepository.dataBaseRepository.SkinsRepo.allSkins.FirstOrDefault(x => x.Id == skinId);
        var loadedSkin = Resources.Load<GameObject>(skinModel.PathToSkin);
        var skinObj = Instantiate(loadedSkin, Vector3.zero, Quaternion.identity);
        skinObj.transform.SetParent(playerModel.transform, false);
        skinObj.GetComponent<Animator>().SetBool("IsFly",true);
    }
    public void OnPlayerUp()
    {
        float normalizedGravity = Mathf.InverseLerp(PlayerInfoModel.MAX_GRAVITY, PlayerInfoModel.MIN_GRAVITY, gravityNow);
        targetSpeed = Mathf.Lerp(0, GlobalPlayerInfo.playerInfoModel.FinalMobility, normalizedGravity);
    }

    public void OnPlayerDown()
    {
        targetSpeed = GlobalPlayerInfo.playerInfoModel.FinalMobility * -1;
    }

    public void OnPlayerTuchEnd()
    {
        targetSpeed = 0f;
    }

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        verticalMovement = 0f;
        OnPlayerPositionYChange?.Invoke(transform.position.y);
    }

    private void Start()
    {
        var startPlayerSpeed = (GlobalPlayerInfo.playerInfoModel.GetMaxPlayerSpeedWithUpdates() > 400) ? 400 : GlobalPlayerInfo.playerInfoModel.GetMaxPlayerSpeedWithUpdates();
        GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(startPlayerSpeed);
    }

    private void FixedUpdate()
    {
        if (isPaused) 
        {
            var main = speedEffect.main;
            main.startSpeed = 0;
            return;
        }
        GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(GlobalPlayerInfo.playerInfoModel.GetPlayerBaseDebuffSpeed());
        RotatePlayerModel(); 
    }
    private void Update()
    {
        if (isPaused)
        {
            var main = speedEffect.main;
            main.startSpeed = 0;
            return;
        }
        PlayerMode();
        CheckPlayerWeight();
        CheckPlayerDistance();
        PlaySpeedEffect();
        PlayFogEffect();
    }

    private void CheckPlayerDistance()
    {
        var playerDistance = Mathf.Round(GlobalPlayerInfo.playerInfoModel.PlayerDistance);
        if ((playerDistance % stepByEventChangeDistance) == 0 && lastPlayerDistance != playerDistance)
        {
            lastPlayerDistance = playerDistance;
            OnPlayerDistanceChange?.Invoke(lastPlayerDistance);
        }
    }

    private void CheckPlayerWeight()
    {
        var playerPositionY = Mathf.Round(transform.position.y);
        if ((playerPositionY % stepByEventChangePositionY) == 0 && lastPlayerPositionY != playerPositionY)
        {
            lastPlayerPositionY = playerPositionY;
            OnPlayerPositionYChange?.Invoke(lastPlayerPositionY);
        }
    }
    private void RotatePlayerModel()
    {
        // Определяем диапазоны для интерполяции
        float minMovement = -30f; // минимальное значение verticalMovement
        float maxMovement = 30f;  // максимальное значение verticalMovement
        float minAngle = 140f;
        float maxAngle = 40f;

        // Вычисляем целевой угол на основе verticalMovement
        float targetAngle = Mathf.Lerp(minAngle, maxAngle, (verticalMovement - minMovement) / (maxMovement - minMovement));

        // Задаём целевую ротацию
        Quaternion targetRotation = Quaternion.Euler(targetAngle , 90, 0);

        // Выполняем плавное вращение модели
        float rotationSpeed = 5f; // Скорость поворота
        playerModel.transform.rotation = Quaternion.Lerp(playerModel.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void PlayFogEffect()
    {
        var maxY = ProjectContext.MAX_POS_Y;
        float playerY = Mathf.Clamp(transform.position.y, 0, maxY);
        float t = playerY / maxY;
        fogEffect.fogColor = backgroundColorGradient.Evaluate(t);
    }

    private void PlaySpeedEffect()
    {
        float alpha = Mathf.Clamp01((GlobalPlayerInfo.playerInfoModel.FinalSpeed - 60) / (GlobalPlayerInfo.playerInfoModel.GetMaxPlayerSpeedWithUpdates() - 60));
        var main = speedEffect.main;
        var color = main.startColor.color;

        color.a = alpha; 
        main.startColor = color;

        float particleSpeed = Mathf.Lerp(15, 25, alpha);
        main.startSpeed = particleSpeed;
    }

    private void PlayerMode()
    {
        float normalizedSpeed = Mathf.Clamp01((GlobalPlayerInfo.playerInfoModel.FinalSpeed - PlayerInfoModel.MIN_SPEED) /
                                      (PlayerInfoModel.MAX_SPEED - PlayerInfoModel.MIN_SPEED));
        float curveValue = GravityCurve.Evaluate(normalizedSpeed);
        gravityNow = Mathf.Lerp(PlayerInfoModel.MAX_GRAVITY, PlayerInfoModel.MIN_GRAVITY, curveValue);
        gravityNow = Mathf.Clamp(gravityNow, PlayerInfoModel.MIN_GRAVITY, PlayerInfoModel.MAX_GRAVITY);

        if (Input.GetKey(KeyCode.W))
        {
            OnPlayerUp();
        } 
        else if (Input.GetKey(KeyCode.S))
        {
            OnPlayerDown();
        } 
        else if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            targetSpeed = 0f;
        }
        // debug
        /*if(Input.GetKey(KeyCode.D))
        {
            GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(1);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(-1);
        }*/

        verticalMovement = Mathf.Lerp(verticalMovement, targetSpeed, Time.deltaTime * acceleration);

        if (useGravityForPlayer)
        {
            verticalMovement -= gravityNow * Time.deltaTime;
        }

        if (verticalMovement != 0f)
        {
            Vector3 newPosition = transform.position + new Vector3(0, verticalMovement, 0) * Time.deltaTime;
            transform.position = newPosition;
        }
    }
}
