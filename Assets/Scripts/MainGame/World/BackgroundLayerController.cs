using Assets.Scripts.MainGame.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundLayerController : MonoBehaviour
{
    private List<LayerMove> activeLayerObjects = new List<LayerMove>();
    private LayerWorldModel actualLayerWorld;

    private int maxLayersInPlace = 3;

    public void AddActiveLayer(GameObject layer)
    {
        var layerMove = layer.GetComponent<LayerMove>();
        activeLayerObjects.Add(layerMove);
    }

    public void ActiveZoneIsChanged(LayerWorldModel activeLayer)
    {
        actualLayerWorld = activeLayer;

        var allActiveLayersTypesCount = activeLayerObjects.Where(x => x.model.LayerName == actualLayerWorld.LayerName && x.gameObject.activeSelf)?.Count() ?? 0;
        for (int i = allActiveLayersTypesCount; i < maxLayersInPlace; i++)
        {
            var lastActiveLayers = activeLayerObjects.Where(x => x.model.LayerName == actualLayerWorld.LayerName);
            float lastActiveZonePositionX = 0;
            if (lastActiveLayers?.Count() > 0)
            {
                lastActiveZonePositionX = lastActiveLayers.Max(x => x.transform.position.x) + actualLayerWorld.SizeLayerWidth;
            }
            var layerPos = new Vector3(lastActiveZonePositionX, activeLayer.BackgrounLayerInfo.BackgrounObject.model.SpawnPosition.y, activeLayer.BackgrounLayerInfo.BackgrounObject.model.SpawnPosition.z);
            var layerForUse = GetObjectInPool(actualLayerWorld, layerPos);
        }
    }

    private LayerMove GetObjectInPool(LayerWorldModel activeLayer, Vector3? spawnPos = null)
    {
        //Создавать объект после следующего, а не на точке спавна
        LayerMove layerCanUse = activeLayerObjects.FirstOrDefault(x => x.model.LayerName == activeLayer.LayerName && !x.gameObject.activeSelf);
        if (layerCanUse == null && activeLayerObjects.Where(x => x.model.LayerName == activeLayer.LayerName).Count() < maxLayersInPlace)
        { 
            layerCanUse = Instantiate(activeLayer.BackgrounLayerInfo.BackgrounObject, spawnPos ?? activeLayer.SpawnPosition, Quaternion.identity, this.transform);
            layerCanUse.SetLayerModel(activeLayer);
            activeLayerObjects.Add(layerCanUse);
            layerCanUse.OnLayerCanCreate += LayerCanCreateNew;
        } 
        else if (layerCanUse != null)
        {
            layerCanUse.gameObject.SetActive(true);
            if (spawnPos.HasValue)
            {
                layerCanUse.transform.position = spawnPos.Value;
            }
        }
        return layerCanUse;
    }

    private void LayerCanCreateNew(LayerEnum layerEnum)
    {
        var lastActiveZonePositionX = activeLayerObjects.Where(x=>x.model.LayerName == actualLayerWorld.LayerName).Max(x => x.transform.position.x) + actualLayerWorld.SizeLayerWidth;
        var spawnPoint = new Vector3(lastActiveZonePositionX, actualLayerWorld.SpawnPosition.y, actualLayerWorld.SpawnPosition.z);
        var layerForUse = GetObjectInPool(actualLayerWorld, spawnPoint);
    }
}
