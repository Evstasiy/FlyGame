using Assets.Scripts.MainGame.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundLayerController : MonoBehaviour
{
    private List<LayerMove> activeLayerObjects = new List<LayerMove>();
    private LayerWorldModel actualLayerWorld;
    private BiomWorldModel activeBiom;

    private int maxLayersInPlace = 3;

    public void AddActiveLayer(GameObject layer)
    {
        var layerMove = layer.GetComponent<LayerMove>();
        activeLayerObjects.Add(layerMove);
    }

    public void ActiveBiomIsChanged(BiomWorldModel activeBiom)
    {
        this.activeBiom = activeBiom;

        //var allActiveOldBiomsCount = activeLayerObjects.Where(x => x.model.LayerName == actualLayerWorld.LayerName && x.gameObject.activeSelf);
        var allActiveOldBiomsCount = activeLayerObjects.Where(x => x.gameObject.activeSelf);
        foreach (var oldBiom in allActiveOldBiomsCount)
        {
            oldBiom.DisabledNow();
        }

        var allActiveLayersTypesCount = activeLayerObjects.Where(x => x.biomWorldModel.BiomName == activeBiom.BiomName && x.gameObject.activeSelf)?.Count() ?? 0;
        for (int i = allActiveLayersTypesCount; i < maxLayersInPlace; i++)
        {
            var lastActiveLayers = activeLayerObjects.Where(x => x.biomWorldModel.BiomName == activeBiom.BiomName && x.gameObject.activeSelf);
            //var lastActiveLayers = activeLayerObjects.Where(x => x.model.LayerName == actualLayerWorld.LayerName);
            float lastActiveZonePositionX = -400;
            if (lastActiveLayers?.Count() > 0)
            {
                lastActiveZonePositionX = lastActiveLayers.Max(x => x.transform.position.x) + actualLayerWorld.SizeLayerWidth;
            }
            var layerPos = new Vector3(lastActiveZonePositionX, actualLayerWorld.SpawnPosition.y, actualLayerWorld.SpawnPosition.z);
            var layerForUse = GetObjectInPool(activeBiom ,layerPos);

            }
    }

    public void ActiveZoneIsChanged(LayerWorldModel activeLayer)
    {
        actualLayerWorld = activeLayer;
    }

    private LayerMove GetObjectInPool(BiomWorldModel activeBiom, Vector3? spawnPos = null)
    {
        //Создавать объект после следующего, а не на точке спавна
        LayerMove layerCanUse = activeLayerObjects.FirstOrDefault(x => x.biomWorldModel.BiomName == activeBiom.BiomName && !x.gameObject.activeSelf);
        if (layerCanUse == null && activeLayerObjects.Where(x => x.biomWorldModel.BiomName == activeBiom.BiomName).Count() < maxLayersInPlace)
        { 
            layerCanUse = Instantiate(activeBiom.BackgrounLayerInfo.BackgrounObject, spawnPos ?? actualLayerWorld.SpawnPosition, Quaternion.identity, this.transform);
            layerCanUse.SetLayerAndBiomModel(actualLayerWorld, activeBiom);
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
        float spawnPositionX = 0;
        var activeLastBiom = activeLayerObjects.Where(x => x.biomWorldModel.BiomName == activeBiom.BiomName);
        if (activeLastBiom.Any())
        {
            spawnPositionX = activeLastBiom.Max(x => x.transform.position.x) + actualLayerWorld.SizeLayerWidth;
        }
        //spawnPositionX = activeLayerObjects.Where(x => x.model.LayerName == actualLayerWorld.LayerName).Max(x => x.transform.position.x) + actualLayerWorld.SizeLayerWidth;
        var spawnPoint = new Vector3(spawnPositionX, actualLayerWorld.SpawnPosition.y, actualLayerWorld.SpawnPosition.z);
        var layerForUse = GetObjectInPool(activeBiom, spawnPoint);
    }
}
