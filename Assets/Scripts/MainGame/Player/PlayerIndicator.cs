using Assets.Scripts.MainGame.World;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    private List<IndicatorContainer> targetsWithIndicators = new List<IndicatorContainer>();
    public GameObject newItemIndicatorIcon;

    public float indicatorRadius = 1.5f;
    public float targetRadius = 30f; 

    public float maxScale = 2f; // Максимальный масштаб индикатора
    public float minScale = 0.5f; // Минимальный масштаб индикатора

    [SerializeField]
    public PlayerEffectController playerEffectController;

    [SerializeField]
    public InteractiveLayerController interactiveLayerController;

    private Color colorWithAlpha = new Color(1, 1, 1, 0.7f);

    private void Start()
    {
        playerEffectController.IsActiveEffectAdd += PlayerAddEffect;
        playerEffectController.IsActiveEffectRemove += PlayerRemoveEffect;
        interactiveLayerController.IsNewInteractiveObjectSpawn += AddTarget;
    }

    void Update()
    {
        if (targetsWithIndicators != null && targetsWithIndicators.Count > 0)
        {
            for (int i = 0; i < targetsWithIndicators.Count; i++)
            {
                var item = targetsWithIndicators[i];
                // Проверяем, находится ли цель позади игрока
                if (item.Target == null || item.Target.position.x < transform.position.x)
                {
                    Destroy(item.IndicatorIconObject);
                    targetsWithIndicators.Remove(item);
                    continue;
                }

                Vector3 directionToTarget = item.Target.position - transform.position;
                directionToTarget.z = 0;

                // Ограничиваем направление радиусом
                directionToTarget = directionToTarget.normalized * indicatorRadius;

                // Устанавливаем позицию индикатора
                item.IndicatorIconObject.transform.position = transform.position + directionToTarget;

                /*Увелицение индикатора при приближении*/
                float distanceToTarget = Vector3.Distance(transform.position, item.Target.position);
                distanceToTarget = Mathf.Clamp(distanceToTarget, 0, targetRadius);
                float normalizedDistance = 1 - (distanceToTarget / targetRadius);
                float scale = Mathf.Lerp(minScale, maxScale, normalizedDistance);
                item.IndicatorIconObject.transform.localScale = new Vector3(scale, scale, 1f);

                /*Изменеение цвета при приближении*/
                Color color = Color.Lerp(colorWithAlpha, Color.white, normalizedDistance);
                item.IconImg.color = color;
                item.IconBackImg.color = color;
            }
        }
    }

    public void AddTarget(GameObject targetObject, InteractiveObjectModel interactiveObjectModel)
    {
        if(interactiveObjectModel.IsVisibleInPlayerIndicator)
        {
            var indicatorIconInstance = Instantiate(newItemIndicatorIcon, transform.position, Quaternion.identity, transform);
            /*FIX*/
            var iconIndicator = indicatorIconInstance.transform.GetChild(0).GetComponent<SpriteRenderer>();
            var iconBackIndicator = indicatorIconInstance.GetComponent<SpriteRenderer>();
            iconIndicator.sprite = interactiveObjectModel.IconImg;

            targetsWithIndicators.Add( new IndicatorContainer(targetObject.transform, indicatorIconInstance, iconBackIndicator, iconIndicator));
        }
    }

    private void PlayerAddEffect(EffectObjectController effectObjectController)
    {
        if(effectObjectController.Model.EffectType == EffectEnum.Jetpack)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void PlayerRemoveEffect(EffectObjectController effectObjectController)
    {
        if(effectObjectController.Model.EffectType == EffectEnum.Jetpack)
        {
            this.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        playerEffectController.IsActiveEffectAdd -= PlayerAddEffect;
        playerEffectController.IsActiveEffectRemove -= PlayerRemoveEffect;
        interactiveLayerController.IsNewInteractiveObjectSpawn -= AddTarget;
    }

    private class IndicatorContainer
    {
        public Transform Target;
        public GameObject IndicatorIconObject;
        public SpriteRenderer IconBackImg;
        public SpriteRenderer IconImg;
        public IndicatorContainer(Transform target, GameObject indicatorIconObject, SpriteRenderer iconBackImg, SpriteRenderer iconImg)
        {
            Target = target;
            IndicatorIconObject = indicatorIconObject;
            IconBackImg = iconBackImg;
            IconImg = iconImg;
        }
    }

}
