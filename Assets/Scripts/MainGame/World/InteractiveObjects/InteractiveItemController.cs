using Assets.Scripts.MainGame.Player;
using System.Buffers.Text;
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

    public bool IsDisabledItemsNearMainObject = true;

    public float MoveY = 0; //Движение вниз
    public float Spacing = 2f; // Отступ между объектами
    public float Amplitude = 4f; // Амплитуда синусоиды
    public float Frequency = 1f;  // Частота синусоиды


    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    private List<InteractiveObjectBaseController> activeInteractiveObjects = new List<InteractiveObjectBaseController>();

    void Start()
    {
        currentSpeedX = InteractiveObjectModel.MinSpeed;
        minSpawnCount = InteractiveObjectModel.MinSpawnCount;
        maxSpawnCount = InteractiveObjectModel.MaxSpawnCount;
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

        transform.Translate(new Vector3(-moveX, -MoveY, 0) * Time.deltaTime);
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
            var position = GetPositionBySpawnType(InteractiveObjectModel.SpawnType, i, objectCount);

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

    private Vector3 GetPositionBySpawnType(MassSpawnItemsTypes type, int counter, int allItemsCount = 0)
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
                var randomOperation = Random.Range(0, 10) >= 5 ? 1 : -1;
                x = transform.position.x + counter * Spacing;
                int peakPointLine = allItemsCount / 2;
                if (counter < peakPointLine)
                {
                    y = transform.position.y + counter * Spacing;
                }
                else
                {
                    y = transform.position.y -counter * Spacing;
                }
                break;
            case MassSpawnItemsTypes.DoubleLine:
                var lineSpacing = 5f;
                x = transform.position.x + counter * Spacing;
                y = transform.position.y + (counter % 2 == 0 ? lineSpacing : -lineSpacing);
                break;
            case MassSpawnItemsTypes.Roof:

                float horizontalSpacing = Spacing; // Шаг по горизонтали
                float verticalSpacing = Spacing;   // Шаг по вертикали

                x = x - counter * horizontalSpacing;

                int peakPoint = allItemsCount / 2;
                if (counter < peakPoint)
                {
                    // До середины увеличиваем Y
                    y = y + counter * verticalSpacing;
                }
                else
                {
                    // После середины уменьшаем Y
                    y = y + (allItemsCount - counter) * verticalSpacing;
                }

                break;
        }
        Vector3 position = new Vector3(x, y, 0);
        return position;
    }

    void ObjectTriggerEnter(Collider collider)
    {
        if((collider.gameObject.tag == "Player" || collider.gameObject.name == "ShieldObject") 
            && IsDisabledItemsNearMainObject)
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
