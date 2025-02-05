using System.Transactions;
using UnityEngine;

public class InteractiveObjectBaseController : MonoBehaviour, IInteractiveObjectBase
{
    //Можно добавить всплывающие показатели того, что собирает пользователь 
    // private InfoPlayerPeekObj peekObj;
    [SerializeField]
    private InteractiveObjectModel model;
    [SerializeField]
    private PlayerItemsController playerItemsController;
    [SerializeField]
    private PlayerEffectController playerEffectController;
    [SerializeField]
    private ParticleSystem particleForDestroy;
    [SerializeField]
    private Animator animatorController;
    [SerializeField]
    private InteractiveObjectBaseController[] itemsNearMainObject = new InteractiveObjectBaseController[0];
    private Transform magnetTransform;

    public event ObjectTriggerEnter OnTriggerEnterInMainObject;
    public delegate void ObjectTriggerEnter(Collider collider);

    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    public void SetActiveItemsNearMainObject(bool isActive)
    {
        if (itemsNearMainObject?.Length > 0)
        {
            foreach (var item in itemsNearMainObject)
            {
                if(item != null)
                {
                    item.gameObject.SetActive(isActive);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (itemsNearMainObject?.Length > 0)
        {
            OnTriggerEnterInMainObject?.Invoke(collision);
        }
        if (collision.gameObject.tag == "Player")
        {
            foreach (var effect in model.Effects)
            {
                playerEffectController.AddEffect(effect);
            }
            playerItemsController.AddPlayerItems(model.ContaiterWithItems?.itemDatas);
            DestroyObject();
        }
    }

    private void Update()
    {
        if (isPaused)
        {
            return;
        }

        if (magnetTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, magnetTransform.position, 30f * Time.deltaTime);
        }
    }

    public void SetPlayerInfo(PlayerItemsController playerItemsController, PlayerEffectController playerEffectController)
    {
        this.playerItemsController = playerItemsController;
        this.playerEffectController = playerEffectController;
        if(itemsNearMainObject?.Length > 0)
        {
            foreach (var item in itemsNearMainObject)
            {
                item.SetPlayerInfo(playerItemsController, playerEffectController);
            }
            SetActiveItemsNearMainObject(true);
        } 
    }

    public void SetTargetToMove(Transform targetTransform)
    {
        this.magnetTransform = targetTransform;
        if (itemsNearMainObject?.Length > 0)
        {
            foreach (var item in itemsNearMainObject)
            {
                item.SetTargetToMove(this.magnetTransform);
            }
        }
    }

    public void DestroyObject()
    {
        GetComponent<Collider>().enabled = false;
        if(particleForDestroy != null)
        {
            particleForDestroy.Play();
        }
        if(animatorController != null)
        {
            animatorController.SetBool("isDestroy", true);
        }
        playerItemsController.CollideObject(model);
        Destroy(gameObject, 0.5f);
    }
}
