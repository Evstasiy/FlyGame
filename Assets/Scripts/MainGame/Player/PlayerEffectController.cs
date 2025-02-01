using Assets.Scripts.MainGame.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField]
    private EffectItem[] presetEffectItemsList;

    [SerializeField]
    private GameObject EffectIconsPanel;
    private int maxEffectCount = 9;

    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    void Start()
    {
        presetEffectItems = presetEffectItemsList.ToDictionary(x => x.Key, y => y.ObjectEffect);
    }

    private Dictionary<EffectEnum, GameObject> presetEffectItems = new Dictionary<EffectEnum, GameObject>();

    private Dictionary<EffectObjectModel, EffectContainer> activeEffectCoroutines = new Dictionary<EffectObjectModel, EffectContainer>();

    public event ActiveEffectAdd IsActiveEffectAdd;
    public delegate void ActiveEffectAdd(EffectObjectController effectObjectController);

    public event ActiveEffectRemove IsActiveEffectRemove;
    public delegate void ActiveEffectRemove(EffectObjectController effectObjectController);

    public IReadOnlyCollection<EffectObjectModel> GetActiveEffectObjectModels()
    {
        return activeEffectCoroutines.Keys;
    }
    public void AddEffect(EffectObjectModel effect)
    {
        if (!activeEffectCoroutines.ContainsKey(effect) && activeEffectCoroutines.Count < maxEffectCount)
        {
            Coroutine effectCoroutine = null;
            if (!effect.IsEffectNotTime)
            {
                effectCoroutine = StartCoroutine(RemoveEffectAfterTime(effect, effect.GetFinalEffectLifeTime()));
            }
            var effectObjectController = effect.InstantiateEffectToPosition(EffectIconsPanel.transform);
            
            var newEffectContainer = new EffectContainer()
            {
                effectCoroutine = effectCoroutine,
                effectObjectController = effectObjectController
            };
            activeEffectCoroutines.Add(effect, newEffectContainer);
            if (presetEffectItems.ContainsKey(effect.EffectType))
            {
                presetEffectItems[effect.EffectType].SetActive(true);
                presetEffectItems[effect.EffectType].GetComponent<IStaticEffect>().SetEffectController(effectObjectController);
                effectObjectController.isEffectRomove += RemoveEffect;
            }
            IsActiveEffectAdd?.Invoke(effectObjectController);
        }
        else
        {
            activeEffectCoroutines[effect].effectObjectController.RestartEffect();
            Coroutine effectCoroutine = null;
            if (!effect.IsEffectNotTime)
            {
                StopCoroutine(activeEffectCoroutines[effect].effectCoroutine);
                effectCoroutine = StartCoroutine(RemoveEffectAfterTime(effect, effect.GetFinalEffectLifeTime()));
            }
            activeEffectCoroutines[effect].effectCoroutine = effectCoroutine;
        }
        if (!GlobalPlayerInfo.playerInfoModel.ActivePlayerEffects.Contains(effect.EffectType))
        {
            GlobalPlayerInfo.playerInfoModel.ActivePlayerEffects.Add(effect.EffectType);
        }
    }

    public void RemoveEffect(EffectObjectModel effect)
    {
        if (activeEffectCoroutines.TryGetValue(effect, out EffectContainer effectContainer))
        {
            if (presetEffectItems.ContainsKey(effect.EffectType))
            {
                presetEffectItems[effect.EffectType].SetActive(false);
            }
            if(effectContainer.effectCoroutine != null)
            {
                StopCoroutine(effectContainer.effectCoroutine);
            }
            IsActiveEffectRemove?.Invoke(effectContainer.effectObjectController);
            GlobalPlayerInfo.playerInfoModel.ActivePlayerEffects.Remove(effect.EffectType);
            Destroy(effectContainer.effectObjectController.gameObject);
            activeEffectCoroutines.Remove(effect);
        }
    }

    private IEnumerator RemoveEffectAfterTime(EffectObjectModel effect, float delay)
    {
        yield return WaitForSecondsWhileUnpaused(delay);
        RemoveEffect(effect);
    }

    private IEnumerator WaitForSecondsWhileUnpaused(float delay)
    {
        float elapsedTime = 0f;
        while (elapsedTime < delay)
        {
            if (!isPaused)
            {
                elapsedTime += Time.deltaTime;
            }
            yield return null;
        }
    }

    private class EffectContainer
    {
        public EffectObjectController effectObjectController;
        public Coroutine effectCoroutine;
    }
}


[System.Serializable]
public class EffectItem
{
    public EffectEnum Key;

    public GameObject ObjectEffect;
}
