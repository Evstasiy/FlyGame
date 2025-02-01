using Assets.Scripts.MainGame.Player;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemsController : MonoBehaviour
{
    public delegate void ItemPickUp(ItemData item);
    public event ItemPickUp IsItemPickUp;

    public delegate void ObjectCollideToPlayer(InteractiveObjectModel interactiveObjectModel);
    public event ObjectCollideToPlayer IsObjectCollideToPlayer;

    public void CollideObject(InteractiveObjectModel interactiveObjectModel)
    {
        IsObjectCollideToPlayer?.Invoke(interactiveObjectModel);
        switch (interactiveObjectModel.ObjectType)
        {
            case InteractiveObjectEnum.BoostJetpack:
            case InteractiveObjectEnum.BoostMagnet:
            case InteractiveObjectEnum.BoostShield:
            case InteractiveObjectEnum.BoostSpeed:
                AudioController.Instance.PlayClip("GetBoost");
                break;

        }
    }

    public void AddPlayerItems(ItemData[] itemDatas)
    {
        if(itemDatas == null)
        {
            return;
        }
        foreach (var itemData in itemDatas)
        {
            IsItemPickUp?.Invoke(itemData);
            if (itemData.Key == ItemDataEnum.Coin)
            {
                GlobalPlayerInfo.playerInfoModel.AddPlayerCoins(itemData.Value);
                AudioController.Instance.PlayClip("GetCoin");
            }
            if (itemData.Key == ItemDataEnum.SpecialCoin)
            {
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpecialCoins(itemData.Value);
                AudioController.Instance.PlayClip("GetSpecialCoin");
            }
            if (itemData.Key == ItemDataEnum.Speed)
            {
                GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(itemData.Value);
                if (itemData.Value > 0)
                {
                    AudioController.Instance.PlayClip("GetSpeed");
                } 
                else
                {
                    //AudioController.Instance.PlayClip("MinusSpeed");
                    AudioController.Instance.PlayClip("Punch");
                }
            }
        }
    }

}
