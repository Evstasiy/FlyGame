using Assets.Scripts.MainGame.Player;
using Assets.Scripts.MainGame.World;
using UnityEngine;

public class MagnetEffectController : MonoBehaviour, IStaticEffect
{
    private float radius = 0.5f;
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private InteractiveLayerController interactiveLayerController;
    private EffectObjectController effectObjectController;

    public event IStaticEffect.RemoveEffect isEffectNeedRomove;

    private InteractiveObjectEnum[] spawnTypesWhenEnable = new InteractiveObjectEnum[]
    {
        InteractiveObjectEnum.BigCoin,
        InteractiveObjectEnum.MassCoins,
        InteractiveObjectEnum.MassBoostSpeed
    };

    private void Start()
    {
        radius = GlobalPlayerInfo.playerInfoModel.GetMagnetRadius();
        transform.localScale = new Vector3(radius*2, radius*2, 3.3f);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "BuffItem")
        {
            collision.GetComponent<IInteractiveObjectBase>().SetTargetToMove(targetTransform);
        }
    }

    public void SetEffectController(EffectObjectController effectObjectController)
    {
        this.effectObjectController = effectObjectController;
        interactiveLayerController.AddTemporarySpawnTypesByEffect(effectObjectController.Model.EffectType, spawnTypesWhenEnable);
    }


    private void OnDisable()
    {
        interactiveLayerController.RemoveTemporarySpawnTypesByEffect(effectObjectController.Model.EffectType);
    }
}
