using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.MainGame.World
{
    public class InteractiveLayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerItemsController playerItemsController;
        [SerializeField]
        private PlayerEffectController playerEffectController;

        public InteractiveObjectModel[] InteractiveObjectModels;
        private IEnumerable<InteractiveObjectModel> interactiveObjectModelsInLayer;

        private BiomWorldModel activeBiom;

        public float spawnInterval = 0.01f;

        public int maxSpawnPerStep = 10;

        private Coroutine coroutine = null;

        private List<(GameObject, InteractiveObjectModel)> activeInteractiveGameObjects = new List<(GameObject, InteractiveObjectModel)>();

        public delegate void NewInteractiveObjectSpawn(GameObject interactiveObject, InteractiveObjectModel interactiveObjectModel);
        public event NewInteractiveObjectSpawn IsNewInteractiveObjectSpawn;

        private IEnumerable<InteractiveObjectEnum> canSpawnTypes;
        //private IEnumerable<InteractiveObjectEnum> temporarySpawnTypes = new List<InteractiveObjectEnum>();
        private Dictionary<EffectEnum, IEnumerable<InteractiveObjectEnum>> temporaryEffectSpawnTypes = new Dictionary<EffectEnum, IEnumerable<InteractiveObjectEnum>>();
        private List<InteractiveObjectEnum> excludeSpawnTypes = new List<InteractiveObjectEnum>();

        public bool IsAutoSpawnObjects = true;
        private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

        private float lastPlayerPositionY;
        /// <summary>
        /// Диапазон для спавна объектов в высоту от позиции игрока
        /// </summary>
        private const float ZONE_Y_TARGET_SPAWN_ITEMS = 35;

        private void Start()
        {
            ProjectContext.instance.PlayerController.OnPlayerPositionYChange += PlayerPositionYChange;
        }

        public GameObject SpawnObjectByObjectTypeAndPosition(InteractiveObjectEnum objectType, Vector3 position)
        {
            var objectForSpawn = InteractiveObjectModels.FirstOrDefault(x => x.ObjectType == objectType);
            var newObj = SpawnObject(objectForSpawn, position);
            return newObj;
        }

        //FIX!! Должно быть универсально. Эта логика должна быть в управлении эффектами и там отслеживаться
        public void RemoveTemporarySpawnTypesByEffect(EffectEnum effectType)
        {
            if (temporaryEffectSpawnTypes.ContainsKey(effectType))
            {
                temporaryEffectSpawnTypes.Remove(effectType);
                if(temporaryEffectSpawnTypes.Count > 0)
                {
                    SetSpawnTypes(temporaryEffectSpawnTypes.SelectMany(x => x.Value));
                } 
                else
                {
                    SetDefaultSpawnTypesInActiveLayer();
                }
            }
        }

        public void AddTemporarySpawnTypesByEffect(EffectEnum effectType, IEnumerable<InteractiveObjectEnum> types)
        {
            temporaryEffectSpawnTypes.TryAdd(effectType, types);
            SetSpawnTypes(temporaryEffectSpawnTypes.SelectMany(x=>x.Value));
        }

        public void ExcludeTypeInSpawnObjects(InteractiveObjectEnum excludeType)
        {
            excludeSpawnTypes.Add(excludeType);
            if(canSpawnTypes != null && canSpawnTypes?.Count() > 0)
            {
                canSpawnTypes = canSpawnTypes.Where(x => x != excludeType);
                interactiveObjectModelsInLayer = this.activeBiom.InteractiveObjectsInZone.Where(x => canSpawnTypes.Any(y => y == x.ObjectType));
            }
        }

        public void SetDefaultSpawnTypesInActiveLayer()
        {
            temporaryEffectSpawnTypes = new Dictionary<EffectEnum, IEnumerable<InteractiveObjectEnum>>();
            SetSpawnTypes(this.activeBiom.InteractiveObjectsInZone.Select(x => x.ObjectType));
        }

        public void ActiveBiomIsChanged(BiomWorldModel activeBiom)
        {
            this.activeBiom = activeBiom;
            SetSpawnTypes(this.activeBiom.InteractiveObjectsInZone.Select(x => x.ObjectType));
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            coroutine = StartCoroutine(SpawnRoutine());
        }

        private void SetSpawnTypes(IEnumerable<InteractiveObjectEnum> types)
        {
            canSpawnTypes = types.Where(x => !excludeSpawnTypes.Any(y => y == x));
            if(temporaryEffectSpawnTypes.Count() > 0)
            {
                canSpawnTypes = types.Intersect(temporaryEffectSpawnTypes.SelectMany(x => x.Value));
            }
            interactiveObjectModelsInLayer = this.activeBiom.InteractiveObjectsInZone.Where(x => canSpawnTypes.Any(y => y == x.ObjectType));
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                if (isPaused || !IsAutoSpawnObjects)
                {
                    yield return new WaitForSeconds(0.2f);
                    continue;
                }

                var randomSpawnCount = Random.Range(1, maxSpawnPerStep);
                for (int i = 0; i < randomSpawnCount; i++)
                {
                    var itemToSpawn = GetInteractiveObjectByRandom();
                    SpawnObject(itemToSpawn);
                }
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private InteractiveObjectModel GetInteractiveObjectByRandom()
        {
            InteractiveObjectModel[] validItems = null;
            while(validItems == null) 
            {
                var chance = Random.Range(0, 100);
                var items = interactiveObjectModelsInLayer.Where(x => chance <= x.SpawnChancePercent).ToArray();
                validItems = (items.Length > 0) ? items : null;
            }

            InteractiveObjectModel itemToSpawn = null;

            var randomIndex = Random.Range(0, validItems.Length);
            itemToSpawn = validItems[randomIndex];

            return itemToSpawn;
        }

        private GameObject SpawnObject(InteractiveObjectModel interactiveObjectModel, Vector3? positionForSpawn = null)
        {
            Vector3 randomPosition = (positionForSpawn == null) ? GetSpawnObjectPosition() : positionForSpawn.Value;
            var target = Instantiate(interactiveObjectModel.Object, randomPosition, Quaternion.identity);
            /*можно вынести в InteractiveObjectModel и избежать GetComponent*/
            target.GetComponent<IInteractiveObjectBase>().SetPlayerInfo(playerItemsController, playerEffectController);
            activeInteractiveGameObjects.Add((target, interactiveObjectModel));
            IsNewInteractiveObjectSpawn?.Invoke(target, interactiveObjectModel);
            return target;
        }
        
        private Vector3 GetSpawnObjectPosition()
        {
            float spawnZoneYMin = lastPlayerPositionY - ZONE_Y_TARGET_SPAWN_ITEMS;
            float spawnZoneYMax = lastPlayerPositionY + ZONE_Y_TARGET_SPAWN_ITEMS;
            if(spawnZoneYMin <= ProjectContext.MIN_POS_Y + ProjectContext.STEP_TO_END_POS_Y)
            {
                spawnZoneYMin = ProjectContext.MIN_POS_Y + ProjectContext.STEP_TO_END_POS_Y;
                spawnZoneYMax += ProjectContext.STEP_TO_END_POS_Y;
            }
            else if(spawnZoneYMax >= ProjectContext.MAX_POS_Y - ProjectContext.STEP_TO_END_POS_Y)
            {
                spawnZoneYMin += ProjectContext.STEP_TO_END_POS_Y;
                spawnZoneYMax = ProjectContext.MAX_POS_Y - ProjectContext.STEP_TO_END_POS_Y;
            }
            float posX = Random.Range(140, 300);
            float randomY = Random.Range(spawnZoneYMin, spawnZoneYMax);
            Vector3 randomPosition = new Vector3(posX, randomY, 0);
            return randomPosition;
        }

        private void OnDestroy()
        {
            ProjectContext.instance.PlayerController.OnPlayerPositionYChange -= PlayerPositionYChange;
        }

        private void PlayerPositionYChange(float newPositionY)
        {
            lastPlayerPositionY = newPositionY;
        }
    }
}
