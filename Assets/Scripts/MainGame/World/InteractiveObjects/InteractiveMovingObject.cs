using Assets.Scripts.SGEngine.UserContent.AchievementFolder;
using UnityEngine;

public class InteractiveMovingObject : MonoBehaviour, IInteractiveObjectBase
{
    [SerializeField]
    private InteractiveObjectModel model;
    [SerializeField]
    private ParticleSystem particleForDestroy;

    public float PlayerTargetSpeed = 30f;
    public bool IsPlayerTarget;
    public bool IsTargetCanLost;
    public bool useTrajectory;
    public AnimationCurve trajectory;
    public float scale = 10f; // Коэффициент масштабирования
    public float xSpeed = 5f; // Скорость движения по оси X
    public float xOffset = 1f; // Постоянное смещение по оси X
    public bool moveRight = true; // Если true, объект будет двигаться вправо

    private Vector3 lastDirection; // Хранит последнее направление движения
    private bool movingForward = true; // Направление движения
    private bool isDead = false; // Направление движения
    private float timeElapsed = 0f;
    private Vector3 startPosition;

    [SerializeField]
    private PlayerItemsController playerItemsController;
    [SerializeField]
    private PlayerEffectController playerEffectController;
    [SerializeField]
    private InteractiveObjectBaseController[] itemsNearMainObject = new InteractiveObjectBaseController[0];

    private Transform targetTransform;
    private Collider thisCollider;
    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    public void DestroyObject()
    {
        thisCollider.enabled = false;
        xSpeed = 0.1f;
        particleForDestroy.Play();
        Destroy(gameObject, 0.5f);
        isDead = true;
    }

    public void SetPlayerInfo(PlayerItemsController playerItemsController, PlayerEffectController playerEffectController)
    {
        this.playerItemsController = playerItemsController;
        this.playerEffectController = playerEffectController;

        if (itemsNearMainObject?.Length > 0)
        {
            foreach (var item in itemsNearMainObject)
            {
                item.SetPlayerInfo(playerItemsController, playerEffectController);
            }
        }
    }

    void Start()
    {
        // Запоминаем начальную позицию объекта
        startPosition = transform.position;
        thisCollider = this.GetComponent<Collider>();
        if (IsPlayerTarget)
        {
            targetTransform = ProjectContext.instance.PlayerController.transform;
        }
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        transform.position = (isDead) ? GetNewPositionByDead() : GetNewPosition();

        if (transform.position.x < model.DestroyPositionX)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (itemsNearMainObject?.Length > 0)
            {
                foreach (var item in itemsNearMainObject)
                {
                    item.gameObject.SetActive(false); 
                }
            }

            foreach (var effect in model.Effects)
            {
                playerEffectController.AddEffect(effect);
            }
            playerItemsController.CollideObject(model);
            playerItemsController.AddPlayerItems(model.ContaiterWithItems?.itemDatas);
            DestroyObject();
        }
    }

    private Vector3 GetNewPositionByDead()
    {
        float x = transform.position.x + Time.deltaTime * 0.1f;
        float y = transform.position.y - Time.deltaTime * 0.1f;
        return new Vector3(x, y, transform.position.z);
    }

    private Vector3 GetNewPosition()
    {
        if (targetTransform != null)
        {
            if (IsTargetCanLost)
            {
                var distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
                if (distanceToTarget <= 9f || 8f >= transform.position.x)
                {
                    targetTransform = null;
                    return transform.position;
                }
            }

            lastDirection = (targetTransform.position - transform.position).normalized;
            var direction = lastDirection * -1;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50f * Time.deltaTime);
            
            return Vector3.MoveTowards(transform.position, targetTransform.position, PlayerTargetSpeed * Time.deltaTime);
        }
        if(lastDirection != Vector3.zero)
        {
            transform.position += lastDirection * PlayerTargetSpeed * Time.deltaTime;
            return transform.position;
        }
        
        // Увеличиваем или уменьшаем timeElapsed в зависимости от направления движения
        timeElapsed += (movingForward ? 1 : -1) * Time.deltaTime;

        // Увеличиваем или уменьшаем startPosition.x в зависимости от направления движения по X
        startPosition.x += (moveRight ? 1 : -1) * xOffset * Time.deltaTime;

        // Вычисляем новую позицию по оси X
        float x = startPosition.x + (timeElapsed * (xSpeed));
        float y = 0;
        if (useTrajectory)
        {
            float curveTime = trajectory.keys[trajectory.length - 1].time;

            // Проверяем, достиг ли timeElapsed конца или начала кривой и меняем направление движения
            if (timeElapsed > curveTime)
            {
                timeElapsed = curveTime;
                movingForward = false;
            } else if (timeElapsed < 0)
            {
                timeElapsed = 0;
                movingForward = true;
            }

            // Вычисляем новую позицию по оси Y, следуя кривой
            y = startPosition.y + trajectory.Evaluate(timeElapsed) * scale;
        } 
        else
        {
            y = startPosition.y + trajectory.Evaluate(timeElapsed) * scale;
        }

        return new Vector3(x, y, startPosition.z);
    }

    public void SetTargetToMove(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }
}
