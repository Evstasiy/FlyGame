using UnityEngine;

public class ShieldEffectController : MonoBehaviour, IStaticEffect
{
    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private ParticleSystem shieldEnterEffect;

    private EffectObjectController effectObjectController;

    public event IStaticEffect.RemoveEffect isEffectNeedRomove;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "DebuffItem")
        {
            try
            {
                if(collision.TryGetComponent(out InteractiveMovingObject component))
                {
                    component.DestroyObject();
                }
                else if(collision.TryGetComponent(out InteractiveObjectBaseController componentBase))
                {
                    componentBase.DestroyObject();
                }
            } 
            catch
            {
                Destroy(collision.gameObject);
            } 
            finally
            {
                shieldEnterEffect.Play();
                effectObjectController.effectCountToEnd--;
                AudioController.Instance.PlayClip("HitShield");
            }
        }
        if(effectObjectController.effectCountToEnd == 0)
        {
            isEffectNeedRomove?.Invoke();
        }
    }

    public void SetEffectController(EffectObjectController effectObjectController)
    {
        this.effectObjectController = effectObjectController;
        isEffectNeedRomove += effectObjectController.NeedRemoveEffect;
    }
}