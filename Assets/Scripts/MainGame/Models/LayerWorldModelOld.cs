using System;
using UnityEngine;

namespace Assets.Scripts.MainGame.Models
{
    [Serializable]
    public class LayerWorldModelOld : MonoBehaviour
    {
        public LayerEnum LayerName;
        public Vector3 SpawnPosition;
        public float SpeedBraking;
        public BackgrounLayerInfo BackgrounLayerInfo;
        public bool IsActiveLayer = false;

        public float SizeLayerYMax;
        public float SizeLayerYMin;
        public float SizeLayerWidth;
    }
}
