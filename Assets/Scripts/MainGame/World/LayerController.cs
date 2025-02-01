using Assets.Scripts.MainGame.World;
using System.Collections.Generic;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerUIController playerUIController;
    [SerializeField]
    private InteractiveLayerController interactiveLayerController;
    [SerializeField]
    private BackgroundLayerController backgroudLayerController;
    [SerializeField]
    private PlayerController playerController;

    public event ActiveZoneChanged OnActiveZoneChanged;
    public delegate void ActiveZoneChanged(LayerWorldModel activeZone);

    public event ApproachedToNewActiveZone OnApproachedToNewActiveZone;
    public delegate void ApproachedToNewActiveZone(LayerWorldModel activeZone, bool isHigh);

    public List<LayerWorldModel> Layers;

    /// <summary>
    /// ƒиапазон дл€ оповещени€ о приближении к новому слою
    /// </summary>
    private const float ZONE_APPROACHED_Y_TO_CHANGE_LAYER = 20;

    void Start()
    {
        foreach (var layer in Layers)
        {
            layer.IsActiveLayer = false;
        }

        OnActiveZoneChanged += backgroudLayerController.ActiveZoneIsChanged;
        OnActiveZoneChanged += interactiveLayerController.ActiveZoneIsChanged;
        OnActiveZoneChanged += playerUIController.ActiveZoneIsChanged;
        OnApproachedToNewActiveZone += playerUIController.ApproachedToNewActiveZone;

        playerController.OnPlayerPositionYChange += PlayerPositionYChange;
        PlayerPositionYChange(playerController.transform.position.y);

    }

    public void PlayerPositionYChange(float newPositionY)
    {
        foreach (var layer in Layers)
        {
            if (newPositionY >= layer.SizeLayerYMin && newPositionY <= layer.SizeLayerYMax)
            {
                if (!layer.IsActiveLayer)
                {
                    layer.IsActiveLayer = true;
                    layer.BackgrounLayerInfo.CreateNewLayer = true;
                    OnActiveZoneChanged.Invoke(layer);
                    //Debug.Log("ActiveLayer: " + layer.LayerName + "\n maxZoneY: " + layer.SizeLayerYMax + " minZoneY: " + layer.SizeLayerYMin + "\n" + newPositionY);
                }
            } 
            else if(layer.IsActiveLayer)
            {
                layer.BackgrounLayerInfo.CreateNewLayer = false;
                layer.IsActiveLayer = false;
            }
            CheckApproachedToNewActiveZone(newPositionY, layer);
        }
    }

    private void OnDestroy()
    {
        OnActiveZoneChanged -= backgroudLayerController.ActiveZoneIsChanged;
        OnActiveZoneChanged -= interactiveLayerController.ActiveZoneIsChanged;
        OnActiveZoneChanged -= playerUIController.ActiveZoneIsChanged;
        OnApproachedToNewActiveZone -= playerUIController.ApproachedToNewActiveZone;

        playerController.OnPlayerPositionYChange -= PlayerPositionYChange;
    }

    private void CheckApproachedToNewActiveZone(float newPositionY, LayerWorldModel layer)
    {
        if (!layer.IsActiveLayer && (
                (newPositionY >= (layer.SizeLayerYMin - ZONE_APPROACHED_Y_TO_CHANGE_LAYER) && newPositionY <= layer.SizeLayerYMin) ||
                (newPositionY <= (layer.SizeLayerYMax + ZONE_APPROACHED_Y_TO_CHANGE_LAYER) && newPositionY >= layer.SizeLayerYMax)))
        {
            if (newPositionY >= (layer.SizeLayerYMin - ZONE_APPROACHED_Y_TO_CHANGE_LAYER) && newPositionY <= layer.SizeLayerYMin)
            {
                OnApproachedToNewActiveZone?.Invoke(layer, true);
            }
            else
            {
                OnApproachedToNewActiveZone?.Invoke(layer, false);
            }
        }
    }
}
