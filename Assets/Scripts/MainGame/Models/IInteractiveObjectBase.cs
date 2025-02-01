using UnityEngine;

public interface IInteractiveObjectBase 
{
    public void SetPlayerInfo(PlayerItemsController playerItemsController, PlayerEffectController playerEffectController);
    public void SetTargetToMove(Transform targetTransform);
}
