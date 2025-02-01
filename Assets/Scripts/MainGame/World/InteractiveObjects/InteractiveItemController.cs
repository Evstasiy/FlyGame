using Assets.Scripts.MainGame.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractiveItemController : MonoBehaviour, IInteractiveObjectBase
{
    [SerializeField]
    private InteractiveObjectModel InteractiveObjectModel;

    [SerializeField]
    private InteractiveObjectBaseController InteractiveObjectForSpawn;

    private PlayerItemsController playerItemsController;
    private PlayerEffectController playerEffectController;

    private float currentSpeedX = 0;
    private int minSpawnCount;
    private int maxSpawnCount;

    public float Spacing = 6f; // Отступ между объектами
    public float Amplitude = 4f; // Амплитуда синусоиды
    public float Frequency = 1f;  // Частота синусоиды

    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    private List<InteractiveObjectBaseController> activeInteractiveObjects = new List<InteractiveObjectBaseController>();

    void Start()
    {
        currentSpeedX = InteractiveObjectModel.MinSpeed;
        minSpawnCount = InteractiveObjectModel.MinSpawnCount;
        maxSpawnCount = InteractiveObjectModel.MaxSpawnCount;
        Amplitude = Random.Range(1.0f, 6.0f);
        /*FIX*/
        if(InteractiveObjectModel.ObjectType == InteractiveObjectEnum.MassCoins)
        {
            bool isMagnet = GlobalPlayerInfo.playerInfoModel.ActivePlayerEffects.Any(x => x == EffectEnum.Magnet);
            //При магните не делать эффект джетпака
            if (isMagnet)
            {
                SetItemsByActiveEffect(EffectEnum.Magnet);
            }
            else
            {
                foreach (var effect in GlobalPlayerInfo.playerInfoModel.ActivePlayerEffects)
                {
                    SetItemsByActiveEffect(effect);
                }
            }
        }

        if (InteractiveObjectModel.MaxSpawnCount > 1)
        {
            var randomCount = Random.Range(minSpawnCount, maxSpawnCount);
            SpawnObjects(randomCount);
        }
    }

    private void Update()
    {
        if (isPaused)
        {
            return;
        }

        currentSpeedX = Mathf.Lerp(currentSpeedX, GlobalPlayerInfo.playerInfoModel.FinalSpeed, Time.deltaTime);
        float moveX = Mathf.Clamp(currentSpeedX, InteractiveObjectModel.MinSpeed, InteractiveObjectModel.MaxSpeed);
        
        transform.Translate(Vector3.left * moveX * Time.deltaTime);
        if (transform.position.x < InteractiveObjectModel.DestroyPositionX)
        {
            Destroy(gameObject);
        }
    }

    private void SetItemsByActiveEffect(EffectEnum effectType)
    {
        switch(effectType)
        {
            case EffectEnum.Magnet:
                minSpawnCount = (int)System.Math.Round(InteractiveObjectModel.MinSpawnCount + InteractiveObjectModel.MinSpawnCount * 0.7f);
                maxSpawnCount = (int)System.Math.Round(InteractiveObjectModel.MaxSpawnCount + InteractiveObjectModel.MaxSpawnCount * 0.7f);
                Spacing = 2f;
                break;
            
            case EffectEnum.Jetpack:
                minSpawnCount = (int)System.Math.Round(InteractiveObjectModel.MinSpawnCount + InteractiveObjectModel.MinSpawnCount * 0.4f);
                maxSpawnCount = (int)System.Math.Round(InteractiveObjectModel.MaxSpawnCount + InteractiveObjectModel.MaxSpawnCount * 0.4f);
                Amplitude = 6f; // Амплитуда синусоиды
                Frequency = 20f;  // Частота синусоиды
                break;
        }
    }

    void SpawnObjects(int objectCount)
    {
        for (int i = 0; i < objectCount; i++)
        {
            var position = GetPositionBySpawnType(InteractiveObjectModel.SpawnType, i);

            var interactiveObject = Instantiate(InteractiveObjectForSpawn, position, Quaternion.identity, this.transform);
            interactiveObject.SetPlayerInfo(playerItemsController, playerEffectController);
            interactiveObject.OnTriggerEnterInMainObject += ObjectTriggerEnter;
            activeInteractiveObjects.Add(interactiveObject);
        }
    }

    public void SetPlayerInfo(PlayerItemsController playerItemsController, PlayerEffectController playerEffectController)
    {
        this.playerItemsController = playerItemsController;
        this.playerEffectController = playerEffectController;
    }

    public void SetTargetToMove(Transform targetTransform)
    {
    }

    private Vector3 GetPositionBySpawnType(MassSpawnItemsTypes type, int counter)
    {
        float x = transform.position.x;
        float y = transform.position.y;
        switch (type)
        {
            case MassSpawnItemsTypes.Sin:
                x = transform.position.x + counter * Spacing;
                y = transform.position.y + Amplitude * Mathf.Sin(Frequency * x);
                break;
            case MassSpawnItemsTypes.Line:
                x = transform.position.x + counter * Spacing;
                y = transform.position.y + counter * Spacing;
                break;
        }
        Vector3 position = new Vector3(x, y, 0);
        return position;
    }

    void ObjectTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player" || collider.gameObject.name == "ShieldObject")
        {
            foreach (var item in activeInteractiveObjects)
            {
                item.SetActiveItemsNearMainObject(false);
            }
        }
    }

    private void OnDestroy()
    {
        
    }
}
