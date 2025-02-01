using Assets.Scripts.MainGame.World;
using JetBrains.Annotations;
using UnityEngine;

public class JetpackEffectController : MonoBehaviour, IStaticEffect
{
    [UsedImplicitly]
    public event IStaticEffect.RemoveEffect isEffectNeedRomove;
    [SerializeField]
    private InteractiveLayerController interactiveLayerController;

    private EffectObjectController effectObjectController;

    private InteractiveObjectEnum[] spawnTypesWhenEnable = new InteractiveObjectEnum[]
    {
            InteractiveObjectEnum.BigCoin,
            InteractiveObjectEnum.BigCristal,
            InteractiveObjectEnum.MassCoins
    };
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
