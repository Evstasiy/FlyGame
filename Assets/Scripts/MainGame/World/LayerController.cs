using Assets.Scripts.MainGame.World;
using System.Collections.Generic;
using System.Linq;
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

    public event ActiveBiomChanged OnActiveBiomChanged;
    public delegate void ActiveBiomChanged(BiomWorldModel activeBiom);

    public event ApproachedToNewActiveZone OnApproachedToNewActiveZone;
    public delegate void ApproachedToNewActiveZone(LayerWorldModel activeZone, bool isHigh);

    public List<LayerWorldModel> Layers;

    /// <summary>
    /// Диапазон для оповещения о приближении к новому слою
    /// </summary>
    private const float ZONE_APPROACHED_Y_TO_CHANGE_LAYER = 20;

    /// <summary>
    /// Шаг в км для оповещения о изменении позоции игрока
    /// </summary>
    private const float STEP_BY_EVENT_CHANGE_DISTANCE = 200;

    private float playerPositionY;
    private float playerDistance;


    void Start()
    {
        foreach (var layer in Layers)
        {
            layer.IsActiveLayer = false;
            foreach (var biom in layer.Bioms)
            {
                biom.IsActive = false;
            }
        }

        OnActiveZoneChanged += backgroudLayerController.ActiveZoneIsChanged;
        OnActiveBiomChanged += backgroudLayerController.ActiveBiomIsChanged;
        OnActiveBiomChanged += interactiveLayerController.ActiveBiomIsChanged;
        OnActiveZoneChanged += playerUIController.ActiveZoneIsChanged;
        OnApproachedToNewActiveZone += playerUIController.ApproachedToNewActiveZone;

        playerController.OnPlayerPositionYChange += PlayerPositionYChange;
        playerController.OnPlayerDistanceChange += PlayerDistanceChange;

        PlayerPositionYChange(playerController.transform.position.y);
    }

    private void OnDestroy()
    {
        OnActiveZoneChanged -= backgroudLayerController.ActiveZoneIsChanged;
        OnActiveZoneChanged -= playerUIController.ActiveZoneIsChanged;
        OnActiveBiomChanged -= backgroudLayerController.ActiveBiomIsChanged;
        OnActiveBiomChanged -= interactiveLayerController.ActiveBiomIsChanged;
        OnApproachedToNewActiveZone -= playerUIController.ApproachedToNewActiveZone;

        playerController.OnPlayerPositionYChange -= PlayerPositionYChange;
        playerController.OnPlayerDistanceChange -= PlayerDistanceChange;
    }

    private void CheckAndChangeLayers()
    {
        var layerInActiveZone = Layers.FirstOrDefault(x => playerPositionY >= x.SizeLayerYMin && playerPositionY <= x.SizeLayerYMax);
        var activeLayerNow = Layers.FirstOrDefault(x => x.IsActiveLayer);
        
        if (!layerInActiveZone.IsActiveLayer)
        {
            if (activeLayerNow != null)
            {
                var activeBiomInLayer = activeLayerNow.Bioms.FirstOrDefault(x => x.IsActive);
                if(activeBiomInLayer != null)
                {
                    activeBiomInLayer.BackgrounLayerInfo.CreateNewLayer = false;
                    activeBiomInLayer.IsActive = false;
                }
                activeLayerNow.IsActiveLayer = false;
            }

            layerInActiveZone.IsActiveLayer = true;
            OnActiveZoneChanged.Invoke(layerInActiveZone);
            //Debug.Log("ActiveLayer: " + layerInActiveZone.LayerName + "\n maxZoneY: " + layerInActiveZone.SizeLayerYMax + " minZoneY: " + layerInActiveZone.SizeLayerYMin + "\n" + playerPositionY);
        }

        var activeLayerBiomIndex = (int)((playerDistance / layerInActiveZone.DistanceToChangeBiom) % layerInActiveZone.Bioms.Count());
        var biomInActiveDistance = layerInActiveZone.Bioms.FirstOrDefault(x => x.BiomIndex == activeLayerBiomIndex);
        var activeLayerBiomNow = layerInActiveZone.Bioms.FirstOrDefault(x => x.IsActive);
        if (biomInActiveDistance != null && !biomInActiveDistance.IsActive)
        {
            if (activeLayerBiomNow != null)
            {
                activeLayerBiomNow.BackgrounLayerInfo.CreateNewLayer = false;
                activeLayerBiomNow.IsActive = false;
            }
            biomInActiveDistance.BackgrounLayerInfo.CreateNewLayer = true;
            biomInActiveDistance.IsActive = true;
            OnActiveBiomChanged?.Invoke(biomInActiveDistance);
            //Debug.Log("ActiveBiom: " + biomInActiveDistance.BiomName + " Index: " + activeLayerBiomIndex);
        }

        foreach (var layer in Layers)
        {
            CheckApproachedToNewActiveZone(playerPositionY, layer);
        }
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

    private void PlayerPositionYChange(float newPositionY)
    {
        playerPositionY = newPositionY;
        CheckAndChangeLayers();
    }

    private void PlayerDistanceChange(float newPlayerDistance)
    {
        playerDistance = newPlayerDistance;
        if ((newPlayerDistance % STEP_BY_EVENT_CHANGE_DISTANCE) == 0)
        {
            CheckAndChangeLayers();
        }
    }
}
